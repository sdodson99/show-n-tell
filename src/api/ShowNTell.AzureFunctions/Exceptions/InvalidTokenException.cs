using System;
using System.Runtime.Serialization;

namespace ShowNTell.AzureFunctions.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public string Token { get; set; }

        public InvalidTokenException(string token)
        {
            Token = token;
        }

        public InvalidTokenException(string message, string token) : base(message)
        {
            Token = token;
        }

        public InvalidTokenException(string message, Exception innerException, string token) : base(message, innerException)
        {
            Token = token;
        }
    }
}