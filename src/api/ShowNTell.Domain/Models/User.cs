using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShowNTell.Domain.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
