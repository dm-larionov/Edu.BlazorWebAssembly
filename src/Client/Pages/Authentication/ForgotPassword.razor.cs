using Edu.BlazorWebAssembly.Client.Components.Common;
using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Edu.WebApi.Shared.Multitenancy;
using Microsoft.AspNetCore.Components;

namespace Edu.BlazorWebAssembly.Client.Pages.Authentication;

public partial class ForgotPassword
{
    private readonly ForgotPasswordRequest _forgotPasswordRequest = new();
    private CustomValidation? _customValidation;
    private bool BusySubmitting { get; set; }

    [Inject]
    private IUsersClient UsersClient { get; set; } = default!;

    private async Task SubmitAsync()
    {
        BusySubmitting = true;

        await ApiHelper.ExecuteCallGuardedAsync(
            () => UsersClient.ForgotPasswordAsync(_forgotPasswordRequest),
            Snackbar,
            _customValidation);

        BusySubmitting = false;
    }
}