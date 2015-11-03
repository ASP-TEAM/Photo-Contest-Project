namespace PhotoContest.Infrastructure.Services
{
    using System.Linq;
    using PhotoContest.Infrastructure.Interfaces;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;
    using AutoMapper.QueryableExtensions;
    using PhotoContest.Infrastructure.Models.ViewModels.Invitation;
    using System.Collections.Generic;

    public class UsersService : BaseService, IUsersService
    {
        public IEnumerable<NotificationViewModel> GetNotifications(string userId)
        {
            var user = this.Data.Users.Find(userId);

            var notifications = user.PendingInvitations
                .Where(n => n.Status == InvitationStatus.Neutral)
                .OrderByDescending(n => n.DateOfInvitation)
                .AsQueryable()
                .ProjectTo<NotificationViewModel>().ToList();

            return notifications;
        }
    }
}