namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public interface IParticipationStrategy
    {
        void CheckPermission(IPhotoContestData data, User user, Contest contest);
    }
}