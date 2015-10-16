namespace PhotoContest.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoContest.App.Models.ViewModels;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Strategies;
    using PhotoContest.Models;

    public class PicturesController : BaseController
    {
        private const int MaxImageSize = 1000000;

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

        [HttpGet]
        [Authorize]
        public ActionResult Vote(int id)
        {
            // TODO change to HTTPPOST
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());
            var picture = this.Data.Pictures.Find(id);

            try
            {
                if (!picture.Contest.IsActive)
                {
                    throw new InvalidOperationException("The contest is closed.");
                }

                this.VotingStrategy =
                    StrategyFactory.GetVotingStrategy(picture.Contest.VotingStrategy.VotingStrategyType);

                this.VotingStrategy.Vote(this.Data, user, picture.Contest);

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
            }

            return null;
        }
    }
}