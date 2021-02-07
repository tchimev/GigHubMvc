using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfig
{
    public class NotificationConfig : EntityTypeConfiguration<Notification>
    {
        public NotificationConfig()
        {
            HasRequired(n => n.Gig);
        }
    }
}