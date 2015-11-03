using System;
using AutoMapper;
using PhotoContest.Infrastructure.Mapping;
using PhotoContest.Infrastructure.Models.ViewModels.Contest;
using PhotoContest.Infrastructure.Models.ViewModels.User;
using PhotoContest.Models.Enums;

namespace PhotoContest.Infrastructure.Models.ViewModels.Invitation
{
    public class InvitationViewModel : IMapFrom<PhotoContest.Models.Invitation>, IHaveCustomMappings
    {
        public int Id { get; set; }
        
        public MinifiedUserViewModel Inviter { get; set; }

        public MinifiedUserViewModel Invited { get; set; }

        public int ContestId { get; set; }

        public ContestViewModel Contest { get; set; }

        public DateTime DateOfInvitation { get; set; }

        public InvitationStatus Status { get; set; }

        public InvitationType Type { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PhotoContest.Models.Invitation, InvitationViewModel>()
                .ForMember(i => i.Inviter, opt => opt.MapFrom(i => i.Inviter))
                .ForMember(i => i.Invited, opt => opt.MapFrom(i => i.Invited))
                .ReverseMap();
        }
    }
}