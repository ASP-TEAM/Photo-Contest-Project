namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Models;

    public interface IDeadlineStrategy
    {
        void ApplyDeadlineStrategy(IPhotoContestData data, Contest contest, IRewardStrategy rewardStrategy);

        bool ParticipantsLimitReached(Contest contest);
    }
}