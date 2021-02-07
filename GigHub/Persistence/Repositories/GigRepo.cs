using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GigHub.Persistence;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
{
    public class GigRepo : IGigRepo
    {
        private readonly IApplicationDbContext _db;

        public GigRepo(IApplicationDbContext db)
        {
            _db = db;
        }

        public void AddGig(Gig gig)
        {
            _db.Gigs.Add(gig);
        }

        public Gig GetGig(int gigId)
        {
            return _db.Gigs.SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetUserFutureAvailableGigs(string userId)
        {
            return _db.Gigs
                            .Where(g => g.ArtistId == userId
                                        && g.Date > DateTime.Now
                                        && !g.IsCanceled
                                        )
                            .Include(g => g.Genre)
                            .ToList();
        }

        public Gig GetGigDetails(int gigId)
        {
            return _db.Gigs
                        .Where(g=>g.Id == gigId)
                        .Include(g => g.Artist.Followers)
                        .Include(g => g.Attendances)
                        .Include(g => g.Genre)
                        .SingleOrDefault();
        }

        public Gig GetGigWithAttendances(int gigId)
        {
            return _db.Gigs
                            .Include(g => g.Attendances.Select(a => a.Attendee))
                            .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return _db.Attendances
                            .Where(a => a.AttendeeId == userId)
                            .Select(a => a.Gig)
                            .Include(g => g.Artist)
                            .Include(g => g.Genre)
                            .ToList();
        }

        public IEnumerable<Gig> GetUpcomingGigs()
        {
            return _db.Gigs
                            .Where(g => g.Date > DateTime.Now && !g.IsCanceled)
                            .Include(g => g.Artist)
                            .Include(g => g.Genre)
                            .ToList();
        }

        public Gig GetGigForUser(string userId, int gigId)
        {
            return _db.Gigs
                        .Where(g => g.Id == gigId && g.ArtistId == userId)
                        .Include(g => g.Attendances.Select(a => a.Attendee))
                        .SingleOrDefault();
        }
    }
}