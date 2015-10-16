namespace PhotoContest.App.Models.ViewModels.Contest
{
    using System;

    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.Models;

    public class ContestViewModel : IMapFrom<Contest>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int ParticipantsLimit { get; set; }

        public bool IsActive { get; set; }

        public bool IsOpenForSubmissions { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? SubmissionDate { get; set; }
    }
}