namespace PhotoContest.Data.Strategies.RewardStrategy
{
    using System.Web.Mvc;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public interface IRewardStrategy
    {
        void Reward(IPhotoContestData data, Contest contest);
    }
}