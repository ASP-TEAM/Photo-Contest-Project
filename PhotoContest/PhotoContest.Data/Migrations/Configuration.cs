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
            if (!context.Contests.Any())
            {
                var testContest = new Contest
                {
                    Title = "TestContest",
                    Description = "TestDescription",
                    IsOpenForSubmissions = true,
                    StartDate = new DateTime(2015, 10, 10),
                    EndDate = new DateTime(2015, 10, 15),
                    RewardStrategyId = context.RewardStrategies.FirstOrDefault().Id,
                    VotingStrategyId = context.VotingStrategies.FirstOrDefault().Id,
                    ParticipationStrategyId = context.ParticipationStrategies.FirstOrDefault().Id,
                    DeadlineStrategyId = context.DeadlineStrategies.FirstOrDefault().Id,
                    OrganizatorId = context.Users.FirstOrDefault().Id
                };

                context.Contests.AddOrUpdate(c => c.Title, testContest);
                context.SaveChanges();
            }
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
            var dlStrategyByNumber = new DeadlineStrategy { DeadlineStrategyType = DeadlineStrategyType.ByNumberOfParticipants, Name = "By Number Of Participants" };
            var dlStrategyByTime = new DeadlineStrategy { DeadlineStrategyType = DeadlineStrategyType.ByTime, Name = "By Time" };

            var rwStrategySingle = new RewardStrategy { RewardStrategyType = RewardStrategyType.SingleWinner, Name = "Single Winner" };
            var rwStrategyTopN = new RewardStrategy { RewardStrategyType = RewardStrategyType.TopNPrizes, Name = "Top N Prizes" };

            var vStrategyOpen = new VotingStrategy { VotingStrategyType = VotingStrategyType.Open, Name  = "Open" };
            var vStrategyClosed = new VotingStrategy { VotingStrategyType = VotingStrategyType.Closed, Name = "Closed" };

            var plStrategyOpen = new ParticipationStrategy { ParticipationStrategyType = ParticipationStrategyType.Open, Name = "Open" };
            var plStrategyClosed = new ParticipationStrategy { ParticipationStrategyType = ParticipationStrategyType.Closed, Name = "Closed" };

            context.DeadlineStrategies.AddOrUpdate(dl => dl.DeadlineStrategyType, dlStrategyByNumber);
            context.DeadlineStrategies.AddOrUpdate(dl => dl.DeadlineStrategyType, dlStrategyByTime);

            context.RewardStrategies.AddOrUpdate(rw => rw.RewardStrategyType, rwStrategySingle);
            context.RewardStrategies.AddOrUpdate(rw => rw.RewardStrategyType, rwStrategyTopN);

            context.VotingStrategies.AddOrUpdate(v => v.VotingStrategyType, vStrategyOpen);

            context.VotingStrategies.AddOrUpdate(v => v.VotingStrategyType, vStrategyClosed);
            context.VotingStrategies.AddOrUpdate(v => v.VotingStrategyType, vStrategyClosed);

            context.ParticipationStrategies.AddOrUpdate(pl => pl.ParticipationStrategyType, plStrategyOpen);
            context.ParticipationStrategies.AddOrUpdate(pl => pl.ParticipationStrategyType, plStrategyClosed);

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