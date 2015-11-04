namespace PhotoContest.Infrastructure.Services
{
    using System.Linq;
    using PhotoContest.Infrastructure.Interfaces;
    using PhotoContest.Models.Enums;
    using AutoMapper.QueryableExtensions;
    using PhotoContest.Infrastructure.Models.ViewModels.Invitation;
    using System.Collections.Generic;
    using System;
    using AutoMapper;
    using System.IdentityModel;

    using PhotoContest.Data.Interfaces;

    public class UsersService : BaseService, IUsersService
    {
        public UsersService(IPhotoContestData data)
            : base(data)
        {
        }

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

        public NotificationViewModel GetNotification(string userId, int invitationId)
        {
            var user = this.Data.Users.Find(userId);

            var notification = user.PendingInvitations.FirstOrDefault(n => n.Id == invitationId);

            if (notification == null)
            {
                throw new ArgumentException("Notification not found");
            }

            return Mapper.Map<NotificationViewModel>(notification);
        }

        public bool RejectInvitation(int id, string userId)
        {
            var user = this.Data.Users.Find(userId);

            var invitation = this.Data.Invitations.Find(id);

            if (invitation == null)
            {
                throw new BadRequestException(string.Format("Invitation with id {0} does not exist", id));
            }

            if (user.PendingInvitations.FirstOrDefault(i => i == invitation) == null)
            {
                throw new BadRequestException("You are not the recipient of this invitation");
            }

            if (invitation.Status != InvitationStatus.Neutral)
            {
                throw new BadRequestException(string.Format("Invitation with id {0} has been already {1}", id, invitation.Status));
            }

            invitation.Status = InvitationStatus.Rejected;

            this.Data.SaveChanges();

            return true;
        }

        public Dictionary<string, int> AcceptInvitation(int id, string userId)
        {
            var user = this.Data.Users.Find(userId);

            var invitation = this.Data.Invitations.Find(id);

            if (invitation == null)
            {
                throw new BadRequestException(string.Format("Invitation with id {0} does not exist", id));
            }

            if (user.PendingInvitations.FirstOrDefault(i => i == invitation) == null)
            {
                throw new BadRequestException("You are not the recipient of this invitation");
            }

            return new Dictionary<string, int>() { { "Type", (int) invitation.Type }, { "ContestId", invitation.ContestId } };
        }

        public InvitationViewModel ShowInvitation(int id, string userId)
        {
            var user = this.Data.Users.Find(userId);

            var notification = user.PendingInvitations.FirstOrDefault(n => n.Id == id);

            if (notification == null)
            {
                throw new ArgumentException("Notification not found");
            }

            return Mapper.Map<InvitationViewModel>(notification);
        }
    }
}