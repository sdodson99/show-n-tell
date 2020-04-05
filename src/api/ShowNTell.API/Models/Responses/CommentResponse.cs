using System;

namespace ShowNTell.API.Models.Responses
{
    /// <summary>
    /// A model for a comment response from the API.
    /// </summary>
    public class CommentResponse
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string Username{ get; set; }
        public int ImagePostId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}