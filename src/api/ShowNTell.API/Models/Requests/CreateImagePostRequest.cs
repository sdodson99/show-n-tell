﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShowNTell.API.Models.Requests
{
    public class CreateImagePostRequest
    {
        [Required]
        public IFormFile Image {get; set; }
        public string Description { get; set; }
    }
}
