using PhotoContest.Models.Enums;

namespace PhotoContest.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoContest.App.Models.ViewModels.Picture;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Strategies;
    using PhotoContest.Models;

    public class PicturesController : BaseController
    {
        public PicturesController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var pictures =
                this.Data.Pictures.All()
                .Select(p => new PictureViewModel
                {
                    Url = p.Url,
                    User = p.User.UserName
                });

            return this.View(pictures);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Vote(int id)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());
            var picture = this.Data.Pictures.Find(id);

            try
            {
                if (picture.Contest.Status != ContestStatus.Active)
                {
                    throw new InvalidOperationException("The contest is closed.");
                }

                this.VotingStrategy =
                    StrategyFactory.GetVotingStrategy(picture.Contest.VotingStrategy.VotingStrategyType);

                this.VotingStrategy.CheckPermission(this.Data, user, picture.Contest);

                if (picture.Votes.Any(v => v.UserId == user.Id))
                {
                    throw new InvalidOperationException("You have already voted for this picture.");
                }

                var vote = new Vote { PictureId = picture.Id, UserId = user.Id };

                this.Data.Votes.Add(vote);
                this.Data.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                this.TempData["message"] = e.Message;

                this.Response.StatusCode = 400;
                return this.Content(e.ToString());
            }

            return this.Content(picture.Votes.Select(p => p.Id).Count().ToString());
        }
    }
}