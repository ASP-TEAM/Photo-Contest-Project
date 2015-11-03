namespace PhotoContest.Infrastructure.Models.ViewModels.Picture
{
    using AutoMapper;
    using PhotoContest.Infrastructure.Mapping;

    public class PictureViewModel : IMapFrom<PhotoContest.Models.Picture>, IHaveCustomMappings
    {
        public string Url { get; set; }

        public string User { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PhotoContest.Models.Picture, PictureViewModel>()
                .ForMember(p => p.User, opt=>opt.MapFrom(p => p.User.UserName))
                .ReverseMap();
        }
    }
}