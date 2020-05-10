using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Models.Notifications.Follows
{
    public class UnfollowNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.PROFILE_UNFOLLOW;

        public object Data => FollowResponse;

        public FollowResponse FollowResponse { get; }

        public UnfollowNotification(FollowResponse followResponse)
        {
            FollowResponse = followResponse;
        }
    }
}