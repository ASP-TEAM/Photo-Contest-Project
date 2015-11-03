namespace PhotoContest.Infrastructure.Interfaces
{
    using PhotoContest.Infrastructure.Models.ViewModels.Strategy.Deadline;
    using PhotoContest.Infrastructure.Models.ViewModels.Strategy.Reward;

    public interface IStrategyService
    {
        AbstractDeadlineStrategyViewModel GetDeadlineStrategyOptions(int id);

        AbstractRewardStrategyViewModel GetRewardStrategyOptions(int id);
    }
}