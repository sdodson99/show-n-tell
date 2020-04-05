using System;
using System.Runtime.Serialization;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Exceptions
{
    /// <summary>
    /// An exception for a user attempting to like their own content.
    /// </summary>
    public class OwnImagePostLikeException : Exception
    {
        public ImagePost LikedImagePost { get; set; }
        public string LikerEmail { get; set; }

        public OwnImagePostLikeException(ImagePost likedImagePost, string likerEmail)
        {
            LikedImagePost = likedImagePost;
            LikerEmail = likerEmail;
        }

        public OwnImagePostLikeException(string message, ImagePost likedImagePost, string likerEmail) : base(message)
        {
            LikedImagePost = likedImagePost;
            LikerEmail = likerEmail;
        }

        public OwnImagePostLikeException(string message, Exception innerException, ImagePost likedImagePost, string likerEmail) : base(message, innerException)
        {
            LikedImagePost = likedImagePost;
            LikerEmail = likerEmail;
        }
    }
}