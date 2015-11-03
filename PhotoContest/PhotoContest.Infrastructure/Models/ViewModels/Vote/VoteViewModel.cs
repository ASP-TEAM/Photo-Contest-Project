using AutoMapper;
using PhotoContest.Infrastructure.Mapping;
using PhotoContest.Infrastructure.Models.ViewModels.Picture;

namespace PhotoContest.Infrastructure.Models.ViewModels.Vote
{
    public class VoteViewModel : IMapFrom<PhotoContest.Models.Vote>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public PictureViewModel Picture { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PhotoContest.Models.Vote, VoteViewModel>()
                .ForMember(v => v.UserName, opt => opt.MapFrom(v => v.User.UserName))
                .ForMember(v => v.Picture, opt => opt.MapFrom(v => v.Picture))
                .ReverseMap();
        }
    }
}