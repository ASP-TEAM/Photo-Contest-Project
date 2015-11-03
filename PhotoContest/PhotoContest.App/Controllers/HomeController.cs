namespace PhotoContest.App.Controllers
{
    #region

    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;

    #endregion

    public class HomeController : BaseController
    {

        public HomeController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

    }
}