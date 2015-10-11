namespace PhotoContest.Data
{
    #region

    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using Microsoft.AspNet.Identity.EntityFramework;

    using PhotoContest.Data.Migrations;
    using PhotoContest.Models;

    #endregion

    public class PhotoContestDbContext : IdentityDbContext<User>
    {
        public PhotoContestDbContext()
            : base("PhotoContestDbContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotoContestDbContext, Configuration>());
        }

        public virtual IDbSet<RewardStrategy> RewardStrategies { get; set; }
        public virtual IDbSet<VotingStrategy> VotingStrategies { get; set; }
        public virtual IDbSet<ParticipationStrategy> ParticipationStrategies { get; set; }
        public virtual IDbSet<DeadlineStrategy> DeadlineStrategies { get; set; }

        public virtual IDbSet<Contest> Contests { get; set; }
        public virtual IDbSet<Picture> Pictures { get; set; }
        public virtual IDbSet<Vote> Votes { get; set; }
        public virtual IDbSet<ContestWinners> ContestWinners { get; set; }
        
        public static PhotoContestDbContext Create()
        {
            return new PhotoContestDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Contest>()
                .HasRequired(c => c.Organizator)
                .WithMany(u => u.OrganizedContests);

            modelBuilder.Entity<User>()
                .HasMany(u => u.InContests)
                .WithMany(c => c.Participants)
                .Map(
                    m =>
                    {
                        m.MapLeftKey("UserId");
                        m.MapRightKey("ContestId");
                        m.ToTable("UserInContests");
                    });

            
            base.OnModelCreating(modelBuilder);
        }
    }
}