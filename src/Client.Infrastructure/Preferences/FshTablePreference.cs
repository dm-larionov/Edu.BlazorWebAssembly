using Edu.WebApi.Shared.Notifications;

namespace Edu.BlazorWebAssembly.Client.Infrastructure.Preferences;

public class EduTablePreference : INotificationMessage
{
    public bool IsDense { get; set; }
    public bool IsStriped { get; set; }
    public bool HasBorder { get; set; }
    public bool IsHoverable { get; set; }
}