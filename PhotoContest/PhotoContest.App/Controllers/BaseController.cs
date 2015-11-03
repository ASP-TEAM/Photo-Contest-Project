namespace PhotoContest.App.Controllers
{
    using System.Web.Mvc;

<<<<<<< HEAD
    using Microsoft.AspNet.SignalR;

    using PhotoContest.App.Hubs;
    using PhotoContest.Data.Interfaces;

    using PhotoContest.Data.UnitOfWork;
=======
    using PhotoContest.Data.Interfaces;

    using PhotoContest.Data.Strategies.DeadlineStrategy;
    using PhotoContest.Data.Strategies.ParticipationStrategy;
    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Data.Strategies.VotingStrategy;
>>>>>>> 8577aeb5c56251899283d8d78526594818d2d534

    public abstract class BaseController : Controller
    {
        private IPhotoContestData _data;

        protected BaseController(IPhotoContestData data)
        {
            this.Data = data;
        }

<<<<<<< HEAD
        protected IHubContext HubContext { get; }
=======
        protected IRewardStrategy RewardStrategy { get; set; }
        protected IVotingStrategy VotingStrategy { get; set; }
        protected IParticipationStrategy ParticipationStrategy { get; set; }
        protected IDeadlineStrategy DeadlineStrategy { get; set; }
>>>>>>> 8577aeb5c56251899283d8d78526594818d2d534

        protected IPhotoContestData Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }
    }
}