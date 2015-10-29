namespace PhotoContest.App.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper.QueryableExtensions;

    using PhotoContest.App.Models.ViewModels.User;
    using PhotoContest.Data.Interfaces;

    public class AdminController : BaseAdminController
    {
        public AdminController(IPhotoContestData data)
            : base(data)
        {
        }
        
        private IEnumerable<FullUserViewModel> GetAllUsers()
        {
            var allUsers =
                this.Data.Users.All()
                    .OrderByDescending(u => u.RegisteredAt)
                    .ThenBy(u => u.Id)
                    .ProjectTo<FullUserViewModel>()
                    .ToList();
            return allUsers;
        }
    }
}