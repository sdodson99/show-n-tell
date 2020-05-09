using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Models.Notifications.Likes
{
    public class UnlikeNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.IMAGE_POST_UNLIKE;

        public object Data => LikeResponse;

        public LikeResponse LikeResponse { get; }

        public UnlikeNotification(LikeResponse likeResponse)
        {
            LikeResponse = likeResponse;
        }
    }
}