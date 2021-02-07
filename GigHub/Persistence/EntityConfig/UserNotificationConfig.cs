using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfig
{
    public class UserNotificationConfig : EntityTypeConfiguration<UserNotification>
    {
        public UserNotificationConfig()
        {
            HasKey(u => new { u.UserId, u.NotificationId });

            Property(u => u.UserId).HasColumnOrder(1);
            Property(u => u.NotificationId).HasColumnOrder(2);

            HasRequired(n => n.User)
                .WithMany(n => n.UserNotifications)
                .WillCascadeOnDelete(false);
        }
    }
}