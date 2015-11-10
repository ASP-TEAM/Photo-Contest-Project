namespace PhotoContest.Infrastructure.Models.ViewModels.Picture
{
    using AutoMapper;
    using PhotoContest.Infrastructure.Mapping;
    using PhotoContest.Infrastructure.Models.ViewModels.Contest;

    public class FullPictureViewModel : IMapFrom<PhotoContest.Models.Picture>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int Votes { get; set; }

        public string UserName { get; set; }

        public ContestViewModel Contest { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PhotoContest.Models.Picture, FullPictureViewModel>()
                .ForMember(p => p.UserName, opt => opt.MapFrom(p => p.User.UserName))
                .ForMember(p => p.Contest, opt => opt.MapFrom(p => p.Contest))
                .ForMember(p => p.Votes, opt => opt.MapFrom(p => p.Votes.Count))
                .ReverseMap();
        }
    }
}