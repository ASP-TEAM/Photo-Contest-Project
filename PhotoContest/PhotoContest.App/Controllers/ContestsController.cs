namespace PhotoContest.App.Controllers
{
    #region
    using System.Net;
    using System.Web;

    using System.Web.Mvc;
    using PhotoContest.Infrastructure.Services;

    using Microsoft.AspNet.Identity;

    using PhotoContest.Data.Interfaces;

    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNet.SignalR;

    using PhotoContest.App.Hubs;
    using PhotoContest.Models.Enums;

    using PhotoContest.Infrastructure.Interfaces;

    using PhotoContest.Infrastructure.Models.BindingModels.Contest;
    using PhotoContest.Infrastructure.Models.BindingModels.Reward;
    using PhotoContest.Infrastructure.Models.ViewModels.Contest;
    using PhotoContest.Infrastructure.Models.ViewModels.Reward;

    using PhotoContest.Common.Exceptions;
    using PhotoContest.Infrastructure.Models.BindingModels.Invitation;

    #endregion

    public class ContestsController : BaseController
    {
        private PictureService _picturesService;

        private IContestsService _service;

        public ContestsController(IPhotoContestData data, IContestsService service)
            : base(data)
        {
            this._picturesService = new PictureService(data);
            this._service = service;
        }

        [HttpGet]
        public ActionResult Contests()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult AllContests()
        {
            var viewModel = _service.GetAllContests(this.User.Identity.GetUserId());

            return this.PartialView("_AllContestsPartial", viewModel);
        }

        [HttpGet]
        public ActionResult ActiveContests()
        {
            var viewModel = this._service.GetActiveContests(this.User.Identity.GetUserId());

            return this.PartialView("_ActiveContestsPartial", viewModel);
        }

        [HttpGet]
        public ActionResult InactiveContests()
        {
            var viewModel = this._service.GetInactiveContests();

            return this.PartialView("_InactiveContestsPartial", viewModel);
        }

        [System.Web.Mvc.Authorize]
        [HttpGet]
        public ActionResult MyContests()
        {
            var viewModel = this._service.GetMyContests(this.User.Identity.GetUserId());

            return this.PartialView("_MyContestsPartial", viewModel);
        }

        [System.Web.Mvc.Authorize]
        [HttpGet]
        public ActionResult GetRewardPartial()
        {
            return PartialView("~/Views/Rewards/_AddPartial.cshtml", new AddRewardViewModel());
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        public ActionResult AddRewards(int id, CreateRewardsBindingModel model)
        {
            if (model == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new {ErrorMessage = "Missing data"});
            }

            try
            {
                this._service.AddRewards(id, model, this.User.Identity.GetUserId());

                return this.RedirectToAction("PreviewContest", new { id = id });

            }
            catch (NotFoundException exception)
            {
                return this.HttpNotFound(exception.Message);
            }
            catch (BadRequestException exception)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new {ErrorMessage = exception.Message});
            }
        }

        [System.Web.Mvc.Authorize]
        [HttpGet]
        public ActionResult NewContest()
        {
            var viewModel = this._service.GetCreateContest();

            return this.View("NewContestForm", viewModel);
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateContest(CreateContestBindingModel model)
        {
            if (model == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = "Missing Data" });
            }

            if (!this.ModelState.IsValid)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            try
            {
                int contestId = this._service.CreateContest(model, this.User.Identity.GetUserId());

                return this.RedirectToAction("PreviewContest", new { id = contestId });
            }
            catch (NotFoundException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(new { ErrorMessage = exception.Message });
            }
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(int id)
        {
            if (this.Request.Files.Count < 1)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = "You must upload atleast one picture to participate" });
            }

            var files = new List<HttpPostedFileBase>();

            for (int i = 0; i < this.Request.Files.Count; i++)
            {
                var result = this._picturesService.ValidateImageData(this.Request.Files[i]);

                if (result != null)
                {
                    this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return this.Json(new { ErrorMessage = result });
                }

                files.Add(this.Request.Files[i]);
            }

            try
            {
                this._service.JoinContest(id, files, this.User.Identity.GetUserId());

                return this.RedirectToAction("PreviewContest", new { id = id });
            }
            catch (NotFoundException exception)
            {
                return this.HttpNotFound(exception.Message);
            }
            catch (BadRequestException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = exception.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (UnauthorizedException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return this.Json(new { ErrorMessage = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [System.Web.Mvc.Authorize]
        [HttpGet]
        public ActionResult JoinCommittee(int id)
        {
            try
            {
                this._service.JoinContestCommittee(id, this.User.Identity.GetUserId());
            }
            catch (BadRequestException exception)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new {ErrorMessage = exception.Message});
            }

            return this.RedirectToAction("PreviewContest", new {id = id});
        }

        [HttpGet]
        public ActionResult PreviewContest(int id)
        {
            var contestViewModel = this._service.GetPreviewContest(id, this.User.Identity.GetUserId());

            if (contestViewModel.Status != ContestStatus.Active)
            {
                return this.View("PreviewInactiveContest", (PreviewInactiveContestViewModel) contestViewModel);
            }

            return this.View((PreviewContestViewModel)contestViewModel);
        }

        [System.Web.Mvc.Authorize]
        [HttpGet]
        public ActionResult ManageContest(int id)
        {
            try
            {
                var viewModel = this._service.GetManageContest(id, this.User.Identity.GetUserId());

                return this.View("ManageContestForm", viewModel);
            }
            catch (NotFoundException exception)
            {
                return this.HttpNotFound(exception.Message);
            }
            catch (UnauthorizedException exception)
            {
                this.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                return this.Json(new { ErrorMessage = exception.Message });
            }
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateContest(UpdateContestBindingModel model)
        {
            if (model == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = "Missing Data" } );
            }

            if (!this.ModelState.IsValid)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            try
            {
                this._service.UpdateContest(model, this.User.Identity.GetUserId());

                return this.RedirectToAction("ManageContest", new { id = model.Id });
            }
            catch (NotFoundException exception)
            {
                return this.HttpNotFound(exception.Message);
            }
            catch (UnauthorizedException exception)
            {
                this.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                return this.Json(new { ErrorMessage = exception.Message });
            }
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InviteUser(CreateInvitationBindingModel model)
        {
            if (model == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new {ErrorMessage = "Missing data"});
            }

            if (!this.ModelState.IsValid)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
            }

            try
            {
                var invitationId = this._service.InviteUser(model, this.User.Identity.GetUserId());

                var hub = GlobalHost.ConnectionManager.GetHubContext<PhotoContestHub>();
                hub.Clients.User(model.Username).notificationReceived(invitationId);
            }
            catch (NotFoundException exception)
            {
                this.Response.StatusCode = (int) HttpStatusCode.NotFound;
                return this.Json(new {ErrorMessage = exception.Message});
            }
            catch (BadRequestException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = exception.Message });
            }

            this.Response.StatusCode = (int)HttpStatusCode.OK;
            return this.Json(new { SuccessfulMessage = string.Format("User with username {0} successfully invited", model.Username) });
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        public ActionResult FinalizeContest(int id)
        {
            try
            {
                this._service.FinalizeContest(id, this.User.Identity.GetUserId());
            }
            catch (NotFoundException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(new { ErrorMessage = exception.Message });
            }
            catch (BadRequestException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = exception.Message });
            }
            catch (UnauthorizedException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return this.Json(new { ErrorMessage = exception.Message });
            }

            return this.RedirectToAction("PreviewContest", new { id = id });
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        public ActionResult DismissContest(int id)
        {
            try
            {
                this._service.DismissContest(id, this.User.Identity.GetUserId());
            }
            catch (NotFoundException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(new { ErrorMessage = exception.Message });
            }
            catch (BadRequestException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = exception.Message });
            }
            catch (UnauthorizedException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return this.Json(new { ErrorMessage = exception.Message });
            }

            return this.RedirectToAction("PreviewContest", new { id = id });
        }
    }
}