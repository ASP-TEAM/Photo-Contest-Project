namespace PhotoContest.Data.Strategies.RewardStrategy
{
    using System.Linq;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class TopNPrizesStrategy : IRewardStrategy
    {
        private int topNPrizes;

        public TopNPrizesStrategy(int topNPrizes = 3)
        {
            this.topNPrizes = topNPrizes;
        }

        public void Reward(IPhotoContestData data, Contest contest)
        {
            var winners = contest
                        .Participants
                        .OrderByDescending(u => u.Pictures.Where(p => p.ContestId == contest.Id).Select(p => p.Votes.Count))
                        .ThenByDescending(u => u.UserName) // todo better order
                        .Take(this.topNPrizes);

            for (int i = 0; i < winners.Count(); i++)
            {
                data.ContestWinners.Add(new ContestWinners()
                {
                    ContestId = contest.Id,
                    Place = i + 1,
                    WinnerId = winners.ElementAt(i).Id
                });
            }

            data.SaveChanges();
        }
    }
}