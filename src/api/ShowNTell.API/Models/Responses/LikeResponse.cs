﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.Responses
{
    public class LikeResponse
    {
        public int ImagePostId { get; set; }
        public string UserEmail { get; set; }
        public DateTime DateCreated { get; set; }
    }
}