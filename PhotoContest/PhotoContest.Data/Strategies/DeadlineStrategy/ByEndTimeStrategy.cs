namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using System;
    using System.Linq;

    using PhotoContest.Data.Interfaces;

    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Models;

    public class ByEndTimeStrategy : IDeadlineStrategy
    {
        public void ApplyDeadlineStrategy(IPhotoContestData data, Contest contest, IRewardStrategy rewardStrategy)
        {
            if (this.CheckDeadline(contest.EndDate))
            {
                contest.IsActive = false;

                if (!data.ContestWinners.All().Any(cw => cw.ContestId == contest.Id))
                {
                    rewardStrategy.Reward(data, contest);
                }
            }
        }

        public virtual bool ParticipantsLimitReached(Contest contest)
        {
            return false;
        }

        private bool CheckDeadline(DateTime deadlineDate)
        {
            if (deadlineDate > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}