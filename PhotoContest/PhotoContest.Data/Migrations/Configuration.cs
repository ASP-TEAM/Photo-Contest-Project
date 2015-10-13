namespace PhotoContest.Data.Migrations
{
    #region

    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

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
            var dlStrategy = new DeadlineStrategy { DeadlineStrategyType = DeadlineStrategyType.ByNumberOfParticipants };
            var rwStrategy = new RewardStrategy { RewardStrategyType = RewardStrategyType.SingleWinner };
            var vStrategy = new VotingStrategy { VotingStrategyType = VotingStrategyType.Closed };
            var plStrategy = new ParticipationStrategy { ParticipationStrategyType = ParticipationStrategyType.Closed };

            context.DeadlineStrategies.AddOrUpdate(dlStrategy);

            context.RewardStrategies.AddOrUpdate(rwStrategy);

            context.VotingStrategies.AddOrUpdate(vStrategy);

            context.ParticipationStrategies.AddOrUpdate(plStrategy);

            // Add User
            if (!context.Roles.Any(r => r.Name == "TestRole"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "TestRole" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "TestUser"))
            {
                var store = new UserStore<User>(context);
                var manager = new UserManager<User>(store);
                var user = new User() { UserName = "TestUser", RegisteredAt = DateTime.Now};

                manager.Create(user, "ChangeItAsap!");
                manager.AddToRole(user.Id, "TestRole");
            }
            context.SaveChanges();

            // Add Contest
            var testContest = new Contest
                              {
                                  Title = "TestContest",
                                  Description = "TestDescription",
                                  IsActive = true,
                                  StartDate = DateTime.Parse("12-12-12"),
                                  EndDate = DateTime.Parse("12-12-12"),
                                  RewardStrategyId = rwStrategy.Id,
                                  VotingStrategyId = vStrategy.Id,
                                  ParticipationStrategyId = plStrategy.Id,
                                  DeadlineStrategyId = dlStrategy.Id,
                                  OrganizatorId = context.Users.FirstOrDefault().Id
                              };
            context.Contests.AddOrUpdate(testContest);

            context.SaveChanges();
        }
    }
}