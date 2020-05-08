using ShowNTell.API.Hubs;

namespace ShowNTell.API.Models.Notifications.ImagePosts
{
    public class ImagePostDeletedNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.IMAGE_POST_DELETED;

        public object Data => ImagePostId;

        public int ImagePostId { get; }

        public ImagePostDeletedNotification(int imagePostId)
        {
            ImagePostId = imagePostId;
        }
    }
}