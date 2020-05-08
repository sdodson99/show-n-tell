using System.Threading.Tasks;
using MediatR;

namespace ShowNTell.API.Services.Notifications
{
    public class MediatRNotificationService : INotificationService
    {
        private readonly IMediator _mediator;

        public MediatRNotificationService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish(Models.Notifications.INotification notification)
        {
            await _mediator.Publish(notification);
        }
    }
}