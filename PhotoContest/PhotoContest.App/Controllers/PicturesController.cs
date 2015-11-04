namespace PhotoContest.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Strategies;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;
    using System.Net;

    public class PicturesController : BaseController
    {
        public PicturesController(IPhotoContestData data)
            : base(data)
        {
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

                var votingStrategy =
                    StrategyFactory.GetVotingStrategy(picture.Contest.VotingStrategy.VotingStrategyType);

                votingStrategy.CheckPermission(this.Data, user, picture.Contest);

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

                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = e.ToString() });
            }

            return this.Content(picture.Votes.Select(p => p.Id).Count().ToString());
        }
    }
}