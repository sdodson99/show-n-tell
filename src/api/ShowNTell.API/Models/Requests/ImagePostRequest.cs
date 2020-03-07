using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.Requests
{
    public class ImagePostRequest
    {
        public string ImageUri { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
