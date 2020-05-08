using System.Threading.Tasks;
using ShowNTell.API.Models.Notifications;

namespace ShowNTell.API.Services.Notifications
{
    public interface INotificationService
    {
         Task Publish(INotification notification);
    }
}