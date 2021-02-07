using Microsoft.VisualStudio.TestTools.UnitTesting;
using GigHub.Controllers.Api;
using Moq;
using GigHub.Core.Repositories;
using GigHub.Core;
using GigHub.Tests.Extensions;
using FluentAssertions;
using System.Web.Http.Results;
using GigHub.Core.Models;

namespace GigHub.Tests.Controllers.Api
{
    /// <summary>
    /// Summary description for AttendancesControllerTests
    /// </summary>
    [TestClass]
    public class AttendancesControllerTests
    {
        private AttendancesController _controller;
        private Mock<IAttendanceRepo> _mockRepo;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";

            _mockRepo = new Mock<IAttendanceRepo>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Attendances).Returns(_mockRepo.Object);

            _controller = new AttendancesController(mockUoW.Object);
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
        public void Attend_GigAlreadyAttended_ShouldReturnBadRequest()
        {
            //arrange
            var attendDto = new Core.Dtos.AttendanceDto { GigId = 1 };

            // what I expect from act
            _mockRepo.Setup(r => r.IsAttending(_userId, 1)).Returns(true);

            //act
            var res = _controller.Attend(attendDto);

            //assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();          
        }

        [TestMethod]
        public void Attend_AttendanceAddToList_ShouldReturnOK()
        {
            var attendDto = new Core.Dtos.AttendanceDto { GigId = 1 };

            var res = _controller.Attend(attendDto);

            _mockRepo.Verify(r => r.Add(It.IsAny<Attendance>()));

            res.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void Delete_AttendanceRemoved_ShouldReturnNotFound()
        {
            Attendance attendance = null;

            _mockRepo.Setup(r => r.GetAttendance(_userId, 1)).Returns(attendance);

            var res = _controller.Delete(1);

            res.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Delete_ValidRequest_ShouldReturnOk()
        {
            var attendance = new Attendance { AttendeeId = _userId, GigId = 1 };

            _mockRepo.Setup(r => r.GetAttendance(_userId, 1)).Returns(attendance);

            var res = _controller.Delete(1);

            _mockRepo.Verify(r => r.Remove(It.IsAny<Attendance>()));

            res.Should().BeOfType(typeof(OkNegotiatedContentResult<int>));
        }
    }
}
