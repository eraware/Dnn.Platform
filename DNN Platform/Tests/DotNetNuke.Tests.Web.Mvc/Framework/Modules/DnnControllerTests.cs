﻿using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Tests.Web.Mvc.Fakes;
using DotNetNuke.Web.Mvc.Framework.ActionResults;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using NUnit.Framework;

namespace DotNetNuke.Tests.Web.Mvc.Framework.Modules
{
    [TestFixture]
    public class DnnControllerTests
    {
        private const string TestViewName = "Foo";

        [Test]
        public void PortalSettings_Property_Is_Null_If_Not_Set_In_Context()
        {
            //Arrange
            HttpContextBase context = MockHelper.CreateMockHttpContext();

            //Act
            var controller = SetupController(context);

            //Assert
            Assert.IsNull(controller.PortalSettings);
        }

        [Test]
        public void ActivePage_Property_Is_Null_If_PortalSettings_Not_Set_In_Context()
        {
            //Arrange
            HttpContextBase httpContextBase = MockHelper.CreateMockHttpContext();

            //Act
            var controller = SetupController(httpContextBase);

            //Assert
            Assert.IsNull(controller.ActivePage);
        }

        [Test]
        public void User_Property_Is_Null_If_PortalSettings_Not_Set_In_Context()
        {
            //Arrange
            HttpContextBase httpContextBase = MockHelper.CreateMockHttpContext();

            //Act
            var controller = SetupController(httpContextBase);

            //Assert
            Assert.IsNull(controller.User);
        }

        [Test]
        public void View_Returns_DnnViewResult()
        {
            //Arrange
            HttpContextBase httpContextBase = MockHelper.CreateMockHttpContext();

            //Act
            var controller = SetupController(httpContextBase);
            var viewResult = controller.Action1();

            //Assert
            Assert.IsInstanceOf<DnnViewResult>(viewResult);
        }

        [Test]
        public void View_Returns_DnnViewResult_With_Correct_ViewName()
        {
            //Arrange
            HttpContextBase httpContextBase = MockHelper.CreateMockHttpContext();

            //Act
            var controller = SetupController(httpContextBase);
            var viewResult = controller.Action1();

            //Assert
            var dnnViewResult = viewResult as DnnViewResult;
            Assert.NotNull(dnnViewResult);
            Assert.AreEqual("Action1", dnnViewResult.ViewName);
        }

        [Test]
        public void View_Returns_DnnViewResult_With_Correct_MasterName()
        {
            //Arrange
            HttpContextBase httpContextBase = MockHelper.CreateMockHttpContext();

            //Act
            var controller = SetupController(httpContextBase);
            var viewResult = controller.Action2();

            //Assert
            var dnnViewResult = viewResult as DnnViewResult;
            Assert.NotNull(dnnViewResult);
            Assert.AreEqual("Action2", dnnViewResult.ViewName);
            Assert.AreEqual("Master2", dnnViewResult.MasterName);
        }

        [Test]
        public void View_Returns_DnnViewResult_With_Correct_ViewData()
        {
            //Arrange
            HttpContextBase httpContextBase = MockHelper.CreateMockHttpContext();

            //Act
            var controller = SetupController(httpContextBase);
            controller.ViewData.Add("key", "value");
            var viewResult = controller.Action2();

            //Assert
            var dnnViewResult = viewResult as DnnViewResult;
            Assert.NotNull(dnnViewResult);
            Assert.AreEqual("value", dnnViewResult.ViewData["key"]);
        }

        [Test]
        public void View_Returns_DnnViewResult_With_Correct_Model()
        {
            //Arrange
            var dog = new Dog() {Name = "Fluffy"};
            HttpContextBase httpContextBase = MockHelper.CreateMockHttpContext();

            //Act
            var controller = SetupController(httpContextBase);
            var viewResult = controller.Action3(dog);

            //Assert
            var dnnViewResult = viewResult as DnnViewResult;
            Assert.NotNull(dnnViewResult);
            Assert.AreEqual(dog, dnnViewResult.ViewData.Model);
        }

        private FakeDnnController SetupController(HttpContextBase context)
        {
            var controller = new FakeDnnController();
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller );
            return controller;
        }
    }
}
