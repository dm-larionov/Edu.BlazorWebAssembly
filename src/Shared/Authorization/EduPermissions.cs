using System.Collections.ObjectModel;

namespace Edu.WebApi.Shared.Authorization;

public static class EduAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class EduResource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);


    public const string Audiences = nameof(Audiences);
    public const string AudienceTypes = nameof(AudienceTypes);
    public const string Cathedras = nameof(Cathedras);
    public const string Departments = nameof(Departments);
    public const string DisciplineScheduleReplacements = nameof(DisciplineScheduleReplacements);
    public const string DisciplineSchedules = nameof(DisciplineSchedules);
    public const string Disciplines = nameof(Disciplines);
    public const string DisciplineSemesters = nameof(DisciplineSemesters);
    public const string EducationCycles = nameof(EducationCycles);
    public const string EducationModules = nameof(EducationModules);
    public const string EducationForms = nameof(EducationForms);
    public const string EducationLevels = nameof(EducationLevels);
    public const string EducationPlans = nameof(EducationPlans);
    public const string Employees = nameof(Employees);
    public const string FixedDisciplines = nameof(FixedDisciplines);
    public const string FixedDisciplineStatuses = nameof(FixedDisciplineStatuses);
    public const string FsesCategoryPartitions = nameof(FsesCategoryPartitions);
    public const string IntermediateCertificationForms = nameof(IntermediateCertificationForms);
    public const string Posts = nameof(Posts);
    public const string ReceivedEducationForms = nameof(ReceivedEducationForms);
    public const string ReceivedEducations = nameof(ReceivedEducations);
    public const string ReceivedSpecialties = nameof(ReceivedSpecialties);
    public const string StudentGroupNameChanges = nameof(StudentGroupNameChanges);
    public const string StudentGroups = nameof(StudentGroups);
}

public static class EduPermissions
{
    private static readonly EduPermission[] _all = new EduPermission[]
    {
        new("View Dashboard", EduAction.View, EduResource.Dashboard),
        new("View Hangfire", EduAction.View, EduResource.Hangfire),
        new("View Users", EduAction.View, EduResource.Users),
        new("Search Users", EduAction.Search, EduResource.Users),
        new("Create Users", EduAction.Create, EduResource.Users),
        new("Update Users", EduAction.Update, EduResource.Users),
        new("Delete Users", EduAction.Delete, EduResource.Users),
        new("Export Users", EduAction.Export, EduResource.Users),
        new("View UserRoles", EduAction.View, EduResource.UserRoles),
        new("Update UserRoles", EduAction.Update, EduResource.UserRoles),
        new("View Roles", EduAction.View, EduResource.Roles),
        new("Create Roles", EduAction.Create, EduResource.Roles),
        new("Update Roles", EduAction.Update, EduResource.Roles),
        new("Delete Roles", EduAction.Delete, EduResource.Roles),
        new("View RoleClaims", EduAction.View, EduResource.RoleClaims),
        new("Update RoleClaims", EduAction.Update, EduResource.RoleClaims),
        new("View Products", EduAction.View, EduResource.Products, IsBasic: true),
        new("Search Products", EduAction.Search, EduResource.Products, IsBasic: true),
        new("Create Products", EduAction.Create, EduResource.Products),
        new("Update Products", EduAction.Update, EduResource.Products),
        new("Delete Products", EduAction.Delete, EduResource.Products),
        new("Export Products", EduAction.Export, EduResource.Products),
        new("View Brands", EduAction.View, EduResource.Brands, IsBasic: true),
        new("Search Brands", EduAction.Search, EduResource.Brands, IsBasic: true),
        new("Create Brands", EduAction.Create, EduResource.Brands),
        new("Update Brands", EduAction.Update, EduResource.Brands),
        new("Delete Brands", EduAction.Delete, EduResource.Brands),
        new("Generate Brands", EduAction.Generate, EduResource.Brands),
        new("Clean Brands", EduAction.Clean, EduResource.Brands),
        new("View Tenants", EduAction.View, EduResource.Tenants, IsRoot: true),
        new("Create Tenants", EduAction.Create, EduResource.Tenants, IsRoot: true),
        new("Update Tenants", EduAction.Update, EduResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", EduAction.UpgradeSubscription, EduResource.Tenants, IsRoot: true),


        new("Search Audiences", EduAction.Search, EduResource.Audiences),
        new("View Audiences", EduAction.View, EduResource.Audiences),
        new("Create Audiences", EduAction.Create, EduResource.Audiences),
        new("Update Audiences", EduAction.Update, EduResource.Audiences),
        new("Delete Audiences", EduAction.Delete, EduResource.Audiences),

        new("Search Audience Types", EduAction.Search, EduResource.AudienceTypes),
        new("View Audience Types", EduAction.View, EduResource.AudienceTypes),
        new("Create Audience Types", EduAction.Create, EduResource.AudienceTypes),
        new("Update Audience Types", EduAction.Update, EduResource.AudienceTypes),
        new("Delete Audience Types", EduAction.Delete, EduResource.AudienceTypes),

        new("Search Cathedras", EduAction.Search, EduResource.Cathedras),
        new("View Cathedras", EduAction.View, EduResource.Cathedras),
        new("Create Cathedras", EduAction.Create, EduResource.Cathedras),
        new("Update Cathedras", EduAction.Update, EduResource.Cathedras),
        new("Delete Cathedras", EduAction.Delete, EduResource.Cathedras),

        new("Search Departments", EduAction.Search, EduResource.Departments),
        new("View Departments", EduAction.View, EduResource.Departments),
        new("Create Departments", EduAction.Create, EduResource.Departments),
        new("Update Departments", EduAction.Update, EduResource.Departments),
        new("Delete Departments", EduAction.Delete, EduResource.Departments),

        new("Search Discipline Schedule Replacements", EduAction.Search, EduResource.DisciplineScheduleReplacements),
        new("View Discipline Schedule Replacements", EduAction.View, EduResource.DisciplineScheduleReplacements),
        new("Create Discipline Schedule Replacements", EduAction.Create, EduResource.DisciplineScheduleReplacements),
        new("Update Discipline Schedule Replacements", EduAction.Update, EduResource.DisciplineScheduleReplacements),
        new("Delete Discipline Schedule Replacements", EduAction.Delete, EduResource.DisciplineScheduleReplacements),

        new("Search Discipline Schedules", EduAction.Search, EduResource.DisciplineSchedules),
        new("View Discipline Schedules", EduAction.View, EduResource.DisciplineSchedules),
        new("Create Discipline Schedules", EduAction.Create, EduResource.DisciplineSchedules),
        new("Update Discipline Schedules", EduAction.Update, EduResource.DisciplineSchedules),
        new("Delete Discipline Schedules", EduAction.Delete, EduResource.DisciplineSchedules),

        new("Search Disciplines", EduAction.Search, EduResource.Disciplines),
        new("View Disciplines", EduAction.View, EduResource.Disciplines),
        new("Create Disciplines", EduAction.Create, EduResource.Disciplines),
        new("Update Disciplines", EduAction.Update, EduResource.Disciplines),
        new("Delete Disciplines", EduAction.Delete, EduResource.Disciplines),

        new("Search Discipline Semesters", EduAction.Search, EduResource.DisciplineSemesters),
        new("View Discipline Semesters", EduAction.View, EduResource.DisciplineSemesters),
        new("Create Discipline Semesters", EduAction.Create, EduResource.DisciplineSemesters),
        new("Update Discipline Semesters", EduAction.Update, EduResource.DisciplineSemesters),
        new("Delete Discipline Semesters", EduAction.Delete, EduResource.DisciplineSemesters),

        new("Search Education Cycles", EduAction.Search, EduResource.EducationCycles),
        new("View Education Cycles", EduAction.View, EduResource.EducationCycles),
        new("Create Education Cycles", EduAction.Create, EduResource.EducationCycles),
        new("Update Education Cycles", EduAction.Update, EduResource.EducationCycles),
        new("Delete Education Cycles", EduAction.Delete, EduResource.EducationCycles),

        new("Search Education Modules", EduAction.Search, EduResource.EducationModules),
        new("View Education Modules", EduAction.View, EduResource.EducationModules),
        new("Create Education Modules", EduAction.Create, EduResource.EducationModules),
        new("Update Education Modules", EduAction.Update, EduResource.EducationModules),
        new("Delete Education Modules", EduAction.Delete, EduResource.EducationModules),

        new("Search Education Forms", EduAction.Search, EduResource.EducationForms),
        new("View Education Forms", EduAction.View, EduResource.EducationForms),
        new("Create Education Forms", EduAction.Create, EduResource.EducationForms),
        new("Update Education Forms", EduAction.Update, EduResource.EducationForms),
        new("Delete Education Forms", EduAction.Delete, EduResource.EducationForms),

        new("Search Education Levels", EduAction.Search, EduResource.EducationLevels),
        new("View Education Levels", EduAction.View, EduResource.EducationLevels),
        new("Create Education Levels", EduAction.Create, EduResource.EducationLevels),
        new("Update Education Levels", EduAction.Update, EduResource.EducationLevels),
        new("Delete Education Levels", EduAction.Delete, EduResource.EducationLevels),

        new("Search Education Plans", EduAction.Search, EduResource.EducationPlans),
        new("View Education Plans", EduAction.View, EduResource.EducationPlans),
        new("Create Education Plans", EduAction.Create, EduResource.EducationPlans),
        new("Update Education Plans", EduAction.Update, EduResource.EducationPlans),
        new("Delete Education Plans", EduAction.Delete, EduResource.EducationPlans),

        new("Search Employees", EduAction.Search, EduResource.Employees),
        new("View Employees", EduAction.View, EduResource.Employees),
        new("Create Employees", EduAction.Create, EduResource.Employees),
        new("Update Employees", EduAction.Update, EduResource.Employees),
        new("Delete Employees", EduAction.Delete, EduResource.Employees),

        new("Search Fixed Disciplines", EduAction.Search, EduResource.FixedDisciplines),
        new("View Fixed Disciplines", EduAction.View, EduResource.FixedDisciplines),
        new("Create Fixed Disciplines", EduAction.Create, EduResource.FixedDisciplines),
        new("Update Fixed Disciplines", EduAction.Update, EduResource.FixedDisciplines),
        new("Delete Fixed Disciplines", EduAction.Delete, EduResource.FixedDisciplines),

        new("Search Fixed Discipline Statuses", EduAction.Search, EduResource.FixedDisciplineStatuses),
        new("View Fixed Discipline Statuses", EduAction.View, EduResource.FixedDisciplineStatuses),
        new("Create Fixed Discipline Statuses", EduAction.Create, EduResource.FixedDisciplineStatuses),
        new("Update Fixed Discipline Statuses", EduAction.Update, EduResource.FixedDisciplineStatuses),
        new("Delete Fixed Discipline Statuses", EduAction.Delete, EduResource.FixedDisciplineStatuses),

        new("Search Fses Category Partitions", EduAction.Search, EduResource.FsesCategoryPartitions),
        new("View Fses Category Partitions", EduAction.View, EduResource.FsesCategoryPartitions),
        new("Create Fses Category Partitions", EduAction.Create, EduResource.FsesCategoryPartitions),
        new("Update Fses Category Partitions", EduAction.Update, EduResource.FsesCategoryPartitions),
        new("Delete Fses Category Partitions", EduAction.Delete, EduResource.FsesCategoryPartitions),

        new("Search Intermediate Certification Forms", EduAction.Search, EduResource.IntermediateCertificationForms),
        new("View Intermediate Certification Forms", EduAction.View, EduResource.IntermediateCertificationForms),
        new("Create Intermediate Certification Forms", EduAction.Create, EduResource.IntermediateCertificationForms),
        new("Update Intermediate Certification Forms", EduAction.Update, EduResource.IntermediateCertificationForms),
        new("Delete Intermediate Certification Forms", EduAction.Delete, EduResource.IntermediateCertificationForms),

        new("Search Posts", EduAction.Search, EduResource.Posts),
        new("View Posts", EduAction.View, EduResource.Posts),
        new("Create Posts", EduAction.Create, EduResource.Posts),
        new("Update Posts", EduAction.Update, EduResource.Posts),
        new("Delete Posts", EduAction.Delete, EduResource.Posts),

        new("Search Received Education Forms", EduAction.Search, EduResource.ReceivedEducationForms),
        new("View Received Education Forms", EduAction.View, EduResource.ReceivedEducationForms),
        new("Create Received Education Forms", EduAction.Create, EduResource.ReceivedEducationForms),
        new("Update Received Education Forms", EduAction.Update, EduResource.ReceivedEducationForms),
        new("Delete Received Education Forms", EduAction.Delete, EduResource.ReceivedEducationForms),

        new("Search Received Educations", EduAction.Search, EduResource.ReceivedEducations),
        new("View Received Educations", EduAction.View, EduResource.ReceivedEducations),
        new("Create Received Educations", EduAction.Create, EduResource.ReceivedEducations),
        new("Update Received Educations", EduAction.Update, EduResource.ReceivedEducations),
        new("Delete Received Educations", EduAction.Delete, EduResource.ReceivedEducations),

        new("Search Received Specialties", EduAction.Search, EduResource.ReceivedSpecialties),
        new("View Received Specialties", EduAction.View, EduResource.ReceivedSpecialties),
        new("Create Received Specialties", EduAction.Create, EduResource.ReceivedSpecialties),
        new("Update Received Specialties", EduAction.Update, EduResource.ReceivedSpecialties),
        new("Delete Received Specialties", EduAction.Delete, EduResource.ReceivedSpecialties),

        new("Search Student Group Name Changes", EduAction.Search, EduResource.StudentGroupNameChanges),
        new("View Student Group Name Changes", EduAction.View, EduResource.StudentGroupNameChanges),
        new("Create Student Group Name Changes", EduAction.Create, EduResource.StudentGroupNameChanges),
        new("Update Student Group Name Changes", EduAction.Update, EduResource.StudentGroupNameChanges),
        new("Delete Student Group Name Changes", EduAction.Delete, EduResource.StudentGroupNameChanges),

        new("Search Student Groups", EduAction.Search, EduResource.StudentGroups),
        new("View Student Groups", EduAction.View, EduResource.StudentGroups),
        new("Create Student Groups", EduAction.Create, EduResource.StudentGroups),
        new("Update Student Groups", EduAction.Update, EduResource.StudentGroups),
        new("Delete Student Groups", EduAction.Delete, EduResource.StudentGroups),
    };

    public static IReadOnlyList<EduPermission> All { get; } = new ReadOnlyCollection<EduPermission>(_all);
    public static IReadOnlyList<EduPermission> Root { get; } = new ReadOnlyCollection<EduPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<EduPermission> Admin { get; } = new ReadOnlyCollection<EduPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<EduPermission> Basic { get; } = new ReadOnlyCollection<EduPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record EduPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}