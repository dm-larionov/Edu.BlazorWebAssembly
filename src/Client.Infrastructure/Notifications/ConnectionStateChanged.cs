using Edu.WebApi.Shared.Notifications;

namespace Edu.BlazorWebAssembly.Client.Infrastructure.Notifications;

public record ConnectionStateChanged(ConnectionState State, string? Message) : INotificationMessage;