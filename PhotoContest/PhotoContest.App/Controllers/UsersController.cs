namespace PhotoContest.App.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.Ajax.Utilities;

    using PhotoContest.App.Models.ViewModels.User;
    using PhotoContest.Common;
    using PhotoContest.Data.Interfaces;

    public class UsersController : BaseController
    {
        public UsersController(IPhotoContestData data)
            : base(data)
        {
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public ActionResult All()
        {
            var allUsers =
                this.Data.Users.All()
                    .OrderByDescending(u => u.RegisteredAt)
                    .ThenBy(u => u.Id)
                    .Project()
                    .To<FullUserViewModel>()
                    .ToList();
            // In the end of the Linq methods after sort, where clauses etc. rather than doing .Select(..)
            // You need to write .Project().To<name of the view model class>()
            // There is 2 important automapper does not automatically materialize the collection, so you must do it mannually
            // And second automapper works only with Queryable
            return this.View(allUsers);
        }

        [HttpGet]
        public ActionResult AutoCompleteUsername(string searchTerm)
        {

            if (searchTerm.IsNullOrWhiteSpace())
            {
                return null;
            }

            var matchingUsers = this.Data.Users.All()
                .Where(u => u.UserName.Contains(searchTerm))
                .Select(u => u.UserName)
                .Take(5)
                .ToList();
            
            return this.Json(matchingUsers, JsonRequestBehavior.AllowGet);
        }
    }
}