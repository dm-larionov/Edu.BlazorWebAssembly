using Edu.BlazorWebAssembly.Client.Components.Common;
using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Infrastructure.Auth;
using Edu.BlazorWebAssembly.Client.Shared;
using Edu.WebApi.Shared.Multitenancy;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Authentication;

public partial class Login
{
    [CascadingParameter]
    public Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    public IAuthenticationService AuthService { get; set; } = default!;

    private CustomValidation? _customValidation;

    public bool BusySubmitting { get; set; }

    private readonly TokenRequest _tokenRequest = new();
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        if (AuthService.ProviderType == AuthProvider.AzureAd)
        {
            AuthService.NavigateToExternalLogin(Navigation.Uri);
            return;
        }

        var authState = await AuthState;
        if (authState.User.Identity?.IsAuthenticated is true)
        {
            Navigation.NavigateTo("/");
        }
    }

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }

    private void FillAdministratorCredentials()
    {
        _tokenRequest.Email = MultitenancyConstants.Root.EmailAddress;
        _tokenRequest.Password = MultitenancyConstants.DefaultPassword;
    }

    private void FillEduHeadCredentials()
    {
        _tokenRequest.Email = MultitenancyConstants.EduHead.EmailAddress;
        _tokenRequest.Password = MultitenancyConstants.DefaultPassword;
    }

    private void FillEduEmployeeCredentials()
    {
        _tokenRequest.Email = MultitenancyConstants.EduEmployee.EmailAddress;
        _tokenRequest.Password = MultitenancyConstants.DefaultPassword;
    }

    private void FillTeacherCredentials()
    {
        _tokenRequest.Email = MultitenancyConstants.Teacher.EmailAddress;
        _tokenRequest.Password = MultitenancyConstants.DefaultPassword;
    }

    private async Task SubmitAsync()
    {
        BusySubmitting = true;

        if (await ApiHelper.ExecuteCallGuardedAsync(
            () => AuthService.LoginAsync(_tokenRequest),
            Snackbar,
            _customValidation))
        {
            Snackbar.Add($"Logged in as {_tokenRequest.Email}", Severity.Info);
        }

        BusySubmitting = false;
    }
}