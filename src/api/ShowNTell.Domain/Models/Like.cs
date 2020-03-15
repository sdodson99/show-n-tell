using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShowNTell.Domain.Models
{
    public class Like
    {
        public string UserEmail { get; set; }
        public int ImagePostId { get; set; }
        public DateTime DateCreated { get; set; }

        public User User { get; set; }
        public ImagePost ImagePost { get; set; }
    }
}
