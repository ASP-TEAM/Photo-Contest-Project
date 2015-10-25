namespace PhotoContest.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;

    using PhotoContest.App.Models.ViewModels.Invitation;
    using PhotoContest.App.Models.ViewModels.User;
    using PhotoContest.Common;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;

    public class UsersController : BaseController
    {
        public UsersController(IPhotoContestData data)
            : base(data)
        {
        }

        [Authorize(Roles = GlobalConstants.AdminRole)]
        [HttpGet]
        public ActionResult All()
        {
            var allUsers =
                this.Data.Users.All()
                    .OrderByDescending(u => u.RegisteredAt)
                    .ThenBy(u => u.Id)
                    .ProjectTo<FullUserViewModel>()
                    .ToList();
            // In the end of the Linq methods after sort, where clauses etc. rather than doing .Select(..)
            // You need to write .Project().To<name of the view model class>()
            // There is 2 important automapper does not automatically materialize the collection, so you must do it mannually
            // And second automapper works only with Queryable
            return this.View(allUsers);
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
            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var notifications = loggedUser.PendingInvitations
                .Where(n => n.Status == InvitationStatus.Neutral)
                .OrderByDescending(n => n.DateOfInvitation)
                .Select(
                    n => new NotificationViewModel
                             {
                                InvitationId = n.Id,
                                Sender = n.Inviter.UserName,
                                Type = n.Type.ToString()
                             });

            return this.PartialView("_Notifications", notifications);
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

            var invitation = loggedUser.PendingInvitations.FirstOrDefault(i => i.Id == id);

            if (invitation == null)
            {
                this.Response.StatusCode = 400;
                return this.Content(string.Format("Invitation with id {0} does not exist", id));
            }

            if (invitation.Type == InvitationType.Committee)
            {
                // TODO
            }

            if (invitation.Type == InvitationType.ClosedContest)
            {
                // TODO
            }
            return new HttpStatusCodeResult(200);
        }

        [Authorize]
        [HttpGet]
        public ActionResult RejectInvitation(int id)
        {
            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var invitation = loggedUser.PendingInvitations.FirstOrDefault(i => i.Id == id);

            if (invitation == null)
            {
                this.Response.StatusCode = 400;
                return this.Content(string.Format("Invitation with id {0} does not exist", id));
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