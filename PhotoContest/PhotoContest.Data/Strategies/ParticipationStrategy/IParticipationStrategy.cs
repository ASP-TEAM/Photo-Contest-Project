namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public interface IParticipationStrategy
    {
        void Participate(IPhotoContestData data, User user, Contest contest);
    }
}