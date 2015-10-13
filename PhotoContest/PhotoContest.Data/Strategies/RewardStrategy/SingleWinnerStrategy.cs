using PhotoContest.Models;

namespace PhotoContest.Data.Strategies.RewardStrategy
{
    using System.Web.Mvc;
    using System.Linq;

    using PhotoContest.Data.Interfaces;

    public class SingleWinnerStrategy : IRewardStrategy
    {
        public void Reward(IPhotoContestData data, int contestId)
        {
            var winner = data.Contests.Find(contestId)
                .Participants
                .OrderByDescending(u => u.Pictures.Where(p => p.ContestId == contestId).Select(p => p.Votes.Count))
                .FirstOrDefault();
              
            data.ContestWinners.Add(new ContestWinners()
            {
                ContestId = contestId,
                Place = 1,
                WinnerId = winner.Id
            });

            data.SaveChanges();
        }
    }
}