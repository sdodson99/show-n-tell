using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShowNTell.API.Models.Requests
{
    /// <summary>
    /// A model for creating an image post.
    /// </summary>
    public class CreateImagePostRequest
    {
        [Required]
        public IFormFile Image {get; set; }
        public string Description { get; set; }

        private IEnumerable<string> _tags;
        public IEnumerable<string> Tags
        {
            get => _tags;
            set
            {
                if(value.Count() > 0 && value.FirstOrDefault().Length > 0)
                {
                    _tags = value.Aggregate((s1, s2) => $"{s1.Trim(',')},{s2.Trim(',')}").Trim(',').Split(',');
                }
                else
                {
                    _tags = new List<string>();
                }
            }
        }
    }
}
