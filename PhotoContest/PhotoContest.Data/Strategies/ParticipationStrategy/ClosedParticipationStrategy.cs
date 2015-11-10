namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using System.Linq;
    using PhotoContest.Models.Enums;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    using PhotoContest.Common.Exceptions;

    public class ClosedParticipationStrategy : IParticipationStrategy
    {
        public void CheckPermission(IPhotoContestData data, User user, Contest contest)
        {
            if (!contest.IsOpenForSubmissions)
            {
                throw new BadRequestException("The registration for this contest is closed.");
            }

            if (!contest.InvitedUsers.Contains(user))
            {
                throw new BadRequestException("The user is not selected to participate.");
            }

            var invitation = user.PendingInvitations.FirstOrDefault(i => i.ContestId == contest.Id);

            invitation.Status = InvitationStatus.Accepted;

            data.SaveChanges();
        }
    }
}