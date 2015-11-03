namespace PhotoContest.App.Areas.Administration.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using PhotoContest.Models;
    using PhotoContest.Infrastructure.Mapping;
    using PhotoContest.Infrastructure.Models.ViewModels.Picture;

    public class ManageUserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [UIHint("TextFieldReadonly")]
        public string Email { get; set; }

        [UIHint("TextFieldReadonly")]
        public string UserName { get; set; }

        [UIHint("TextFieldReadonly")]
        public string FullName { get; set; }

        public string ProfileImageUrl { get; set; }

        [UIHint("KendoDateTime")]
        public DateTime RegisteredAt { get; set; }

        public IEnumerable<ManageContestViewModel> OrganizedContests { get; set; }

        public IEnumerable<PictureViewModel> Pictures { get; set; } 

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, ManageUserViewModel>()
                .ForMember(u => u.OrganizedContests, opt => opt.MapFrom(u => u.OrganizedContests))
                .ForMember(u => u.Pictures, opt => opt.MapFrom(u => u.Pictures))
                .ReverseMap();
        }
    }
}