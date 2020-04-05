using System.ComponentModel.DataAnnotations;

namespace ShowNTell.API.Models.Requests
{
    /// <summary>
    /// A model for creating a comment.
    /// </summary>
    public class CreateCommentRequest
    {
        [Required]
        public string Content { get; set; }
    }
}