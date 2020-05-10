using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Models.Notifications.Likes
{
    public class LikeNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.IMAGE_POST_LIKE;

        public object Data => LikeResponse;

        public LikeResponse LikeResponse { get; }

        public LikeNotification(LikeResponse likeResponse)
        {
            LikeResponse = likeResponse;
        }
    }
}