﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using GigHub.Core.Models;

namespace GigHub.Core.ViewModels
{
    public class GigFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }

        public DateTime GetDateTime() { return DateTime.Parse(String.Format("{0} {1}", this.Date, this.Time)); }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }

        public string Heading { get; set; }

        public string Action
        {
            get
            {
                Expression<Func<GigHub.Controllers.GigsController, ActionResult>> update = (c => c.Update(this));
                Expression<Func<GigHub.Controllers.GigsController, ActionResult>> create = (c=>c.Create(this));

                var action = Id != 0 ? update : create;

                return (action.Body as MethodCallExpression).Method.Name;
            }
        }

        public IEnumerable<Genre> Genres { get; set; }
    }
}