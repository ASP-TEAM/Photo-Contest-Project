namespace PhotoContest.App.Models.ViewModels.Contest
{
    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.Models;
    using AutoMapper;

    public class ContestWinnerViewModel : IMapFrom<ContestWinners>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ContestTitle { get; set; }

        public string ContestDescription { get; set; }

        public string Winner { get; set; }

        public int Place { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ContestWinners, ContestWinnerViewModel>()
               .ForMember(c => c.Id, opt => opt.MapFrom(c => c.ContestId))
               .ForMember(c => c.ContestTitle, opt => opt.MapFrom(c => c.Contest.Title))
               .ForMember(c => c.ContestDescription, opt => opt.MapFrom(c => c.Contest.Description))
               .ForMember(c => c.Place, opt => opt.MapFrom(c => c.Place))
               .ForMember(c => c.Winner, opt => opt.MapFrom(c => c.Winner.UserName))
               .ReverseMap();
        }
    }
}