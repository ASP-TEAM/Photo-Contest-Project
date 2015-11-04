namespace PhotoContest.Infrastructure.Models.ViewModels.Contest
{
    using PhotoContest.Models.Enums;
    using PhotoContest.Infrastructure.Mapping;
    using PhotoContest.Models;

    public class BaseContestViewModel : IMapFrom<Contest>
    {
        public ContestStatus Status { get; set; }
    }
}