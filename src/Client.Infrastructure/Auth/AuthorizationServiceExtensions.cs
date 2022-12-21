using Edu.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Edu.BlazorWebAssembly.Client.Infrastructure.Auth;

public static class AuthorizationServiceExtensions
{
    public static async Task<bool> HasPermissionAsync(this IAuthorizationService service, ClaimsPrincipal user, string action, string resource) =>
        (await service.AuthorizeAsync(user, null, EduPermission.NameFor(action, resource))).Succeeded;

    public static async Task<bool> HasPermissionRoleAsync(this IAuthorizationService service, ClaimsPrincipal user, string role) =>
        (await service.AuthorizeAsync(user, null, role)).Succeeded;
}