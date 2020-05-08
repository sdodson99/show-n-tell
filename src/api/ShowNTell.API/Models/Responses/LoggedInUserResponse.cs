using System;
using System.Collections.Generic;

namespace ShowNTell.API.Models.Responses
{
    public class LoggedInUserResponse
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateJoined { get; set; }

        public IEnumerable<FollowingResponse> Following { get; set; }
    }
}