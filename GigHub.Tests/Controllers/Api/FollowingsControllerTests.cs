using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GigHub.Core;
using GigHub.Core.Repositories;
using GigHub.Controllers.Api;
using GigHub.Tests.Extensions;
using GigHub.Core.Dtos;
using FluentAssertions;
using System.Web.Http.Results;
using GigHub.Core.Models;

namespace GigHub.Tests.Controllers.Api
{
    /// <summary>
    /// Summary description for FollowingsControllerTests
    /// </summary>
    [TestClass]
    public class FollowingsControllerTests
    {
        private FollowingsController _controller;
        private Mock<IFollowingRepo> _mockRepo;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";

            var mockUoW = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IFollowingRepo>();
            mockUoW.SetupGet(r => r.Followings).Returns(_mockRepo.Object);

            _controller = new FollowingsController(mockUoW.Object);
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
        public void Follow_FollowingExists_ShouldReturnBadRequest()
        {
            // arrange
            var followDto = new FollowingDto { FolloweeId = "1"};
            _mockRepo.Setup(r => r.IsFollowing(_userId, "1")).Returns(true);

            // act
            var res = _controller.Follow(followDto);

            // assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public void Follow_FollowArtist_ShouldReturnOk()
        {
            var followDto = new FollowingDto { FolloweeId = "1"};
            _mockRepo.Setup(r => r.IsFollowing(_userId, "1")).Returns(false);

            var res = _controller.Follow(followDto);

            _mockRepo.Verify(r => r.Add(It.IsAny<Following>()));

            res.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void Unfollow_MissingFollowing_ShouldReturnNotFound()
        {
            Following following = null;

            _mockRepo.Setup(r => r.GetFollowing(_userId, "1")).Returns(following);

            var res = _controller.Unfollow(_userId);

            res.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Unfollow_ValidRequest_ShouldReturnOk()
        {
            var following = new Following { FolloweeId = _userId, FollowerId = "1"};

            _mockRepo.Setup(r => r.GetFollowing(_userId, "1")).Returns(following);

            var res = _controller.Unfollow("1");

            _mockRepo.Verify(r => r.Remove(It.IsAny<Following>()));

            res.Should().BeOfType(typeof(OkNegotiatedContentResult<string>));
        }
    }
}
