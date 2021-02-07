using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public ActionResult Details(int id)
        {
            // details of the Gig
            var gig = _unitOfWork.Gigs.GetGigDetails(id);

            if (gig == null)
                return HttpNotFound();

            var gigvm = new GigDetailViewModel
            {
                Gig = gig,
            };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                // get following/attending by userId
                gigvm.IsFollowing = gig.Artist.Followers.Any(f => f.FollowerId == userId);
                gigvm.IsAttending = gig.Attendances.Any(a => a.AttendeeId == userId);
            }

            return View(gigvm);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _unitOfWork.Gigs.GetUserFutureAvailableGigs(userId);

            return View(gigs);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel model)
        {
            return RedirectToAction("Index", "Home", new { query = model.SearchTerm });
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var vm = new GigsViewModel
            {
                UpcomingGigs = _unitOfWork.Gigs.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a=>a.GigId)
            };

            return View("Gigs", vm);
        }

        [Authorize]
        public ActionResult Create()
        {
            var vm = new GigFormViewModel();
            vm.Heading = "Create a Gig";
            vm.Genres = _unitOfWork.Genres.GetGenres();

            return View("GigForm", vm);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = _unitOfWork.Gigs.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            var userId = User.Identity.GetUserId();
            if (gig.ArtistId != userId)
                return new HttpUnauthorizedResult();

            var vm = new GigFormViewModel
            { 
                Id = gig.Id,
                Genres = _unitOfWork.Genres.GetGenres(),
                Date = gig.Date.ToString("dd MM yyyy"),
                Time = gig.Date.ToShortTimeString(),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Heading = "Edit a Gig"
            };

            return View("GigForm", vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm", model);
            }

            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                Date = model.GetDateTime(),
                GenreId = model.Genre,
                Venue = model.Venue
            };

            _unitOfWork.Gigs.AddGig(gig);
            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm", model);
            }

            var gig = _unitOfWork.Gigs.GetGigWithAttendances(model.Id);
            if (gig == null)
                return HttpNotFound();

            var userId = User.Identity.GetUserId();
            if (gig.ArtistId != userId)
                return new HttpUnauthorizedResult();

            gig.Modify(model.Venue, model.GetDateTime(), model.Genre);

            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }
    }
}