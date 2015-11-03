namespace PhotoContest.App.Controllers
{
    using System.IdentityModel;
    using System.Net;

    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models.Enums;
    using PhotoContest.Infrastructure.Interfaces;

    public class UsersController : BaseController
    {
        private IUsersService _service;

        public UsersController(IPhotoContestData data, IUsersService service)
            : base(data)
        {
            this._service = service;
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult AutoCompleteUsername(string searchTerm)
        {
            if (searchTerm.IsNullOrWhiteSpace())
            {
                return null;
            }

            var username = this.User.Identity.GetUserName();

            var matchingUsers = this.Data.Users.All()
                .Where(u => u.UserName != username && searchTerm.Length <= u.UserName.Length && u.UserName.ToLower().Substring(0, searchTerm.Length) == searchTerm.ToLower())
                .Select(u => u.UserName)
                .Take(5)
                .ToList();
            
            return this.Json(matchingUsers, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult IsUsernameAvailable(string username)
        {
            if (String.IsNullOrWhiteSpace(username) || !this.Data.Users.All().Any(u => u.UserName == username))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return this.Json("Username '" + username + "' is already taken!", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult IsEmailAvailable(string email)
        {
            if (String.IsNullOrWhiteSpace(email) || !this.Data.Users.All().Any(u => u.Email == email))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return this.Json("Email '" + email + "' is already taken!", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetNotifications()
        {
            var viewModel = this._service.GetNotifications(this.User.Identity.GetUserId());

            return this.PartialView("_Notifications", viewModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetNotification(int id)
        {
            var viewModel = this._service.GetNotification(this.User.Identity.GetUserId(), id);

            return this.PartialView("_Notification", viewModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ShowInvitation(int id)
        {
            try
            {
                var viewModel = this._service.ShowInvitation(id, this.User.Identity.GetUserId());

                return this.View("Invitation", viewModel);
            }
            catch (ArgumentException exception)
            {
                this.Response.StatusCode = (int) HttpStatusCode.NotFound;
                return this.Json(new {ErrorMessage = exception.Message});
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult AcceptInvitation(int id)
        {
            try
            {
                var invitationResult = this._service.AcceptInvitation(id, this.User.Identity.GetUserId());

                switch ((InvitationType) invitationResult["Type"])
                {
                    case InvitationType.ClosedContest:
                        return RedirectToAction("Join", "Contests", new { id = invitationResult["ContestId"] });
                    case InvitationType.Committee:
                        return RedirectToAction("JoinCommittee", "Contests", new { id = invitationResult["ContestId"] });
                }
            }
            catch (BadRequestException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = exception.Message }, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(200);
        }

        [Authorize]
        [HttpGet]
        public ActionResult RejectInvitation(int id)
        {
            try
            {
                this._service.RejectInvitation(id, this.User.Identity.GetUserId());
            }
            catch (BadRequestException exception)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new {ErrorMessage = exception.Message}, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(200);
        }
    }
}