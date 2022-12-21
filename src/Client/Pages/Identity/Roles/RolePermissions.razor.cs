using System.Security.Claims;
using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Infrastructure.Auth;
using Edu.BlazorWebAssembly.Client.Shared;
using Edu.WebApi.Shared.Authorization;
using Edu.WebApi.Shared.Multitenancy;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Identity.Roles;

public partial class RolePermissions
{
    [Parameter]
    public string Id { get; set; } = default!; // from route
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IRolesClient RolesClient { get; set; } = default!;

    private Dictionary<string, List<PermissionViewModel>> _groupedRoleClaims = default!;

    public string _title = string.Empty;
    public string _description = string.Empty;

    private string _searchString = string.Empty;

    private bool _canEditRoleClaims;
    private bool _canSearchRoleClaims;
    private bool _loaded;

    static RolePermissions() => TypeAdapterConfig<EduPermission, PermissionViewModel>.NewConfig().MapToConstructor(true);

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState;
        _canEditRoleClaims = await AuthService.HasPermissionAsync(state.User, EduAction.Update, EduResource.RoleClaims);
        _canSearchRoleClaims = await AuthService.HasPermissionAsync(state.User, EduAction.View, EduResource.RoleClaims);

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => RolesClient.GetByIdWithPermissionsAsync(Id), Snackbar)
            is RoleDto role && role.Permissions is not null)
        {
            _title = string.Format(L["{0} Permissions"], role.Name);
            _description = string.Format(L["Manage {0} Role Permissions"], role.Name);

            var permissions = "root" == MultitenancyConstants.Root.Id
                ? EduPermissions.All
                : EduPermissions.Admin;

            _groupedRoleClaims = permissions
                .GroupBy(p => p.Resource)
                .ToDictionary(g => g.Key, g => g.Select(p =>
                {
                    var permission = p.Adapt<PermissionViewModel>();
                    permission.Enabled = role.Permissions.Contains(permission.Name);
                    return permission;
                }).ToList());
        }

        _loaded = true;
    }

    private Color GetGroupBadgeColor(int selected, int all)
    {
        if (selected == 0)
            return Color.Error;

        if (selected == all)
            return Color.Success;

        return Color.Info;
    }

    private async Task SaveAsync()
    {
        var allPermissions = _groupedRoleClaims.Values.SelectMany(a => a);
        var selectedPermissions = allPermissions.Where(a => a.Enabled);
        var request = new UpdateRolePermissionsRequest()
        {
            RoleId = Id,
            Permissions = selectedPermissions.Where(x => x.Enabled).Select(x => x.Name).ToList(),
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => RolesClient.UpdatePermissionsAsync(request.RoleId, request),
                Snackbar,
                successMessage: L["Updated Permissions."])
            is not null)
        {
            Navigation.NavigateTo("/roles");
        }
    }

    private bool Search(PermissionViewModel permission) =>
        string.IsNullOrWhiteSpace(_searchString)
            || permission.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) is true
            || permission.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase) is true;
}

public record PermissionViewModel : EduPermission
{
    public bool Enabled { get; set; }

    public PermissionViewModel(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
        : base(Description, Action, Resource, IsBasic, IsRoot)
    {
    }
}