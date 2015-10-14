namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using System.Linq;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Models;

    public class ByNumberOfParticipantsStrategy : ByEndTimeStrategy
    {
        public override bool ParticipantsLimitReached(Contest contest)
        {
            if (contest.ParticipantsLimit == contest.Participants.Count)
            {
                return true;
            }

            return false;
        }
    }
}