using Edu.BlazorWebAssembly.Client.Infrastructure.Notifications;
using Edu.BlazorWebAssembly.Client.Infrastructure.Preferences;
using MediatR.Courier;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Components.Common;

public class EduTable<T> : MudTable<T>
{
    [Inject]
    private IClientPreferenceManager ClientPreferences { get; set; } = default!;
    [Inject]
    protected ICourier Courier { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference clientPreference)
        {
            SetTablePreference(clientPreference.TablePreference);
        }

        Courier.SubscribeWeak<NotificationWrapper<EduTablePreference>>(wrapper =>
        {
            SetTablePreference(wrapper.Notification);
            StateHasChanged();
        });

        await base.OnInitializedAsync();
    }

    private void SetTablePreference(EduTablePreference tablePreference)
    {
        Dense = tablePreference.IsDense;
        Striped = tablePreference.IsStriped;
        Bordered = tablePreference.HasBorder;
        Hover = tablePreference.IsHoverable;
    }
}