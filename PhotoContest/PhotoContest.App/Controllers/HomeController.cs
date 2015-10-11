namespace PhotoContest.App.Controllers
{
    #region

    using System.Web.Mvc;

    #endregion

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}