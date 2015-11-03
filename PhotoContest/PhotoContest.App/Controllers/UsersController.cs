using PhotoContest.Infrastructure.Interfaces;
using PhotoContest.Infrastructure.Models.ViewModels.Invitation;

namespace PhotoContest.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;

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
            var viewModel = this._service.GetNotifications(this.User.Identity.GetUserId())
                            .AsQueryable().ProjectTo<NotificationViewModel>().ToList();

            return this.PartialView("_Notifications", viewModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetNotification(int id)
        {
            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var notification =
                loggedUser.PendingInvitations.Where(n => n.Id == id)
                    .Select(
                        n =>
                        new NotificationViewModel
                            {
                                InvitationId = n.Id,
                                Sender = n.Inviter.UserName,
                                Type = n.Type.ToString()
                            })
                    .FirstOrDefault();

            return this.PartialView("_Notification", notification);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ShowInvitation(int id)
        {
            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var invitation = loggedUser.PendingInvitations.FirstOrDefault(i => i.Id == id);

            if (invitation == null)
            {
                return this.HttpNotFound(string.Format("Invitation with id {0} does not exist", id));
            }

            var invitationView = Mapper.Map<Invitation, InvitationViewModel>(invitation);

            return this.View("Invitation", invitationView);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AcceptInvitation(int id)
        {
            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var invitation = this.Data.Invitations.Find(id);

            if (invitation == null)
            {
                this.Response.StatusCode = 400;
                return this.Content(string.Format("Invitation with id {0} does not exist", id));
            }

            if (loggedUser.PendingInvitations.FirstOrDefault(i => i == invitation) == null)
            {
                this.Response.StatusCode = 400;
                return this.Content("You are not the recipient of this invitation");
            }

            switch (invitation.Type)
            {
                case InvitationType.ClosedContest:
                    return RedirectToAction("Join", "Contests", new {id = invitation.ContestId});
                case InvitationType.Committee:
                    return RedirectToAction("JoinCommittee", "Contests", new { id = invitation.ContestId });
            }

            return new HttpStatusCodeResult(200);
        }

        [Authorize]
        [HttpGet]
        public ActionResult RejectInvitation(int id)
        {
            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var invitation = this.Data.Invitations.Find(id);

            if (invitation == null)
            {
                this.Response.StatusCode = 400;
                return this.Content(string.Format("Invitation with id {0} does not exist", id));
            }

            if (loggedUser.PendingInvitations.FirstOrDefault(i => i == invitation) == null)
            {
                this.Response.StatusCode = 400;
                return this.Content("You are not the recipient of this invitation");
            }

            if (invitation.Status != InvitationStatus.Neutral)
            {
                this.Response.StatusCode = 400;
                return this.Content(string.Format("Invitation with id {0} has been already {1}", id, invitation.Status));
            }

            invitation.Status = InvitationStatus.Rejected;

            this.Data.SaveChanges();

            return new HttpStatusCodeResult(200);
        }
    }
}