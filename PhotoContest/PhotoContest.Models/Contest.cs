namespace PhotoContest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PhotoContest.Models.Enums;

    public class Contest
    {
        private const ContestStatus DefaultContestState = ContestStatus.Active;

        private ICollection<User> participants;

        private ICollection<Picture> pictures;

        private ICollection<User> committee;

        private ICollection<User> invitedUsers;

        private ICollection<Reward> rewards;

        public Contest()
        {
            this.participants = new HashSet<User>();
            this.committee = new HashSet<User>();
            this.invitedUsers = new HashSet<User>();
            this.pictures = new HashSet<Picture>();
            this.rewards = new HashSet<Reward>();
            this.Status = DefaultContestState;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Description { get; set; }

        public string OrganizatorId { get; set; }

        public virtual User Organizator { get; set; }

        public ContestStatus Status { get; set; }

        public bool IsOpenForSubmissions { get; set; }
       
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int RewardStrategyId { get; set; }

        public virtual RewardStrategy RewardStrategy { get; set; }

        public int VotingStrategyId { get; set; }

        public virtual VotingStrategy VotingStrategy { get; set; }

        public int ParticipationStrategyId { get; set; }

        public virtual ParticipationStrategy ParticipationStrategy { get; set; }

        public int DeadlineStrategyId { get; set; }

        public virtual DeadlineStrategy DeadlineStrategy { get; set; }

        #region Nullables

        public int? ParticipantsLimit { get; set; }

        public int? TopNPlaces { get; set; }

        public DateTime? SubmissionDeadline { get; set; }

        #endregion

        public virtual ICollection<User> Participants
        {
            get
            {
                return this.participants;
            }
            set
            {
                this.participants = value;
            }
        }

        public virtual ICollection<User> Committee
        {
            get
            {
                return this.committee;
            }
            set
            {
                this.committee = value;
            }
        }

        public virtual ICollection<User> InvitedUsers
        {
            get
            {
                return this.invitedUsers;
            }
            set
            {
                this.invitedUsers = value;
            }
        }

        public virtual ICollection<Picture> Pictures
        {
            get
            {
                return this.pictures;
            }
            set
            {
                this.pictures = value;
            }
        }

        public virtual ICollection<Reward> Rewards
        {
            get
            {
                return this.rewards; 
            }
            set
            {
                this.rewards = value;
            }
        } 
    }
}