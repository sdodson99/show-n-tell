using System;

namespace ShowNTell.API.Models.Responses
{
    public class UserResponse
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateJoined { get; set; }
    }
}