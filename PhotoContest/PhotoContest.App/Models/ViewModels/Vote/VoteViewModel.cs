namespace PhotoContest.App.Models.ViewModels.Vote
{
    using AutoMapper;

    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.Models;

    public class VoteViewModel : IMapFrom<Vote>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public PictureViewModel Picture { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Vote, VoteViewModel>()
                .ForMember(v => v.UserName, opt => opt.MapFrom(v => v.User.UserName))
                .ForMember(v => v.Picture, opt => opt.MapFrom(v => v.Picture))
                .ReverseMap();
        }
    }
}