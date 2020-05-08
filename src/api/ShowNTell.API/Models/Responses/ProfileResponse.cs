using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.Responses
{
    /// <summary>
    /// A model for a profile response from the API.
    /// </summary>
    public class ProfileResponse
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateJoined { get; set; }

        public ICollection<ImagePostResponse> ImagePosts { get; set; }
        public ICollection<FollowerResponse> Followers { get; set; }
        public ICollection<FollowingResponse> Following { get; set; }
    }
}
