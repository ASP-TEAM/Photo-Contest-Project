namespace PhotoContest.App.Models.ViewModels.Contest
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.Models;
    using AutoMapper;
    using PhotoContest.Models.Enums;

    public class ContestViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public ContestViewModel()
        {
            this.CanParticipate = false;
            this.CanManage = false;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string OrganizatorId { get; set; }

        public bool IsActive { get; set; }

        public bool IsOpenForSubmissions { get; set; }

        public bool CanParticipate { get; set; }

        public bool CanManage { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime StartDate { get; set; }

<<<<<<< HEAD
=======
        [UIHint("DateTimeNullable")]
        public DateTime EndDate { get; set; }

        [UIHint("DateTimeNullable")]
>>>>>>> 503d9b8bd7cd32ea446dc9db63b29af3101c3934
        public DateTime? SubmissionDate { get; set; }
        
        public ParticipationStrategyType ParticipationStrategyType { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestViewModel>()
               .ForMember(c => c.ParticipationStrategyType, opt => opt.MapFrom(c => c.ParticipationStrategy.ParticipationStrategyType))
               .ReverseMap();
        }
    }
}