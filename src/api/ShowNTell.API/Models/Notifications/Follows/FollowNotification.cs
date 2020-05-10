using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Models.Notifications.Follows
{
    public class FollowNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.PROFILE_FOLLOW;

        public object Data => FollowResponse;

        public FollowResponse FollowResponse { get; }

        public FollowNotification(FollowResponse followResponse)
        {
            FollowResponse = followResponse;
        }
    }
}