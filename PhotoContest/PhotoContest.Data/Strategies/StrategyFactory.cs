namespace PhotoContest.Data.Strategies
{
    using System;
    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Models.Enums;

    using PhotoContest.Data.Strategies.DeadlineStrategy;
    using PhotoContest.Data.Strategies.ParticipationStrategy;
    using PhotoContest.Data.Strategies.VotingStrategy;

    public class StrategyFactory
    {
        public static IRewardStrategy GetRewardStrategy(RewardStrategyType rewardStrategyType)
        {
            switch (rewardStrategyType)
            {
                case RewardStrategyType.SingleWinner:
                    return new SingleWinnerStrategy();
                case RewardStrategyType.TopNPrizes:
                    return new TopNPrizesStrategy();
                default:
                    throw new InvalidOperationException("Strategy not found");
            }
        }

        public static IVotingStrategy GetVotingStrategy(VotingStrategyType votingStrategy)
        {
            switch (votingStrategy)
            {
                case VotingStrategyType.Open:
                    return null; 
                case VotingStrategyType.Closed:
                    return null;
                default:
                    throw new InvalidOperationException("Strategy not found");
            }
        }

        public static IParticipationStrategy GetParticipationStrategy(ParticipationStrategyType participationStrategyType)
        {
            switch (participationStrategyType)
            {
                case ParticipationStrategyType.Open:
                    return null;
                case ParticipationStrategyType.Closed:
                    return null;
                default:
                    throw new InvalidOperationException("Strategy not found");
            }
        }

        public static IDeadlineStrategy GetDeadlineStrategy(DeadlineStrategyType deadlineStrategyType)
        {
            switch (deadlineStrategyType)
            {
                case DeadlineStrategyType.ByTime:
                    return null;
                case DeadlineStrategyType.ByNumberOfParticipants:
                    return null;
                default:
                    throw new InvalidOperationException("Strategy not found");
            }
        }
    }
}
