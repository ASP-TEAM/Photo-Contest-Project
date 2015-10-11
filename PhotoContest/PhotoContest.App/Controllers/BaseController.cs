namespace PhotoContest.App.Controllers
{
    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;

    public abstract class BaseController : Controller
    {
        private IPhotoContestData data;

        protected BaseController(IPhotoContestData data)
        {
            this.Data = data;
        }

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