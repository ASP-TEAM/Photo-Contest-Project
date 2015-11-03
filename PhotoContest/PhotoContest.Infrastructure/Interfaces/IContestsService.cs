namespace PhotoContest.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using PhotoContest.Infrastructure.Models.ViewModels.Contest;

    public interface IContestsService
    {
        IEnumerable<ContestViewModel> GetActiveContests(string userId);

        IEnumerable<ContestViewModel> GetInactiveContests();

        IEnumerable<ContestViewModel> GetMyContests(string userId);

        IEnumerable<ContestViewModel> GetAllContests(string userId);
    }
}