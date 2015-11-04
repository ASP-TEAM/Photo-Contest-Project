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

        private MocksContainer mocksContainer;

        [TestInitialize]
        public void TestInitialize()
        {
            AutoMapperConfig.Execute();
            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                       {"X-Requested-With", "XMLHttpRequest"}
                });

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            this.mocksContainer = new MocksContainer();
            this.mocksContainer.SetupMocks();

            var dataMock = new Mock<IPhotoContestData>();
            dataMock.Setup(d => d.Contests).Returns(this.mocksContainer.ContestsRepositoryMock.Object);

            var service = new ContestService(dataMock.Object);
            var controller = new ContestsController(dataMock.Object, service);

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
