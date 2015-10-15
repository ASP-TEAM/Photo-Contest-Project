namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using System;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class ClosedParticipationStrategy : IParticipationStrategy
    {
        public void Participate(IPhotoContestData data, User user, Contest contest)
        {
            if (!contest.IsOpenForSubmissions)
            {
                throw new InvalidOperationException("The registration for this contest is closed.");
            }

            if (!contest.InvitedUsers.Contains(user))
            {
                // TODO send invitation to users
                throw new ArgumentException("The user is not selected to participate.");
            }
        }
    }
}
