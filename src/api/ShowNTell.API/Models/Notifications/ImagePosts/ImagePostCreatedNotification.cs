using MediatR;
using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Models.Notifications.ImagePosts
{
    public class ImagePostCreatedNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.IMAGE_POST_CREATED;
        public object Data => ImagePostResponse;

        public ImagePostResponse ImagePostResponse { get; }

        public ImagePostCreatedNotification(ImagePostResponse imagePostResponse)
        {
            ImagePostResponse = imagePostResponse;
        }
    }
}