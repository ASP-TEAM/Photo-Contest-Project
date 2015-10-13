namespace PhotoContest.Data.Strategies.RewardStrategy
{
    using System.Web.Mvc;
    using PhotoContest.Data.Interfaces;

    public interface IRewardStrategy
    {
        void Reward(IPhotoContestData data, int contestId);
    }
}