namespace PhotoContest.App.Controllers
{
    using System.Web.Mvc;

    using Microsoft.AspNet.SignalR;

    using PhotoContest.App.Hubs;
    using PhotoContest.Data.Interfaces;

    using PhotoContest.Data.UnitOfWork;

    public abstract class BaseController : Controller
    {
        private IPhotoContestData _data;

        protected BaseController()
            :this(new PhotoContestData())
        {
        }

        protected BaseController(IPhotoContestData data)
        {
            this.Data = data;
            this.HubContext = GlobalHost.ConnectionManager.GetHubContext<PhotoContestHub>();
        }

        protected IHubContext HubContext { get; }

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