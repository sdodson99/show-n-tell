using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Models.Notifications.ImagePosts
{
    public class ImagePostUpdatedNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.IMAGE_POST_UPDATED;

        public object Data => ImagePostResponse;

        public ImagePostResponse ImagePostResponse { get; }

        public ImagePostUpdatedNotification(ImagePostResponse imagePostResponse)
        {
            ImagePostResponse = imagePostResponse;
        }
    }
}