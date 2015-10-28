namespace PhotoContest.Data.Strategies.VotingStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public interface IVotingStrategy
    {
        void CheckPermission(IPhotoContestData data, User user, Contest contest);
    }
}