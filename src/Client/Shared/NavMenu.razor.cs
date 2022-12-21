using Edu.BlazorWebAssembly.Client.Infrastructure.Auth;
using Edu.BlazorWebAssembly.Client.Infrastructure.Common;
using Edu.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Edu.BlazorWebAssembly.Client.Shared;

public partial class NavMenu
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    private string? _hangfireUrl;
    private bool _canViewHangfire;
    private bool _canViewDashboard;
    private bool _canViewRoles;
    private bool _canViewUsers;
    private bool _canViewProducts;
    private bool _canViewBrands;
    private bool _canViewTenants;

    private bool _canViewAudiences;
    private bool _canViewAudienceTypes;
    private bool _canViewCathedras;
    private bool _canViewDisciplines;
    private bool _canViewDisciplineScheduleReplacements;
    private bool _canViewDisciplineSchedules;
    private bool _canViewDisciplineSemesters;
    private bool _canViewEducationCycles;
    private bool _canViewEducationForms;
    private bool _canViewEducationLevels;
    private bool _canViewEducationModules;
    private bool _canViewEducationPlans;
    private bool _canViewEmployees;
    private bool _canViewFixedDisciplines;
    private bool _canViewFixedDisciplineStatuses;
    private bool _canViewFsesCategoryPartitions;
    private bool _canViewIntermediateCertificationForms;
    private bool _canViewPosts;
    private bool _canViewReceivedEducationForms;
    private bool _canViewReceivedEducations;
    private bool _canViewReceivedSpecialties;
    private bool _canViewStudentGroupNameChanges;
    private bool _canViewStudentGroups;

    private bool _isAdmin;
    private bool _isHeadUmo;
    private bool _isTeacher;

    private bool CanViewAdministrationGroup => _canViewUsers || _canViewRoles || _canViewTenants;

    protected override async Task OnParametersSetAsync()
    {
        _hangfireUrl = Config[ConfigNames.ApiBaseUrl] + "jobs";
        var user = (await AuthState).User;

        _isAdmin = user.IsInRole("Admin");
        _isHeadUmo = user.IsInRole("HeadUmo");
        _isTeacher = user.IsInRole("Teacher");

        _canViewHangfire = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Hangfire);
        _canViewDashboard = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Dashboard);
        _canViewRoles = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Roles);
        _canViewUsers = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Users);
        _canViewProducts = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Products);
        _canViewBrands = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Brands);
        _canViewTenants = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Tenants);

        _canViewAudiences = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Audiences);
        _canViewAudienceTypes = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.AudienceTypes);
        _canViewCathedras = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Cathedras);
        _canViewDisciplines = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Disciplines);
        _canViewDisciplineScheduleReplacements = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.DisciplineScheduleReplacements);
        _canViewDisciplineSchedules = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.DisciplineSchedules);
        _canViewDisciplineSemesters = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.DisciplineSemesters);
        _canViewEducationCycles = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.EducationCycles);
        _canViewEducationForms = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.EducationForms);
        _canViewEducationLevels = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.EducationLevels);
        _canViewEducationModules = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.EducationModules);
        _canViewEducationPlans = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.EducationPlans);
        _canViewEmployees = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Employees);
        _canViewFixedDisciplines = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.FixedDisciplines);
        _canViewFixedDisciplineStatuses = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.FixedDisciplineStatuses);
        _canViewFsesCategoryPartitions = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.FsesCategoryPartitions);
        _canViewIntermediateCertificationForms = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.IntermediateCertificationForms);
        _canViewPosts = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.Posts);
        _canViewReceivedEducationForms = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.ReceivedEducationForms);
        _canViewReceivedEducations = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.ReceivedEducations);
        _canViewReceivedSpecialties = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.ReceivedSpecialties);
        _canViewStudentGroupNameChanges = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.StudentGroupNameChanges);
        _canViewStudentGroups = await AuthService.HasPermissionAsync(user, EduAction.View, EduResource.StudentGroups);
    }
}