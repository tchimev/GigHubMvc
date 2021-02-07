using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly ApplicationDbContext _db;

        public NotificationRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<UserNotification> GetUnreadUserNotifications(string userId)
        {
            return _db.UserNotifications.Where(n => n.UserId == userId && !n.IsRead).ToList();
        }

        public IEnumerable<Notification> GetUnreadNotificationsWithArtist(string userId)
        {
            return _db.UserNotifications
                                    .Where(n => n.UserId == userId && !n.IsRead)
                                    .Select(n => n.Notification)
                                    .Include(n => n.Gig.Artist)
                                    .ToList();
        }
    }
}