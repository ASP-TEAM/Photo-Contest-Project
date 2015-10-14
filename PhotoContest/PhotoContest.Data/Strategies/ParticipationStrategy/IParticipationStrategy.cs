namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public interface IParticipationStrategy
    {
        void SubmitPicture(Picture picture, IPhotoContestData data, User user, Contest contest);
    }
}