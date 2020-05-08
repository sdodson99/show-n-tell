using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Notifications;

namespace ShowNTell.API.Services.Notifications.Handlers
{
    public class SignalRNotificationHandler<T> : INotificationHandler<ISignalRNotification>
    {
        private readonly IHubContext<ShowNTellHub> _hub;

        public SignalRNotificationHandler(IHubContext<ShowNTellHub> hub)
        {
            _hub = hub;
        }

        public async Task Handle(ISignalRNotification notification, CancellationToken cancellationToken)
        {
            await _hub.Clients.All.SendAsync(notification.MethodName, notification.Data);
        }
    }
}