using GigHub.Core.Repositories;

namespace GigHub.Core
{
    public interface IUnitOfWork
    {
        IAttendanceRepo Attendances { get; }
        IGenreRepo Genres { get; }
        IGigRepo Gigs { get; }
        IFollowingRepo Followings { get; }
        INotificationRepo Notifications { get; }

        void Complete();
    }
}