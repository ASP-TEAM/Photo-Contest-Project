using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhotoContest.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;

    using PhotoContest.App.Controllers;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Repositories;
    using PhotoContest.Infrastructure.Interfaces;
    using PhotoContest.Infrastructure.Mapping;
    using PhotoContest.Infrastructure.Services;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;

    [TestClass]
    public class ControllersTests
    {
        private ContestsController fakeContestsController;
        private ContestService fakeContestsService;

        [TestInitialize]
        public void TestInitialize()
        {
            // TODO Extract this in Mock Container

            AutoMapperConfig.Execute();
            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                       {"X-Requested-With", "XMLHttpRequest"}
                });

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            var contestsRepoMock = new Mock<IRepository<Contest>>();

            contestsRepoMock.Setup(r => r.All())
                .Returns(new List<Contest>
                             {
                                 new Contest {
                                     Id = 1,
                                     Title = "Test Title1",
                                     Description = "Test Descr",
                                     OrganizatorId = "asdsada",
                                     ParticipationStrategy = new ParticipationStrategy()
                                                                 {
                                                                     Id = 1,
                                                                     Name = "test strategy name",
                                                                     Description = "test strategy",
                                                                     ParticipationStrategyType = ParticipationStrategyType.Closed
                                                                 },
                                     Status = ContestStatus.Finalized,
                                     EndDate = DateTime.Now,
                                     IsOpenForSubmissions = false,
                                     StartDate = DateTime.Now
                                 }
                             }.AsQueryable());

            var dataMock = new Mock<IPhotoContestData>();
            dataMock.Setup(d => d.Contests).Returns(contestsRepoMock.Object);

            var strategyServiceMock = new Mock<IStrategyService>();

            var service = new ContestService(dataMock.Object);
            var controller = new ContestsController(dataMock.Object, service, strategyServiceMock.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            this.fakeContestsController = controller;
        }
        [TestMethod]
        public void InactiveContestsShouldReturn_InactiveContestsPartial()
        {
            var result = (PartialViewResult)this.fakeContestsController.InactiveContests();

            Assert.AreEqual("_InactiveContestsPartial", result.ViewName);
        }
    }
}
