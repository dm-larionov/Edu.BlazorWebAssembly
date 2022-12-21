using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Infrastructure.Notifications;
using Edu.BlazorWebAssembly.Client.Shared;
using Edu.WebApi.Shared.Notifications;
using MediatR.Courier;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Edu.BlazorWebAssembly.Client.Pages.Personal;

public partial class DashboardTest
{
    [Parameter] public int OnlineCount { get; set; } = 24;
    [Parameter] public int NotificationCount { get; set; } = 0;
    [Parameter] public int ActionsCount { get; set; } = 12;
    [Parameter] public int ErrorCount { get; set; } = 0;


    private string userName;
    private bool _loaded;
    private Random rnd;

    [CascadingParameter]
    public Task<AuthenticationState> AuthState { get; set; } = default!;


    protected override async Task OnInitializedAsync()
    {
        rnd = new Random();
        var authState = await AuthState;
        userName = authState.User.Identity.Name;

        _loaded = true;
    }

}