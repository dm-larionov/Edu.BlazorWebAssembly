using Edu.WebApi.Shared.Notifications;

namespace Edu.BlazorWebAssembly.Client.Infrastructure.Notifications;

public interface INotificationPublisher
{
    Task PublishAsync(INotificationMessage notification);
}