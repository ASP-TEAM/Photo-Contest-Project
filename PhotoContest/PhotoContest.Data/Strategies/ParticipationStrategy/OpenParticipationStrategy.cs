namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class OpenParticipationStrategy : IParticipationStrategy
    {
        public void SubmitPicture(Picture picture, IPhotoContestData data, User user, Contest contest)
        {
            contest.Participants.Add(user);
            contest.Pictures.Add(picture);

            data.SaveChanges();
        }
    }
}
