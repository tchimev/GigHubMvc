using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GigHub.Persistence.Repositories;
using Moq;
using GigHub.Persistence;
using GigHub.Core.Models;
using System.Data.Entity;
using System.Linq;
using GigHub.Tests.Extensions;
using FluentAssertions;

namespace GigHub.Tests.Persistance.Repositories
{

    /// <summary>
    /// Summary description for GigRepositoryTests
    /// </summary>
    [TestClass]
    public class GigRepositoryTests
    {
        private Mock<DbSet<Gig>> _mockGigs;
        private GigRepo _gigRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockGigs = new Mock<DbSet<Gig>>();
            var mockDb = new Mock<IApplicationDbContext>();
            mockDb.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);

            _gigRepo = new GigRepo(mockDb.Object);
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
        public void GetUserFutureAvailableGigs_GigIsNotInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig() { Date = DateTime.Now.AddDays(-1), ArtistId = "1" };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _gigRepo.GetUserFutureAvailableGigs("1");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUserAvailableGigs_GigIsCanceled_ShouldNotBeReturned()
        {
            var gig = new Gig() { Date = DateTime.Now.AddDays(1), ArtistId = "1" };
            gig.Cancel();

            _mockGigs.SetSource(new[] { gig });

            var gigs = _gigRepo.GetUserFutureAvailableGigs("1");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUserAvailableGigs_GigIsForOtherArtist_ShouldNotBeReturned()
        {
            var gig = new Gig() { Date = DateTime.Now.AddDays(1), ArtistId = "1" };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _gigRepo.GetUserFutureAvailableGigs(gig.ArtistId + "-");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUserAvailableGigs_GigIsCorrect_ShouldBeReturned()
        {
            var gig = new Gig() { Date = DateTime.Now.AddDays(1), ArtistId = "1" };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _gigRepo.GetUserFutureAvailableGigs(gig.ArtistId);

            gigs.Should().Contain(gig);
        }

        [TestMethod]
        public void GetUpcomingGigs_GigsInTheFuture_ShouldBeReturned()
        {
            var gig = new Gig() { Date = DateTime.Now.AddDays(3) };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _gigRepo.GetUpcomingGigs();

            gigs.Should().Contain(gig);
        }

        [TestMethod]
        public void GetUpcomingGigs_CanceledGigsInTheFuture_ShoudNotBeReturned()
        {
            var gig = new Gig() { Date = DateTime.Now.AddDays(4) };
            gig.Cancel();

            _mockGigs.SetSource(new[] { gig });

            var gigs = _gigRepo.GetUpcomingGigs();

            gigs.Should().BeEmpty();
        }
    }
}
