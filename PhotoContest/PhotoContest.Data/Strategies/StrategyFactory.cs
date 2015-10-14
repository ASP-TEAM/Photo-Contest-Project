namespace PhotoContest.Data.Strategies
{
    using System;

    using PhotoContest.Data.Strategies.DeadlineStrategy;
    using PhotoContest.Data.Strategies.ParticipationStrategy;
    using PhotoContest.Data.Strategies.RewardStrategy;
    using PhotoContest.Data.Strategies.VotingStrategy;
    using PhotoContest.Models.Enums;

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
                    return new OpenVotingStrategy(); 
                case VotingStrategyType.Closed:
                    return new ClosedVotingStrategy();
                default:
                    throw new InvalidOperationException("Strategy not found");
            }
        }

        public static IParticipationStrategy GetParticipationStrategy(ParticipationStrategyType participationStrategyType)
        {
            switch (participationStrategyType)
            {
                case ParticipationStrategyType.Open:
                    return new OpenParticipationStrategy();
                case ParticipationStrategyType.Closed:
                    return new ClosedParticipationStrategy();
                default:
                    throw new InvalidOperationException("Strategy not found");
            }
        }

        public static IDeadlineStrategy GetDeadlineStrategy(DeadlineStrategyType deadlineStrategyType)
        {
            switch (deadlineStrategyType)
            {
                case DeadlineStrategyType.ByTime:
                    return new ByEndTimeStrategy();
                case DeadlineStrategyType.ByNumberOfParticipants:
                    return new ByNumberOfParticipantsStrategy();
                default:
                    throw new InvalidOperationException("Strategy not found");
            }
        }
    }
}
