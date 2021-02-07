using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Core;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        /// <summary>
        /// Cancel a Gig
        /// </summary>
        /// <param name="id">Gig id</param>
        /// <returns>Ok if successful, or NotFound if Gig data is missing</returns>
        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _unitOfWork.Gigs.GetGigForUser(userId, id);

            if (gig == null || gig.IsCanceled)
                return NotFound();

            gig.Cancel();
            _unitOfWork.Complete();

            return Ok();
        }
    }
}
