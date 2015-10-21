namespace PhotoContest.App.Models.ViewModels.Contest
{
    using System.Collections.Generic;
    using AutoMapper;
    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.App.Models.ViewModels.Picture;
    using PhotoContest.Models;

    public class PreviewContestViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsOpenForSubmissions { get; set; }

        public IEnumerable<FullPictureViewModel> Pictures { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, PreviewContestViewModel>()
               .ForMember(c => c.Pictures, opt => opt.MapFrom(c => c.Pictures))
               .ReverseMap();
        }
    }
}