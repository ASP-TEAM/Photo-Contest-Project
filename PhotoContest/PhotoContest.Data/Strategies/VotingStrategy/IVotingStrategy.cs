namespace PhotoContest.Data.Strategies.VotingStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public interface IVotingStrategy
    {
        void Vote(Vote vote, IPhotoContestData data, User user, Contest contest);
    }
}