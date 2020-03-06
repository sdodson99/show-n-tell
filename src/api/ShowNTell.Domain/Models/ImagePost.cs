using System;
using System.Collections.Generic;
using System.Text;

namespace ShowNTell.Domain.Models
{
    public class ImagePost
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ImageUri { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
