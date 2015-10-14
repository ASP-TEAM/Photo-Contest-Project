using System;

namespace PhotoContest.Data.Strategies.VotingStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class ClosedVotingStrategy : IVotingStrategy
    {
        public void Vote(Vote vote, IPhotoContestData data, User user, Contest contest)
        {
            if (contest.Committee.Contains(user))
            {
                data.Votes.Add(vote);
                data.SaveChanges();
            }
            else
            {
                throw new ArgumentException("User is not  in the voting committee.");
            }
        }
    }
}
