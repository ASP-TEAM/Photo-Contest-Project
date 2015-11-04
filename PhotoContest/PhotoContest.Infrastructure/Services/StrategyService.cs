namespace PhotoContest.Infrastructure.Services
{
    using System;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Infrastructure.Interfaces;
    using PhotoContest.Infrastructure.Models.ViewModels.Strategy.Deadline;
    using PhotoContest.Infrastructure.Models.ViewModels.Strategy.Reward;

    public class StrategyService : BaseService, IStrategyService
    {
        public StrategyService(IPhotoContestData data)
            : base(data)
        {
        }

        public AbstractDeadlineStrategyViewModel GetDeadlineStrategyOptions(int id)
        {
            var strategy = this.Data.DeadlineStrategies.Find(id);

            var viewModel = (AbstractDeadlineStrategyViewModel)Activator.CreateInstance(null, "PhotoContest.Infrastructure.Models.ViewModels.Strategy.Deadline." + strategy.DeadlineStrategyType + "ViewModel").Unwrap();

            viewModel.Type = strategy.DeadlineStrategyType;

            return viewModel;
        }

        public AbstractRewardStrategyViewModel GetRewardStrategyOptions(int id)
        {
            var strategy = this.Data.RewardStrategies.Find(id);

            var viewModel = (AbstractRewardStrategyViewModel)Activator.CreateInstance(null, "PhotoContest.Infrastructure.Models.ViewModels.Strategy.Reward." + strategy.RewardStrategyType + "ViewModel").Unwrap();

            viewModel.Type = strategy.RewardStrategyType;

            return viewModel;
        }
    }
}