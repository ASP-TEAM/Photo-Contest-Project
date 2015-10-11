﻿namespace PhotoContest.Models
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

        public User()
        {
            this.organizedContests = new HashSet<Contest>();
            this.inContests = new HashSet<Contest>();
            this.votes = new HashSet<Vote>();
            this.pictures = new HashSet<Picture>();
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