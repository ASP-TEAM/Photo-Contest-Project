namespace PhotoContest.Data.Strategies.RewardStrategy
{
    using System.Web.Mvc;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public interface IRewardStrategy
    {
        void ApplyReward(IPhotoContestData data, Contest contest);
    }
}