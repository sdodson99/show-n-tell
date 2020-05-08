using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.Responses
{
    /// <summary>
    /// A model for a following response from the API.
    /// </summary>
    public class FollowingResponse
    {
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
