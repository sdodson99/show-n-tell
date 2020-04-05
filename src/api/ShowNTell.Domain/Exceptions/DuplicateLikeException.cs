using System;
using System.Runtime.Serialization;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Exceptions
{
    /// <summary>
    /// An exception for a user attempting to like content more than once.
    /// </summary>
    public class DuplicateLikeException : Exception
    {
        public ImagePost LikedImagePost { get; set; }
        public string LikerEmail { get; set; }

        public DuplicateLikeException(ImagePost likedImagePost, string likerEmail)
        {
            LikedImagePost = likedImagePost;
            LikerEmail = likerEmail;
        }

        public DuplicateLikeException(string message, ImagePost likedImagePost, string likerEmail) : base(message)
        {
            LikedImagePost = likedImagePost;
            LikerEmail = likerEmail;
        }

        public DuplicateLikeException(string message, Exception innerException, ImagePost likedImagePost, string likerEmail) : base(message, innerException)
        {
            LikedImagePost = likedImagePost;
            LikerEmail = likerEmail;
        }
    }
}