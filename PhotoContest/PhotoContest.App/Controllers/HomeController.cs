namespace PhotoContest.App.Controllers
{
    #region

    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoContest.App.Models.ViewModels.Invitation;
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