namespace PhotoContest.Data.Strategies.VotingStrategy
{
    using System;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class ClosedVotingStrategy : IVotingStrategy
    {
        public void Vote(IPhotoContestData data, User user, Contest contest)
        {
            if (!contest.Committee.Contains(user))
            {
                throw new InvalidOperationException("User is not in the voting committee.");
            }
        }
    }
}
