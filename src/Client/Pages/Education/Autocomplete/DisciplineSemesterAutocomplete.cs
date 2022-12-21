using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class DisciplineSemesterAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<DisciplineSemesterAutocomplete> L { get; set; } = default!;
    [Inject]
    private IDisciplineSemestersClient DisciplineSemestersClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<DisciplineSemesterDto> _disciplineSemesters = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["DisciplineSemester"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchDisciplineSemesters;
        ToStringFunc = GetDisciplineSemesterName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one disciplineSemester to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => DisciplineSemestersClient.GetAsync(_value), Snackbar) is { } disciplineSemester)
        {
            _disciplineSemesters.Add(disciplineSemester.Adapt<DisciplineSemesterDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchDisciplineSemesters(string value)
    {
        var filter = new SearchDisciplineSemestersRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => DisciplineSemestersClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfDisciplineSemesterDto response)
        {
            _disciplineSemesters = response.Data.OrderBy(x => x.SemesterNumber).ToList();
        }

        return _disciplineSemesters.Select(x => x.Id);
    }

    private string GetDisciplineSemesterName(int id)
    {
        var result = _disciplineSemesters.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Id}";
    }
}