namespace PhotoContest.Data.Strategies.VotingStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class OpenVotingStrategy : IVotingStrategy
    {
        public void Vote(Vote vote, IPhotoContestData data, User user, Contest contest)
        {
            data.Votes.Add(vote);
            data.SaveChanges();
        }
    }
}
