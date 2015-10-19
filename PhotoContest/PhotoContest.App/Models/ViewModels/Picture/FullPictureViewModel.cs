namespace PhotoContest.App.Models.ViewModels.Picture
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.App.Models.ViewModels.Contest;
    using PhotoContest.App.Models.ViewModels.Vote;
    using PhotoContest.Models;

    public class FullPictureViewModel : IMapFrom<Picture>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int Votes { get; set; }

        public string UserName { get; set; }

        public ContestViewModel Contest { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Picture, FullPictureViewModel>()
                .ForMember(p => p.UserName, opt => opt.MapFrom(p => p.User.UserName))
                .ForMember(p => p.Contest, opt => opt.MapFrom(p => p.Contest))
                .ForMember(p => p.Votes, opt => opt.MapFrom(p => p.Votes.Count))
                .ReverseMap();
        }
    }
}