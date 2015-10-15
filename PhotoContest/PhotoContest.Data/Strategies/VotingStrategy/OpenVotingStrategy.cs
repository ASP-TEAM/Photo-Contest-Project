namespace PhotoContest.Data.Strategies.VotingStrategy
{
    using System;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class OpenVotingStrategy : IVotingStrategy
    {
        public void Vote(IPhotoContestData data, User user, Contest contest)
        {
            if (contest.Participants.Contains(user))
            {
                throw new InvalidOperationException("You cannot vote for contest that you currently participate in.");
            }
        }
    }
}
