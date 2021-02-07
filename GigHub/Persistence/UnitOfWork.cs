using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Core;
using GigHub.Core.Repositories;
using GigHub.Persistence.Repositories;

namespace GigHub.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IGigRepo Gigs { get; private set; }
        public IAttendanceRepo Attendances { get; private set; }
        public IGenreRepo Genres { get; private set; }
        public IFollowingRepo Followings { get; private set; }
        public INotificationRepo Notifications { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Gigs = new GigRepo(_db);
            Attendances = new AttendanceRepo(_db);
            Genres = new GenreRepo(_db);
            Followings = new FollowingRepo(_db);
            Notifications = new NotificationRepo(_db);
        }

        public void Complete()
        {
            _db.SaveChanges();
        }
    }
}