using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfig
{
    public class ApplicationUserConfig : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfig()
        {
            Property(a => a.Name).IsRequired().HasMaxLength(100);

            HasMany(u => u.Followers)
              .WithRequired(f => f.Followee)
              .WillCascadeOnDelete(false);

            HasMany(u => u.Followees)
              .WithRequired(f => f.Follower)
              .WillCascadeOnDelete(false);
        }
    }
}