namespace PhotoContest.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using PhotoContest.App.Controllers;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Repositories;
    using PhotoContest.Infrastructure.Interfaces;
    using PhotoContest.Infrastructure.Mapping;
    using PhotoContest.Infrastructure.Models.BindingModels.Contest;
    using PhotoContest.Infrastructure.Models.ViewModels.Contest;
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
            this.mocksContainer = new MocksContainer();
            this.mocksContainer.SetupMocks();

            var requestMock = new Mock<HttpRequestBase>();
            requestMock.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                       {"X-Requested-With", "XMLHttpRequest"}
                });

            var responseMock = new Mock<HttpResponseBase>();

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(c => c.Request).Returns(requestMock.Object);
            contextMock.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(true);
            contextMock.SetupGet(c => c.User.Identity.Name).Returns("admin");
            contextMock.Setup(c => c.Response).Returns(responseMock.Object);

            var dataMock = new Mock<IPhotoContestData>();
            dataMock.Setup(d => d.Contests).Returns(this.mocksContainer.ContestsRepositoryMock.Object);

            var service = new ContestService(dataMock.Object);
            var controller = new ContestsController(dataMock.Object, service);

            controller.ControllerContext = new ControllerContext(contextMock.Object, new RouteData(), controller);

            this.fakeContestsController = controller;
        }

        [TestMethod]
        public void InactiveContestsShouldReturnInactiveContestsPartial()
        {
            var result = (PartialViewResult)this.fakeContestsController.InactiveContests();

            Assert.AreEqual("_InactiveContestsPartial", result.ViewName);
        }

        [TestMethod]
        public void InactiveContestsShouldReturnOneContestItem()
        {
            var result = (PartialViewResult)this.fakeContestsController.InactiveContests();

            var resultModelCount = result.Model as List<ContestViewModel>;

            Assert.AreEqual(1, resultModelCount.Count);
        }

        [TestMethod]
        public void ActiveContestsActionShouldReturnActiveContestsPartial()
        {
            var result = (PartialViewResult)this.fakeContestsController.ActiveContests();

            Assert.AreEqual("_ActiveContestsPartial", result.ViewName);
        }


        [TestMethod]
        public void ActiveContestsActionShouldReturnOneContestItem()
        {
            var result = (PartialViewResult)this.fakeContestsController.ActiveContests();

            var resultModelCount = result.Model as List<ContestViewModel>;

            Assert.AreEqual(1, resultModelCount.Count);
        }

        [TestMethod]
        public void PreviewActiveContestShouldReturnCorrectContestTitle()
        {
            var result = (ViewResult)this.fakeContestsController.PreviewContest(2);

            var contest = result.Model as PreviewContestViewModel;

            Assert.AreEqual("Test Title1 - Active", contest.Title);
        }

    }
}
