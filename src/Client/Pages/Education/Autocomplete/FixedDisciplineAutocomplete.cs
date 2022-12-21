using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class FixedDisciplineAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<FixedDisciplineAutocomplete> L { get; set; } = default!;
    [Inject]
    private IFixedDisciplinesClient FixedDisciplinesClient { get; set; } = default!;

    [Inject]
    private IEmployeesClient EmployeesClient { get; set; } = default!;
    [Inject]
    private IDisciplineSemestersClient DisciplineSemestersClient { get; set; } = default!;
    [Inject]
    private IStudentGroupsClient StudentGroupsClient { get; set; } = default!;
    [Inject]
    private IDisciplinesClient DisciplinesClient { get; set; } = default!;
    [Inject]
    private IFixedDisciplineStatusesClient FixedDisciplineStatusesClient { get; set; } = default!;


    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<FixedDisciplineDto> _fixedDisciplines = new();

    protected ICollection<EmployeeDto> _employees = default!;
    protected ICollection<DisciplineSemesterDto> _disciplineSemesters = default!;
    protected ICollection<StudentGroupDto> _studentGroups = default!;
    protected ICollection<FixedDisciplineStatusDto> _fixedDisciplineStatuses = default!;
    protected ICollection<DisciplineDto> _disciplines = default!;

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["FixedDiscipline"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchFixedDisciplines;
        ToStringFunc = GetFixedDisciplineName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one fixedDiscipline to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => FixedDisciplinesClient.GetAsync(_value), Snackbar) is { } fixedDiscipline)
        {
            _fixedDisciplines.Add(fixedDiscipline.Adapt<FixedDisciplineDto>());
            ForceRender(true);
        }
        _employees = await EmployeesClient.GetAllByIdRangeAsync(_fixedDisciplines.Select(x => x.FixingEmployeeId).Distinct());
        _disciplineSemesters = await DisciplineSemestersClient.GetAllByIdRangeAsync(_fixedDisciplines.Select(x => x.DisciplineSemesterId).Distinct());
        _studentGroups = await StudentGroupsClient.GetAllByIdRangeAsync(_fixedDisciplines.Select(x => x.StudentGroupId).Distinct());
        _fixedDisciplineStatuses = await FixedDisciplineStatusesClient.GetAllByIdRangeAsync(_fixedDisciplines.Select(x => x.FixedDisciplineStatusId).Distinct());
        _disciplines = await DisciplinesClient.GetAllByIdRangeAsync(_disciplineSemesters.Select(x => x.DisciplineId).Distinct());

        ForceRender(true);
    }

    private async Task<IEnumerable<int>> SearchFixedDisciplines(string value)
    {
        var filter = new SearchFixedDisciplinesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => FixedDisciplinesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfFixedDisciplineDto response)
        {
            _fixedDisciplines = response.Data.OrderBy(x => x.Id).ToList();
        }

        return _fixedDisciplines.Select(x => x.Id);
    }

    private string GetFixedDisciplineName(int id)
    {
        var result = _fixedDisciplines.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{GetEmoloyeeFio(result.FixingEmployeeId)} {GetStudentGroupInfo(result.StudentGroupId)} {GetDisciplineSemesterInfo(result.DisciplineSemesterId)}";
    }

    private string GetEmoloyeeFio(int id)
    {
        var result = _employees.FirstOrDefault(x => x.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Lastname} {result.Firstname} {result.Middlename}";
    }

    private string GetDisciplineSemesterInfo(int id)
    {
        var result = _disciplineSemesters.FirstOrDefault(x => x.Id == id);
        var discipline = _disciplines.FirstOrDefault(x => x.Id == result!.DisciplineId);
        if (result is null)
            return string.Empty;
        return $" {result.SemesterNumber} {discipline!.DisciplineIndex} ({GetHoursSum(result)} ч.) {discipline.Name}";
    }

    private string GetStudentGroupInfo(int id)
    {
        var result = _studentGroups.FirstOrDefault(x => x.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }

    private string GetStatusInfo(int id)
    {
        var result = _fixedDisciplineStatuses.FirstOrDefault(x => x.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }

    private int GetHoursSum(DisciplineSemesterDto disciplineSemester)
    {
        return disciplineSemester.TheoryLessonHours +
               disciplineSemester.PracticeWorkHours +
               disciplineSemester.LaboratoryWorkHours +
               disciplineSemester.ControlWorkHours +
               disciplineSemester.IndependentWorkHours +
               disciplineSemester.ConsultationHours +
               disciplineSemester.ExamHours +
               disciplineSemester.EducationalPracticeHours +
               disciplineSemester.ProductionPracticeHours;
    }
}