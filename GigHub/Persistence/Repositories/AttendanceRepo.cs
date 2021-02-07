using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Persistence;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
{
    public class AttendanceRepo : IAttendanceRepo
    {
        private readonly IApplicationDbContext _db;

        public AttendanceRepo(IApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(Attendance attend)
        {
            _db.Attendances.Add(attend);
        }

        public Attendance GetAttendance(string userId, int gigId)
        {
            return _db.Attendances.SingleOrDefault(a => a.AttendeeId == userId && a.GigId == gigId);
        }

        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return _db.Attendances.Where(a => a.AttendeeId == userId && a.Gig.Date > DateTime.Now).ToList();
        }

        public bool IsAttending(string userId, int gigId)
        {
            return _db.Attendances.Any(a => a.AttendeeId == userId && a.GigId == gigId);
        }

        public void Remove(Attendance attend)
        {
            _db.Attendances.Remove(attend);
        }
    }
}