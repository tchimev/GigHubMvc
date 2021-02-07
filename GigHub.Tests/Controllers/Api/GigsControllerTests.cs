using Microsoft.VisualStudio.TestTools.UnitTesting;
using GigHub.Core;
using GigHub.Controllers.Api;
using Moq;
using GigHub.Tests.Extensions;
using GigHub.Core.Repositories;
using FluentAssertions;
using System.Web.Http.Results;
using GigHub.Core.Models;

namespace GigHub.Tests.Controllers.Api
{
    /// <summary>
    /// Summary description for GigsControllerTests
    /// </summary>
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController _controller;
        private Mock<IGigRepo> _mockRepository;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";

            _mockRepository = new Mock<IGigRepo>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Gigs).Returns(_mockRepository.Object);

            _controller = new GigsController(mockUoW.Object);
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
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            //act
            var result = _controller.Cancel(1);

            //assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_GigIsCanceled_ShouldReturnNotFound()
        {
            //arrange
            var gig = new Gig();
            gig.Cancel();

            //what I expect from act
            _mockRepository.Setup(r => r.GetGigForUser(_userId, 1)).Returns(gig);

            //act
            var res = _controller.Cancel(1);

            //assert
            res.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            //arrange
            var gig = new Gig() { ArtistId = _userId };
            //what I expect from act
            _mockRepository.Setup(r => r.GetGigForUser(_userId, 1)).Returns(gig);

            //act
            var res = _controller.Cancel(1);

            //assert
            res.Should().BeOfType<OkResult>();
        }
    }
}
