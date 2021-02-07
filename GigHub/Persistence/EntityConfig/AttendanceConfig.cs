using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfig
{
    public class AttendanceConfig : EntityTypeConfiguration<Attendance>
    {
        public AttendanceConfig()
        {
            HasKey(a => new { a.GigId, a.AttendeeId });

            Property(a => a.GigId)
                .HasColumnOrder(1);
            Property(a => a.AttendeeId)
                .HasColumnOrder(2);
        }
    }
}