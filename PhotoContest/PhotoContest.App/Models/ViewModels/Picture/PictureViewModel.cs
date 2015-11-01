namespace PhotoContest.App.Models.ViewModels.Picture
{
    using AutoMapper;

    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.Models;

    public class PictureViewModel : IMapFrom<Picture>, IHaveCustomMappings
    {
        public string Url { get; set; }

        public string User { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Picture, PictureViewModel>()
                .ForMember(p => p.User, opt=>opt.MapFrom(p => p.User.UserName))
                .ReverseMap();
        }
    }
}