using System.Linq;
using PhotoContest.Models;

namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Strategies.RewardStrategy;

    public class ByEndTimeStrategy : Deadline
    {
        public override void ApplyDeadlineStrategy(IPhotoContestData data, Contest contest, IRewardStrategy rewardStrategy)
        {
            if (CheckDeadline(contest.EndDate))
            {
                contest.IsActive = false;

                if (!data.ContestWinners.All().Any(cw => cw.ContestId == contest.Id))
                {
                    rewardStrategy.Reward(data, contest);
                }
            }
        }
    }
}