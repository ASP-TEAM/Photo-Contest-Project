namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public interface IDeadlineStrategy
    {
        void CheckDeadline(IPhotoContestData data, Contest contest, User user);
    }
}