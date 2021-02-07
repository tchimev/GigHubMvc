using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GigHub.Persistence.Repositories;
using System.Data.Entity;
using Moq;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Tests.Extensions;
using FluentAssertions;

namespace GigHub.Tests.Persistance.Repositories
{
    /// <summary>
    /// Summary description for NotificationRepositoryTests
    /// </summary>
    [TestClass]
    public class AttendanceRepositoryTests
    {
        private Mock<DbSet<Attendance>> _mockAttends;
        private AttendanceRepo _attendRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockAttends = new Mock<DbSet<Attendance>>();
            var mockDb = new Mock<IApplicationDbContext>();
            mockDb.SetupGet(n => n.Attendances).Returns(_mockAttends.Object);

            _attendRepo = new AttendanceRepo(mockDb.Object);
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
        public void GetAttendance_GetAttendanceByUser_ShouldContainAttendance()
        {
            var att = new Attendance() { AttendeeId = "1", GigId = 1};

            _mockAttends.SetSource(new[] { att });

            var res = _attendRepo.GetAttendance("1", 1);

            res.Should().BeSameAs(att);
        }

        [TestMethod]
        public void GetFutureAttendance_AttendancesInTheFuture_ShouldBeReturned()
        {
            var att = new Attendance { AttendeeId = "1", Gig = new Gig { Id = 1, Date = DateTime.Now.AddDays(5) } };

            _mockAttends.SetSource(new[] { att });

            var res = _attendRepo.GetFutureAttendances("1");

            res.Should().Contain(att);
        }

        [TestMethod]
        public void IsAttending_UserAttendsGig_ShouldReturnTrue()
        {
            var att = new Attendance { AttendeeId = "1", GigId = 1 };

            _mockAttends.SetSource(new[] { att});

            var res = _attendRepo.IsAttending("1", 1);

            res.Should().BeTrue();
        }
    }
}
