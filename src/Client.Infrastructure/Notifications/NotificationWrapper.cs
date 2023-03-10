using Edu.WebApi.Shared.Notifications;
using MediatR;

namespace Edu.BlazorWebAssembly.Client.Infrastructure.Notifications;

public class NotificationWrapper<TNotificationMessage> : INotification
    where TNotificationMessage : INotificationMessage
{
    public NotificationWrapper(TNotificationMessage notification) => Notification = notification;

    public TNotificationMessage Notification { get; }
}