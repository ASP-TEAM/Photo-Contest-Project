using AutoMapper;
using PhotoContest.Infrastructure.Mapping;

namespace PhotoContest.Infrastructure.Models.ViewModels.Invitation
{
    public class NotificationViewModel : IMapFrom<PhotoContest.Models.Invitation>, IHaveCustomMappings
    {
        public int InvitationId { get; set; }

        public string Sender { get; set; }

        public string Type { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PhotoContest.Models.Invitation, NotificationViewModel>()
               .ForMember(i => i.InvitationId, opt => opt.MapFrom(i => i.Id))
               .ForMember(i => i.Sender, opt => opt.MapFrom(i => i.Inviter.UserName))
               .ForMember(i => i.Type, opt => opt.MapFrom(i => i.Type.ToString()))
               .ReverseMap();
        }
    }
}