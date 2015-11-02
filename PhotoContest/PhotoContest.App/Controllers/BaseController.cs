namespace PhotoContest.App.Controllers
{
    using System.Web.Mvc;

    using Microsoft.AspNet.SignalR;

    using PhotoContest.App.Hubs;
    using PhotoContest.Data;
    using PhotoContest.Data.Interfaces;

    using PhotoContest.Data.Strategies.DeadlineStrategy;
    using PhotoContest.Data.Strategies.ParticipationStrategy;
    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Data.Strategies.VotingStrategy;
    using PhotoContest.Data.UnitOfWork;

    public abstract class BaseController : Controller
    {
        private IPhotoContestData data;

        protected BaseController()
            :this(new PhotoContestData())
        {
        }

        protected BaseController(IPhotoContestData data)
        {
            this.Data = data;
            this.HubContext = GlobalHost.ConnectionManager.GetHubContext<PhotoContestHub>();
        }

        protected IRewardStrategy RewardStrategy { get; set; }
        protected IVotingStrategy VotingStrategy { get; set; }
        protected IParticipationStrategy ParticipationStrategy { get; set; }
        protected IDeadlineStrategy DeadlineStrategy { get; set; }
        protected IHubContext HubContext { get; }

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