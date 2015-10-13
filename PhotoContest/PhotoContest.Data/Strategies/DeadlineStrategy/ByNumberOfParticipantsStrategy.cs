namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Models;

    public class ByNumberOfParticipantsStrategy : Deadline
    {
        public override void ApplyDeadlineStrategy(IPhotoContestData data, Contest contest, IRewardStrategy rewardStrategy)
        {
            
        }
    }
}