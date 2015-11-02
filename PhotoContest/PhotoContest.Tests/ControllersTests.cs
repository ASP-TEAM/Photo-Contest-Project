using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhotoContest.Tests
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;

    using PhotoContest.App.Controllers;

    [TestClass]
    public class ControllersTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var request = new Mock<HttpRequestBase>();
            // Not working - IsAjaxRequest() is static extension method and cannot be mocked
            // request.Setup(x => x.IsAjaxRequest()).Returns(true /* or false */);
            // use this
            request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                       {"X-Requested-With", "XMLHttpRequest"}
                });

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var controller = new ContestsController();
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
        }
    }
}
