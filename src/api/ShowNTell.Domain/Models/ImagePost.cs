﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShowNTell.Domain.Models
{
    /// <summary>
    /// A model for an image post.
    /// </summary>
    public class ImagePost
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string ImageUri { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public User User { get; set; }
        public ICollection<ImagePostTag> Tags { get;set; }
        public ICollection<Like> Likes { get;set; }
        public ICollection<Comment> Comments { get;set; }
    }
}
