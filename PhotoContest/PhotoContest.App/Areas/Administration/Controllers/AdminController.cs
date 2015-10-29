namespace PhotoContest.App.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using PhotoContest.App.Models.ViewModels.Contest;
    using PhotoContest.App.Models.ViewModels.User;
    using PhotoContest.Data.Interfaces;

    public class AdminController : BaseAdminController
    {
        public AdminController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult ManageContests()
        {
            var contests = this.Data.Contests.All().Project().To<ContestViewModel>().ToList();
            return this.View();
        }

        public ActionResult ManageUsers()
        {
            return this.View();
        }

        protected IEnumerable GetContestsData()
        {
            return this.Data.Contests.All().Project().To<ContestViewModel>();
        }

        [HttpPost]
        public ActionResult ReadContests([DataSourceRequest]DataSourceRequest request)
        {
            var ads =
                this.GetContestsData()
                .ToDataSourceResult(request);

            return this.Json(ads);
        }

        protected IEnumerable GetUsersData()
        {
            return this.Data.Users.All().Project().To<FullUserViewModel>();
        }

        [HttpPost]
        public ActionResult ReadUsers([DataSourceRequest]DataSourceRequest request)
        {
            var ads =
                this.GetUsersData()
                .ToDataSourceResult(request);

            return this.Json(ads);
        }
    }
}