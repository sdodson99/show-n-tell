using System;
using System.Collections.Generic;
using System.Text;

namespace ShowNTell.Domain.Models
{
    /// <summary>
    /// A model for an image post tag.
    /// </summary>
    public class Tag
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public ICollection<ImagePostTag> ImagePosts { get; set; }
    }
}
