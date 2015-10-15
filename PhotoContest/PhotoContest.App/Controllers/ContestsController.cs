namespace PhotoContest.App.Controllers
{
    using System;

    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoContest.App.Models.BindingModels.Contest;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Strategies;

    public class ContestsController : BaseController
    {
        public ContestsController(IPhotoContestData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult Contest(int id)
        {
            return null;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContestBindingModel contestModel)
        {
            return null;
        }

        [HttpGet]
        public ActionResult Join(int id)
        {
            var currentUser = this.User.Identity.GetUserId();
            var user = this.Data.Users.Find(currentUser);
            var contest = this.Data.Contests.Find(id);

            this.DeadlineStrategy = StrategyFactory.GetDeadlineStrategy(contest.DeadlineStrategy.DeadlineStrategyType);

            this.DeadlineStrategy.Deadline(this.Data, contest, user);

            this.ParticipationStrategy =
                StrategyFactory.GetParticipationStrategy(contest.ParticipationStrategy.ParticipationStrategyType);

            this.ParticipationStrategy.Participate(this.Data, user, contest);

            if (contest.Participants.Contains(user))
            {
                throw new ArgumentException("You already participate in this contest");
            }

            contest.Participants.Add(user);
            this.Data.Contests.Update(contest);
            this.Data.SaveChanges();

            return null;
        }
    }
}