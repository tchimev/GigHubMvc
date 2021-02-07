using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Core.ViewModels
{
    public class GigDetailViewModel
    {
        public Gig Gig { get; set; }
        public bool IsAttending { get; set; }
        public bool IsFollowing { get; set; }
        public string BtnFollowText { get { return IsFollowing ? "Following" : "Follow"; } }
        public string AttendingText { get { return IsAttending ? "You are going to this event." : string.Empty; } }
        public string GigDate { get { return this.Gig.Date.ToString("dd MM"); } }
        public string GigTime { get { return this.Gig.Date.ToString("HH:mm"); } }
    }
}