namespace PhotoContest.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using PhotoContest.Infrastructure.Models.ViewModels.Invitation;

    public interface IUsersService
    {
        IEnumerable<NotificationViewModel> GetNotifications(string userId);

        NotificationViewModel GetNotification(string userId, int invitationId);

        bool RejectInvitation(int id, string userId);

        Dictionary<string, int> AcceptInvitation(int id, string userId);

        InvitationViewModel ShowInvitation(int id, string userId);
    }
}