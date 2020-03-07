using System;

namespace ShowNTell.API.Models.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }
    }
}