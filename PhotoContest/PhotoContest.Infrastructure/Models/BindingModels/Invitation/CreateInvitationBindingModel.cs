namespace PhotoContest.Infrastructure.Models.BindingModels.Invitation
{
    using PhotoContest.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class CreateInvitationBindingModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public int ContestId { get; set; }

        [Required]
        public InvitationType Type { get; set; }
    }
}