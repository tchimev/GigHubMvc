using System;
using GigHub.Core.Models;
namespace GigHub.Core.Dtos
{
    public class NotificationDto
    {
        public DateTime Date { get; set; }
        public NotificationType Type { get; set; }
        public bool IsCancel { get { return this.Type == NotificationType.GigCanceled; } }
        public bool IsUpdate { get { return this.Type == NotificationType.GigUpdated; } }
        public bool IsCreate { get { return this.Type == NotificationType.GigCreated; } }
        public DateTime? OriginDateTime { get; set; }
        public string OriginVenue { get; set; }
        public GigDto Gig { get; set; }
    }
}