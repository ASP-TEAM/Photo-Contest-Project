namespace PhotoContest.Data.Migrations
{
    #region

    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhotoContest.Common;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;

    #endregion

    public sealed class Configuration : DbMigrationsConfiguration<PhotoContestDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PhotoContestDbContext context)
        {
            // Add strategies
            this.SeedStrategies(context);

            // Add roles
            this.SeedDefaultApplicationRoles(context);

            // Add User
            this.SeedAdminUser(context);
            
            // Add Contest
            this.SeedContest(context);
        }

        private void SeedContest(PhotoContestDbContext context)
        {
            var testContest = new Contest
            {
                Title = "TestContest",
                Description = "TestDescription",
                IsActive = true,
                StartDate = new DateTime(2015, 10, 10),
                SubmissionDate = DateTime.Now,
                EndDate = new DateTime(2015, 10, 15),
                RewardStrategyId = context.RewardStrategies.FirstOrDefault().Id,
                VotingStrategyId = context.VotingStrategies.FirstOrDefault().Id,
                ParticipationStrategyId = context.ParticipationStrategies.FirstOrDefault().Id,
                DeadlineStrategyId = context.DeadlineStrategies.FirstOrDefault().Id,
                OrganizatorId = context.Users.FirstOrDefault().Id
            };
            context.Contests.AddOrUpdate(testContest);
            context.SaveChanges();
        }

        private void SeedAdminUser(PhotoContestDbContext context)
        {
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<User>(context);
                var manager = new UserManager<User>(store);
                var user = new User() { UserName = "admin", RegisteredAt = DateTime.Now };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, GlobalConstants.AdminRole);
            }
            context.SaveChanges();
        }

        private void SeedStrategies(PhotoContestDbContext context)
        {
            var dlStrategy = new DeadlineStrategy { DeadlineStrategyType = DeadlineStrategyType.ByNumberOfParticipants };
            var rwStrategy = new RewardStrategy { RewardStrategyType = RewardStrategyType.SingleWinner };
            var vStrategy = new VotingStrategy { VotingStrategyType = VotingStrategyType.Closed };
            var plStrategy = new ParticipationStrategy { ParticipationStrategyType = ParticipationStrategyType.Closed };

            context.DeadlineStrategies.AddOrUpdate(dlStrategy);
            context.RewardStrategies.AddOrUpdate(rwStrategy);
            context.VotingStrategies.AddOrUpdate(vStrategy);
            context.ParticipationStrategies.AddOrUpdate(plStrategy);
            context.SaveChanges();
        }

        private void SeedDefaultApplicationRoles(PhotoContestDbContext context)
        {
            if (!context.Roles.Any())
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);

                var adminRole = new IdentityRole { Name = GlobalConstants.AdminRole };
                var userRole = new IdentityRole { Name = GlobalConstants.UserRole };

                manager.Create(adminRole);
                manager.Create(userRole);
            }
        }
    }
}