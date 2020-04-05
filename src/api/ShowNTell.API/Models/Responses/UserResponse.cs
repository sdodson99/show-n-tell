using System;

namespace ShowNTell.API.Models.Responses
{
    /// <summary>
    /// A model for a user response from the API.
    /// </summary>
    public class UserResponse
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateJoined { get; set; }
    }
}