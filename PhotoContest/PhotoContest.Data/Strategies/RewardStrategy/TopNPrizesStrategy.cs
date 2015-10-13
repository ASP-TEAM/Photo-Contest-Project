namespace PhotoContest.Data.Strategies.RewardStrategy
{
    using System.Linq;
    using PhotoContest.Models;

    using PhotoContest.Data.Interfaces;

    public class TopNPrizesStrategy : IRewardStrategy
    {
        private int topNPrizes;

        public TopNPrizesStrategy(int topNPrizes = 3)
        {
            this.topNPrizes = topNPrizes;
        }

        public void Reward(IPhotoContestData data, int contestId)
        {
            var winners = data.Contests.Find(contestId)
                .Participants
                .OrderByDescending(u => u.Pictures.Where(p => p.ContestId == contestId).Select(p => p.Votes.Count))
                .ThenByDescending(u => u.UserName) // todo better order
                .Take(this.topNPrizes);

            for (int i = 0; i < winners.Count(); i++)
            {
                data.ContestWinners.Add(new ContestWinners()
                {
                    ContestId = contestId,
                    Place = i + 1,
                    WinnerId = winners.ElementAt(i).Id
                });
            }

            data.SaveChanges();
        }
    }
}