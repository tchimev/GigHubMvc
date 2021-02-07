using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfig
{
    public class GigConfig : EntityTypeConfiguration<Gig>
    {
        public GigConfig()
        {
            Property(g => g.ArtistId)
                .IsRequired();
            
            Property(g => g.GenreId)
                .IsRequired();

            Property(g => g.Venue)
                .IsRequired()
                .HasMaxLength(255);

            HasMany(a => a.Attendances)
                .WithRequired(g => g.Gig)
                .WillCascadeOnDelete(false);
        }
    }
}