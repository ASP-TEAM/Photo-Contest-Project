namespace PhotoContest.Data.Strategies.RewardStrategy
{
    using System.Linq;

    using PhotoContest.Data.Interfaces;

    using PhotoContest.Models;

    public class SingleWinnerStrategy : IRewardStrategy
    {
        public void ApplyReward(IPhotoContestData data, Contest contest)
        {
            var winner = contest
                        .Participants
                        .OrderByDescending(u => u.Pictures.Where(p => p.ContestId == contest.Id)
                        .Select(p => p.Votes.Count))
                        .FirstOrDefault();

            if (winner != null)
            {
                data.ContestWinners.Add(new ContestWinners()
                {
                    ContestId = contest.Id,
                    Place = 1,
                    WinnerId = winner.Id
                });

                data.SaveChanges();
            }  
        }
    }
}