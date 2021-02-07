using System.Collections.Generic;
using System.Linq;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;

namespace GigHub.Core.Repositories
{
    public interface IGigRepo
    {
        void AddGig(Gig gig);
        Gig GetGig(int gigId);
        Gig GetGigDetails(int gigId);
        Gig GetGigForUser(string userId, int gigId);
        Gig GetGigWithAttendances(int gigId);
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        IEnumerable<Gig> GetUserFutureAvailableGigs(string userId);
        IEnumerable<Gig> GetUpcomingGigs();
    }
}