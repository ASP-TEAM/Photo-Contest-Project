namespace PhotoContest.Infrastructure.Models.ViewModels.User
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using PhotoContest.Infrastructure.Mapping;
    using PhotoContest.Infrastructure.Models.ViewModels.Contest;
    using PhotoContest.Infrastructure.Models.ViewModels.Invitation;

    /// <summary>
    /// IMapFrom<> - maps all properties(automatically) from the class provided eg(User),
    ///  but the types and names must match
    /// IHaveCustomMappings - in case you need custom mappings (like the name says :D),
    /// you need to implemented it and write the logic in method CreateMappings(see the example below)
    /// </summary>
    public class FullUserViewModel : IMapFrom<PhotoContest.Models.User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string ProfileImageUrl { get; set; }

        public DateTime RegisteredAt { get; set; }

        public IEnumerable<ContestViewModel> OrganizedContests { get; set; }

        public IEnumerable<InvitationViewModel> PendingInvitations { get; set; }

        public IEnumerable<InvitationViewModel> SendedInvitations { get; set; }
        
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PhotoContest.Models.User, FullUserViewModel>()
                .ForMember(u => u.OrganizedContests, opt => opt.MapFrom(u => u.OrganizedContests))
                .ForMember(u => u.PendingInvitations, opt => opt.MapFrom(u => u.PendingInvitations))
                .ForMember(u => u.SendedInvitations, opt => opt.MapFrom(u => u.SendedInvitations))
                .ReverseMap();
        }
    }
}