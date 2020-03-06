using System;
using System.Collections.Generic;
using System.Text;

namespace ShowNTell.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
