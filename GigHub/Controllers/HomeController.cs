using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GigHub.Persistence;
using GigHub.Core.ViewModels;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using GigHub.Persistence.Repositories;
using GigHub.Core;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public ActionResult Index(string query = null)
        {
            var upComingGigs = _unitOfWork.Gigs.GetUpcomingGigs();

            if (!string.IsNullOrWhiteSpace(query))
                upComingGigs = upComingGigs
                                    .Where(g => g.Artist.Name.Contains(query)
                                            || g.Genre.Name.Contains(query)
                                            || g.Venue.Contains(query));

            var userId = User.Identity.GetUserId();
            var attendances = _unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a=>a.GigId);

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = upComingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendances
            };

            return View("Gigs", viewModel);
        }

        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();
            var fs = _unitOfWork.Followings.GetFollowingsByUser(userId);

            return View("Artists", fs);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}