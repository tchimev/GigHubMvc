﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GigHub.Core.Models
{
    public class Genre
    {
        public Byte Id { get; set; }

        public string Name { get; set; }
    }
}