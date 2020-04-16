using System.ComponentModel.DataAnnotations;

namespace ShowNTell.API.Models.Requests
{
    public class UpdateCommentRequest
    {
        [Required]
        public string Content { get; set; }
    }
}