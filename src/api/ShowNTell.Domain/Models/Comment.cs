using System;
using System.Collections.Generic;
using System.Text;

namespace ShowNTell.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ImagePostId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
