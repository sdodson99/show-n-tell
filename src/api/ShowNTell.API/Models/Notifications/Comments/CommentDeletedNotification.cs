using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Models.Notifications.Comments
{
    public class CommentDeletedNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.IMAGE_POST_COMMENT_DELETED;

        public object Data => CommentDeletedResponse;

        public CommentDeletedResponse CommentDeletedResponse { get; }

        public CommentDeletedNotification(CommentDeletedResponse commentDeletedResponse)
        {
            CommentDeletedResponse = commentDeletedResponse;
        }
    }
}