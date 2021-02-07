using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Core.Dtos;
using GigHub.Persistence;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using GigHub.Core;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        /// <summary>
        /// Get unread notifications for current user
        /// </summary>
        /// <returns>unread notifications</returns>
        [HttpGet]
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notis = _unitOfWork.Notifications.GetUnreadNotificationsWithArtist(userId);

            return notis.Select(AutoMapper.Mapper.Map<Notification, NotificationDto>);
        }

        /// <summary>
        /// Mark all notifications for current user as read
        /// </summary>
        /// <returns>Ok if successful, or NotFound if there are no unread notifications</returns>
        [HttpPut]
        public IHttpActionResult DismissNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notis = _unitOfWork.Notifications.GetUnreadUserNotifications(userId);
            if (notis.Count() == 0)
                return NotFound();

            foreach (var item in notis)
            {
                item.MarkRead();
            }
            _unitOfWork.Complete();

            return Ok();
        }
    }
}
