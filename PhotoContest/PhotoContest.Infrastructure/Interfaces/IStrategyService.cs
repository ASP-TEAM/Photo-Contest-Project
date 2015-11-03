using PhotoContest.Infrastructure.Models.ViewModels.Strategy.Deadline;

namespace PhotoContest.Infrastructure.Interfaces
{
    public interface IStrategyService
    {
        AbstractDeadlineStrategyViewModel GetDeadlineStrategyOptions(int id);
    }
}