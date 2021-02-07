using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Core.Models;
using GigHub.Core.Dtos;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using GigHub.Core;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowingsController(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        /// <summary>
        /// Mark current user as follower to a FollowingDto.FolloweeId
        /// </summary>
        /// <param name="dto">FollowingDto</param>
        /// <returns>Ok if successful, or BadRequest if already exists</returns>
        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            if (_unitOfWork.Followings.IsFollowing(userId, dto.FolloweeId))
                return BadRequest("Following already exists!");

            var fol = new Following
            {
                FolloweeId = dto.FolloweeId,
                FollowerId = userId
            };

            _unitOfWork.Followings.Add(fol);
            _unitOfWork.Complete();

            return Ok();
        }

        /// <summary>
        /// Mark current user as NOT follower to a FollowingDto.FolloweeId
        /// </summary>
        /// <param name="id">followee id</param>
        /// <returns>Ok if successful, or NotFound if missing following data</returns>
        [HttpDelete]
        public IHttpActionResult Unfollow(string id)
        {
            var userId = User.Identity.GetUserId();
            var following = _unitOfWork.Followings.GetFollowing(userId, id);
            if (following == null)
                return NotFound();

            _unitOfWork.Followings.Remove(following);
            _unitOfWork.Complete();
            
            return Ok(id);
        }
    }
}
