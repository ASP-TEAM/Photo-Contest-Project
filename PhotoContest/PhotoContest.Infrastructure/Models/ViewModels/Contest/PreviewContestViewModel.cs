namespace PhotoContest.Infrastructure.Models.ViewModels.Contest
{
    using System.Collections.Generic;
    using PhotoContest.Infrastructure.Models.ViewModels.Picture;
    using PhotoContest.Infrastructure.Models.ViewModels.User;
    using AutoMapper;
    using PhotoContest.Infrastructure.Mapping;

    public class PreviewContestViewModel : BaseContestViewModel, IMapFrom<PhotoContest.Models.Contest>, IHaveCustomMappings
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
            configuration.CreateMap<PhotoContest.Models.Contest, PreviewContestViewModel>()
               .ForMember(c => c.Pictures, opt => opt.MapFrom(c => c.Pictures))
               .ForMember(c => c.Participants, opt => opt.MapFrom(c => c.Participants))
               .ReverseMap();
        }
    }
}