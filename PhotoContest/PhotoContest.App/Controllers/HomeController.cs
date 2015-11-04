namespace PhotoContest.App.Controllers
{
    #region

    using System.Linq;
    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Infrastructure.Interfaces;

    #endregion

    public class HomeController : BaseController
    {
        private const int Top3NewstContests = 3;

        private IContestsService service;

        public HomeController(IPhotoContestData data, IContestsService service)
            : base(data)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            var contests = this.service.GetTopNewestContests(Top3NewstContests).ToList();
            return this.View(contests);
        }

    }
}