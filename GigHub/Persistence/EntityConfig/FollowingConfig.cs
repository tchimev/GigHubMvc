using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfig
{
    public class FollowingConfig : EntityTypeConfiguration<Following>
    {
        public FollowingConfig()
        {
            HasKey(f => new { f.FolloweeId, f.FollowerId });

            Property(f => f.FollowerId).HasColumnOrder(1);
            Property(f => f.FolloweeId).HasColumnOrder(2);
        }
    }
}