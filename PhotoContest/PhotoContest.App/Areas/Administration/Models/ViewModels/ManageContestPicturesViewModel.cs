namespace PhotoContest.App.Areas.Administration.Models.ViewModels
{
    using System.Web.Mvc;

    using AutoMapper;

    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.Models;

    public class ManageContestPicturesViewModel : IMapFrom<Picture>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public string Url { get; set; }

        public int VotesCount { get; set; }

        public string UserName { get; set; }

        public ManageContestViewModel Contest { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Picture, ManageContestPicturesViewModel>()
                .ForMember(p => p.VotesCount, opt=>opt.MapFrom(p => p.Votes.Count))
                .ForMember(p => p.UserName, opt=>opt.MapFrom(p=>p.User.UserName))
                .ForMember(p => p.Contest, opt=>opt.MapFrom(p => p.Contest))
                .ReverseMap();
        }
    }
}