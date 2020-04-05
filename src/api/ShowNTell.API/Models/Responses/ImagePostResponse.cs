using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.Responses
{
    /// <summary>
    /// A model for an image post response from the API.
    /// </summary>
    public class ImagePostResponse
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string ImageUri { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public UserResponse User { get; set; }
        public ICollection<TagResponse> Tags { get; set; }
        public ICollection<LikeResponse> Likes { get; set; }
        public ICollection<CommentResponse> Comments { get; set; }
    }
}
