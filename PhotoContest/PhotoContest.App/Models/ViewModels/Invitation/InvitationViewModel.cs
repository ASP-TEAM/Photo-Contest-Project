namespace PhotoContest.App.Models.ViewModels.Invitation
{
    using System;

    using AutoMapper;

    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.App.Models.ViewModels.Contest;
    using PhotoContest.App.Models.ViewModels.User;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;

    public class InvitationViewModel : IMapFrom<Invitation>, IHaveCustomMappings
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
            configuration.CreateMap<Invitation, InvitationViewModel>()
                .ForMember(i => i.Inviter, opt => opt.MapFrom(i => i.Inviter))
                .ForMember(i => i.Invited, opt => opt.MapFrom(i => i.Invited))
                .ReverseMap();
        }
    }
}