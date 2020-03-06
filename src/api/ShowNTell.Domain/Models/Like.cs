using System;
using System.Collections.Generic;
using System.Text;

namespace ShowNTell.Domain.Models
{
    public class Like
    {
        public int UserId { get; set; }
        public int ImagePostId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
