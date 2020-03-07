using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.Responses
{
    public class ImagePostResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ImageUri { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public UserResponse User { get; set; }
    }
}
