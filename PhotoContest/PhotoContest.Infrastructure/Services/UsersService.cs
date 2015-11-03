namespace PhotoContest.Infrastructure.Services
{
    using System.Linq;
    using PhotoContest.Infrastructure.Interfaces;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;

    public class UsersService : BaseService, IUsersService
    {
        public IQueryable<Invitation> GetNotifications(string userId)
        {
            var user = this.Data.Users.Find(userId);

            var notifications = user.PendingInvitations
                .Where(n => n.Status == InvitationStatus.Neutral)
                .OrderByDescending(n => n.DateOfInvitation)
                .AsQueryable();

            return notifications;
        }
    }
}