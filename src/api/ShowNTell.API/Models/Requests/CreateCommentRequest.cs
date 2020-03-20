using System.ComponentModel.DataAnnotations;

namespace ShowNTell.API.Models.Requests
{
    public class CreateCommentRequest
    {
        [Required]
        public string Content { get; set; }
    }
}