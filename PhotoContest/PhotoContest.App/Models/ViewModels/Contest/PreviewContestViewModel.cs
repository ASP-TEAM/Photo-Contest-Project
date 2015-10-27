namespace PhotoContest.App.Models.ViewModels.Contest
{
    using System.Collections.Generic;
    using AutoMapper;
    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.App.Models.ViewModels.Picture;
    using PhotoContest.App.Models.ViewModels.User;
    using PhotoContest.Models;

    public class PreviewContestViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public PreviewContestViewModel()
        {
            this.CanParticipate = false;
            this.CanManage = false;
            this.CanUpload = false;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsOpenForSubmissions { get; set; }

        public IEnumerable<FullPictureViewModel> Pictures { get; set; }

        public IEnumerable<MinifiedUserViewModel> Participants { get; set; }

        public bool CanParticipate { get; set; }

        public bool CanManage { get; set; }

        public bool CanUpload { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, PreviewContestViewModel>()
               .ForMember(c => c.Pictures, opt => opt.MapFrom(c => c.Pictures))
               .ForMember(c => c.Participants, opt => opt.MapFrom(c => c.Participants))
               .ReverseMap();
        }
    }
}