namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using System;

    using PhotoContest.Data.Interfaces;

    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Models;

    public abstract class Deadline : IDeadlineStrategy
    {
        public abstract void ApplyDeadlineStrategy(IPhotoContestData data, Contest contest, IRewardStrategy rewardStrategy);

        public static bool CheckDeadline(DateTime deadlineDate)
        {
            if (deadlineDate > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}