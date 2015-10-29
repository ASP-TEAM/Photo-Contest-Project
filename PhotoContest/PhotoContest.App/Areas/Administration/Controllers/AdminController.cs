namespace PhotoContest.App.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using PhotoContest.App.Areas.Administration.Models.BindingModels;
    using PhotoContest.App.Models.BindingModels.Contest;
    using PhotoContest.App.Models.ViewModels;
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
            return this.View();
        }

        public ActionResult ManageUsers()
        {
            return this.View();
        }

        protected IEnumerable GetContestsData()
        {
            return this.Data.Contests.All().Project().To<ContestViewModel>().ToList();
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
        public ActionResult DestroyContest([DataSourceRequest]DataSourceRequest request, DeleteContestBindingModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var contest = this.Data.Contests.Find(model.Id);
                contest.IsActive = false;
                contest.IsOpenForSubmissions = false;
                this.Data.Contests.Update(contest);
                this.Data.SaveChanges();
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult EditContest([DataSourceRequest]DataSourceRequest request, UpdateContestBindingModel model)
        {
            var contest = this.Data.Contests.Find(model.Id);
            contest.IsActive = model.IsActive;
            
            contest.EndDate = model.EndDate;
            contest.Description = model.Description;
            this.Data.Contests.Update(contest);
            this.Data.SaveChanges();

            return this.RedirectToAction("ReadContests");
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