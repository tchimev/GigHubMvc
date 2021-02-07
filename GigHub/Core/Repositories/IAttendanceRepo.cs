using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IAttendanceRepo
    {
        void Add(Attendance attend);
        void Remove(Attendance attend);
        Attendance GetAttendance(string userId, int gigId);
        bool IsAttending(string userId, int gigId);
        IEnumerable<Attendance> GetFutureAttendances(string userId);
    }
}