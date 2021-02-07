using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GigHub.Core.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginDateTime { get; private set; }
        public string OriginVenue { get; private set; }

        public Gig Gig { get; private set; }

        protected Notification()
        {

        }

        private Notification(Gig gig, NotificationType type)
        {
            if (gig == null)
                throw new ArgumentNullException("gig");

            Gig = gig;
            Date = DateTime.Now;
            Type = type;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(gig, NotificationType.GigCreated);
        }

        public static Notification GigUpdated(Gig newGig, DateTime originDate, string originVenue)
        {
            var noti = new Notification(newGig, NotificationType.GigUpdated);
            noti.OriginDateTime = originDate;
            noti.OriginVenue = originVenue;

            return noti;
        }

        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(gig, NotificationType.GigCanceled);
        }
    }
}