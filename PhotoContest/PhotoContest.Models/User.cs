namespace PhotoContest.Models
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    #endregion

    public class User : IdentityUser
    {
        private ICollection<Picture> pictures;

        private ICollection<Vote> votes;

        private ICollection<Contest> inContests;

        private ICollection<Contest> organizedContests;

        private ICollection<Contest> committeeInContests;

        private ICollection<Invitation> pendingInvitations;

        private ICollection<Invitation> sendedInvitations;

        private ICollection<Reward> rewards; 

        public User()
        {
            this.organizedContests = new HashSet<Contest>();
            this.inContests = new HashSet<Contest>();
            this.committeeInContests = new HashSet<Contest>();

            this.votes = new HashSet<Vote>();

            this.pictures = new HashSet<Picture>();
            
            this.rewards = new HashSet<Reward>();

            this.sendedInvitations = new HashSet<Invitation>();
            this.pendingInvitations = new HashSet<Invitation>();
        }

        public string FullName { get; set; }

        public string ProfileImageUrl { get; set; }

        public DateTime RegisteredAt { get; set; }
        
        public virtual ICollection<Contest> OrganizedContests
        {
            get
            {
                return this.organizedContests;
            }
            set
            {
                this.organizedContests = value;
            }
        }

        public virtual ICollection<Contest> InContests
        {
            get
            {
                return this.inContests;
            }
            set
            {
                this.inContests = value;
            }
        }
        
        public virtual ICollection<Contest> CommitteeInContests
        {
            get
            {
                return this.committeeInContests;
            }
            set
            {
                this.committeeInContests = value;
            }
        }

        public virtual ICollection<Vote> Votes
        {
            get
            {
                return this.votes;
            }
            set
            {
                this.votes = value;
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

        public virtual ICollection<Invitation> SendedInvitations
        {
            get
            {
                return this.sendedInvitations;
            }
            set
            {
                this.sendedInvitations = value;
            }
        }

        public virtual ICollection<Invitation> PendingInvitations
        {
            get
            {
                return this.pendingInvitations;
            }
            set
            {
                this.pendingInvitations = value;
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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}