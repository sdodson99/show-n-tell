using System.Collections.Generic;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Models.Requests
{
    public class UpdateImagePostRequest
    {
        public string Description { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}