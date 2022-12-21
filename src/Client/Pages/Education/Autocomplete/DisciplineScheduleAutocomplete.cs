using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class DisciplineScheduleAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<DisciplineScheduleAutocomplete> L { get; set; } = default!;
    [Inject]
    private IDisciplineSchedulesClient DisciplineSchedulesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<DisciplineScheduleDto> _disciplineSchedules = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["DisciplineSchedule"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchDisciplineSchedules;
        ToStringFunc = GetDisciplineScheduleName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one disciplineSchedule to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => DisciplineSchedulesClient.GetAsync(_value), Snackbar) is { } disciplineSchedule)
        {
            _disciplineSchedules.Add(disciplineSchedule.Adapt<DisciplineScheduleDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchDisciplineSchedules(string value)
    {
        var filter = new SearchDisciplineSchedulesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => DisciplineSchedulesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfDisciplineScheduleDto response)
        {
            _disciplineSchedules = response.Data.OrderBy(x => x.Date).ToList();
        }

        return _disciplineSchedules.Select(x => x.Id);
    }

    private string GetDisciplineScheduleName(int id)
    {
        var result = _disciplineSchedules.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Id}";
    }
}