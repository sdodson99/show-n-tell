using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShowNTell.Domain.Models
{
    /// <summary>
    /// A model for a user of the application.
    /// </summary>
    public class User
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateJoined { get; set; }

        public ICollection<ImagePost> ImagePosts { get; set; }
        public ICollection<Follow> Followers { get; set; }
        public ICollection<Follow> Following { get; set; }
    }
}
