namespace PhotoContest.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using PhotoContest.Data.Repositories;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;

    public class MocksContainer
    {
        private static List<Contest> fakeContests = new List<Contest>
                                        {
                                            new Contest
                                                {
                                                    Id = 1,
                                                    Title = "Test Title1 - Inactive",
                                                    Description = "Test Descr",
                                                    OrganizatorId = "asdsada",
                                                    ParticipationStrategy =
                                                        new ParticipationStrategy()
                                                            {
                                                                Id = 1,
                                                                Name = "test strategy name",
                                                                Description = "test strategy",
                                                                ParticipationStrategyType = ParticipationStrategyType
                                                                    .Closed
                                                            },
                                                    Status = ContestStatus.Finalized,
                                                    EndDate = DateTime.Now,
                                                    IsOpenForSubmissions = false,
                                                    StartDate = DateTime.Now
                                                }, new Contest
                                                {
                                                    Id = 2,
                                                    Title = "Test Title1 - Active",
                                                    Description = "Test Descr 2",
                                                    OrganizatorId = "asdsada",
                                                    ParticipationStrategy =
                                                        new ParticipationStrategy()
                                                            {
                                                                Id = 1,
                                                                Name = "test strategy name",
                                                                Description = "test strategy",
                                                                ParticipationStrategyType = ParticipationStrategyType
                                                                    .Closed
                                                            },
                                                    
                                                    Status = ContestStatus.Active,
                                                    EndDate = DateTime.Now,
                                                    IsOpenForSubmissions = false,
                                                    StartDate = DateTime.Now
                                                }
                                        };

        public Mock<IRepository<Contest>> ContestsRepositoryMock;
        public Mock<IRepository<ContestWinners>> ContestsWinnersRepositoryMock;

        public void SetupMocks()
        {
            this.ContestsRepositoryMock = new Mock<IRepository<Contest>>();
            this.ContestsWinnersRepositoryMock = new Mock<IRepository<ContestWinners>>();

            this.ContestsRepositoryMock.Setup(r => r.All()).Returns(fakeContests.AsQueryable());
            this.ContestsRepositoryMock.Setup(r => r.Find(It.IsAny<int>())).Returns(
                (int id) =>
                    {
                        return fakeContests.FirstOrDefault(c => c.Id == id);
                    });
            this.ContestsWinnersRepositoryMock.Setup(cw => cw.All())
                .Returns(new List<ContestWinners>().AsQueryable());
        }
    }
}
