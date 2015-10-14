namespace PhotoContest.Models
{
    using System;
    using PhotoContest.Models.Enums;

    public class Invitation
    {
        public Invitation()
        {
            this.Status = InvitationStatus.Neutral;
        }

        public int Id { get; set; } 
        
        public int InviterId { get; set; }

        public virtual User Inviter { get; set; }

        public int InvitedId { get; set; }

        public virtual User Invited { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public DateTime DateOfInvitation { get; set; }

        public InvitationStatus Status { get; set; }

        public InvitationType Type { get; set; }
    }
}