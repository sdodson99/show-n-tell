using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShowNTell.Domain.Models
{
    /// <summary>
    /// A model for a user following another user.
    /// </summary>
    public class Follow
    {
        public string UserEmail { get; set; }
        public string FollowerEmail { get; set; }

        public User User { get; set; }
        public User Follower { get; set; }
    }
}
