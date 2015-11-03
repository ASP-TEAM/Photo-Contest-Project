namespace PhotoContest.App.Controllers
{
    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;

    using PhotoContest.Data.Strategies.DeadlineStrategy;
    using PhotoContest.Data.Strategies.ParticipationStrategy;
    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Data.Strategies.VotingStrategy;

    public abstract class BaseController : Controller
    {
        private IPhotoContestData data;

        protected BaseController(IPhotoContestData data)
        {
            this.Data = data;
        }

        protected IRewardStrategy RewardStrategy { get; set; }
        protected IVotingStrategy VotingStrategy { get; set; }
        protected IParticipationStrategy ParticipationStrategy { get; set; }
        protected IDeadlineStrategy DeadlineStrategy { get; set; }

        protected IPhotoContestData Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }
    }
}