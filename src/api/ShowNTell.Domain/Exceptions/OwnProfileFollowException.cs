using System;

namespace ShowNTell.Domain.Exceptions
{
    public class OwnProfileFollowException : Exception
    {
        public string UserEmail { get; set; }

        public OwnProfileFollowException(string userEmail)
        {
            UserEmail = userEmail;
        }

        public OwnProfileFollowException(string message, string userEmail) : base(message)
        {
            UserEmail = userEmail;
        }

        public OwnProfileFollowException(string message, Exception innerException, string userEmail) : base(message, innerException)
        {
            UserEmail = userEmail;
        }
    }
}