namespace PhotoContest.Infrastructure.Models.ViewModels.Contest
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using PhotoContest.Infrastructure.Mapping;
    using PhotoContest.Models.Enums;

    public class ContestViewModel : BaseContestViewModel, IMapFrom<PhotoContest.Models.Contest>, IHaveCustomMappings
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

        public ParticipationStrategyType ParticipationStrategyType { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PhotoContest.Models.Contest, ContestViewModel>()
               .ForMember(c => c.ParticipationStrategyType, opt => opt.MapFrom(c => c.ParticipationStrategy.ParticipationStrategyType))
               .ReverseMap();
        }
    }
}