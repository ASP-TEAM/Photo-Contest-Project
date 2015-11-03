namespace PhotoContest.Infrastructure.Services
{
    using System;
    using PhotoContest.Infrastructure.Interfaces;
    using PhotoContest.Infrastructure.Models.ViewModels.Strategy.Deadline;

    public class StrategyService : BaseService, IStrategyService
    {
        public AbstractDeadlineStrategyViewModel GetDeadlineStrategyOptions(int id)
        {
            var strategy = this.Data.DeadlineStrategies.Find(id);

            var viewModel = (AbstractDeadlineStrategyViewModel)Activator.CreateInstance(null, "PhotoContest.Infrastructure.Models.ViewModels.Strategy.Deadline." + strategy.DeadlineStrategyType + "ViewModel").Unwrap();

            viewModel.Type = strategy.DeadlineStrategyType;

            return viewModel;
        }
    }
}