using PhotoContest.Models.Enums;

namespace PhotoContest.App.Areas.Administration.Controllers
{
    using System;
    using System.Net;
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhotoContest.App.Areas.Administration.Models.BindingModels;
    using PhotoContest.App.Services;
    using PhotoContest.App.Areas.Administration.Models.ViewModels;
    using PhotoContest.Data;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class AdminController : BaseAdminController
    {
        private PicturesService _picturesService;

        public AdminController(IPhotoContestData data)
            : base(data)
        {
            this._picturesService = new PicturesService();
        }

        public ActionResult ManageContestPictures()
        {
            return this.View();
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
            return this.Data.Contests.All().Project().To<ManageContestViewModel>().ToList();
        }

        [HttpPost]
        public ActionResult ReadContests([DataSourceRequest]DataSourceRequest request)
        {
            var ads =
                this.GetContestsData()
                .ToDataSourceResult(request);

            return this.Json(ads);
        }

        [HttpPost]
        public ActionResult DestroyContest([DataSourceRequest]DataSourceRequest request, DeleteContestBindingModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var contest = this.Data.Contests.Find(model.Id);
                //contest.IsActive = false;
                contest.IsOpenForSubmissions = false;
                this.Data.Contests.Update(contest);
                this.Data.SaveChanges();
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult EditContest([DataSourceRequest]DataSourceRequest request, ManageContestViewModel model)
        {
            var contest = this.Data.Contests.Find(model.Id);
            contest.Status = model.Status;
            contest.Title = model.Title;
            contest.Description = model.Description;
            contest.IsOpenForSubmissions = model.IsOpenForSubmissions;
            contest.ParticipationStrategy.ParticipationStrategyType = model.ParticipationStrategyType;
            contest.VotingStrategy.VotingStrategyType = model.VotingStrategyType;
            contest.DeadlineStrategy.DeadlineStrategyType = model.DeadlineStrategyType;
            this.Data.Contests.Update(contest);
            this.Data.SaveChanges();

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpGet]
        public JsonResult SearchByUsername(string text)
        {
            var usersByCriteria =
                this.Data.Users.All()
                    .Where(u => u.UserName.ToLower().Contains(text.ToLower()))
                    .Project()
                    .To<ManageUserViewModel>()
                    .ToList();
            return this.Json(usersByCriteria, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUserDetails([DataSourceRequest] DataSourceRequest request, string searchUsers)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == searchUsers);
            var model = Mapper.Map<ManageUserViewModel>(user);
            return this.View("ManageUser", model);
        }

        public async Task<JsonResult> BanUser(ManageUserViewModel model)
        {
            var context = new PhotoContestDbContext();
            var store = new UserStore<User>(context);
            var manager = new UserManager<User>(store);
            await manager.SetLockoutEnabledAsync(model.Id, true);
            await manager.SetLockoutEndDateAsync(model.Id, DateTime.Now.AddDays(14));
            return this.Json(
                string.Format("Successfully locked user {0}", model.UserName),
                JsonRequestBehavior.AllowGet); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowContestPictures(ManageContestViewModel model)
        {
            var contest = this.Data.Contests.Find(model.Id);

            if (contest == null)
            {
                return this.HttpNotFound("Contest does not exists");
            }

            var viewModel =
                        contest.Pictures.AsQueryable()
                       .Project()
                       .To<ManageContestPicturesViewModel>()
                       .ToList();

            return this.PartialView("_ManageContestPicturesPartial", viewModel);
        }

        [HttpGet]
        public ActionResult GetActiveContests()
        {
            var activeContests =
                this.Data.Contests.All()
                    .Where(c => c.Status == ContestStatus.Active)
                    .Project()
                    .To<ManageContestViewModel>()
                    .ToList();

            return this.Json(activeContests, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeletePictureFromContest(int id)
        {
            var picture = this.Data.Pictures.Find(id);

            if (picture == null)
            {
                this.Response.StatusCode = 400;
                return this.Json("Picture not found", JsonRequestBehavior.AllowGet);
            }

            var googleDeleteResult = this._picturesService.DeleteImageFromGoogleDrive(picture.GoogleFileId);

            if (googleDeleteResult[0] != "success")
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(googleDeleteResult[1], JsonRequestBehavior.AllowGet);
            }

            picture.Contest.Pictures.Remove(picture);
            picture.User.Pictures.Remove(picture);

            this.Data.Pictures.Delete(picture);
            this.Data.SaveChanges();
            this.Response.StatusCode = 200;

            return this.Json("Successfully deleted picture!", JsonRequestBehavior.AllowGet);
        }
    }
}