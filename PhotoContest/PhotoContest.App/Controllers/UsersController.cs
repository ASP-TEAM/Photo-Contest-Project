namespace PhotoContest.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;

    using PhotoContest.App.Models.ViewModels.Invitation;
    using PhotoContest.App.Models.ViewModels.User;
    using PhotoContest.Common;
    using PhotoContest.Data.Interfaces;

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

            return this.Json("Username '" + username + "' is taken!", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetNotifications()
        {
            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var notifications =
                loggedUser.PendingInvitations.Select(
                    n => new NotificationViewModel
                    { Sender = n.Inviter.UserName, Type = n.Type.ToString() });

            return this.PartialView("_Notifications", notifications);
        }
    }
}