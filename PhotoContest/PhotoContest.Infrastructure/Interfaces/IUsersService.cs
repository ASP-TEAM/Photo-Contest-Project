namespace PhotoContest.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using PhotoContest.Infrastructure.Models.ViewModels.Invitation;

    public interface IUsersService
    {
        IEnumerable<NotificationViewModel> GetNotifications(string userId);
    }
}