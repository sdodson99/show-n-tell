namespace ShowNTell.API.Models.Responses
{
    /// <summary>
    /// A model for a follower response from the API.
    /// </summary>
    public class FollowResponse
    {
        public string UserEmail { get; set; }
        public string UserUsername { get; set; }
        public string FollowerEmail { get; set; }
        public string FollowerUsername { get; set; }
    }
}