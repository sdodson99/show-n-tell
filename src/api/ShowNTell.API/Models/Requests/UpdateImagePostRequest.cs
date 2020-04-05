using System.Collections.Generic;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Models.Requests
{
    /// <summary>
    /// A model for updating an image post.
    /// </summary>
    public class UpdateImagePostRequest
    {
        public string Description { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}