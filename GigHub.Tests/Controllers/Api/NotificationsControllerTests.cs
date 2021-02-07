using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GigHub.Core.Repositories;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Tests.Extensions;
using GigHub.Core.Models;
using FluentAssertions;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.Api
{
    /// <summary>
    /// Summary description for NotificationsControllerTests
    /// </summary>
    [TestClass]
    public class NotificationsControllerTests
    {
        private Mock<INotificationRepo> _mockRepo;
        private NotificationsController _controller;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";

            var mockUoW = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<INotificationRepo>();
            mockUoW.SetupGet(r => r.Notifications).Returns(_mockRepo.Object);

            _controller = new NotificationsController(mockUoW.Object);
            _controller.MockCurrentUser(_userId, "ice@domain.com");
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void DismissNotifications_NoNotifications_ShouldReturnNotFound()
        {
            List<UserNotification> list = new List<UserNotification>();

            _mockRepo.Setup(r => r.GetUnreadUserNotifications(_userId)).Returns(list);

            var res = _controller.DismissNotifications();

            res.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DismissNotifications_MarkReadNotification_ShouldReturnOk()
        {
            List<UserNotification> list = new List<UserNotification>()
            {
                UserNotification.GetEmptyNotification(),
                UserNotification.GetEmptyNotification(),
                UserNotification.GetEmptyNotification()
            };

            _mockRepo.Setup(r => r.GetUnreadUserNotifications(_userId)).Returns(list);

            var res = _controller.DismissNotifications();

            list.Should().NotContain(x => x.IsRead == false);

            res.Should().BeOfType<OkResult>();
        }
    }
}