using Edu.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Edu.BlazorWebAssembly.Client.Infrastructure.Auth;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = EduPermission.NameFor(action, resource);
}