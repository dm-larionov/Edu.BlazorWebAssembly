using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class DisciplineScheduleReplacementAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<DisciplineScheduleReplacementAutocomplete> L { get; set; } = default!;
    [Inject]
    private IDisciplineScheduleReplacementsClient DisciplineScheduleReplacementsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<DisciplineScheduleReplacementDto> _disciplineScheduleReplacements = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["DisciplineScheduleReplacement"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchDisciplineScheduleReplacements;
        ToStringFunc = GetDisciplineScheduleReplacementName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one disciplineScheduleReplacement to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => DisciplineScheduleReplacementsClient.GetAsync(_value), Snackbar) is { } disciplineScheduleReplacement)
        {
            _disciplineScheduleReplacements.Add(disciplineScheduleReplacement.Adapt<DisciplineScheduleReplacementDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchDisciplineScheduleReplacements(string value)
    {
        var filter = new SearchDisciplineScheduleReplacementsRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => DisciplineScheduleReplacementsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfDisciplineScheduleReplacementDto response)
        {
            _disciplineScheduleReplacements = response.Data.OrderBy(x => x.Date).ToList();
        }

        return _disciplineScheduleReplacements.Select(x => x.Id);
    }

    private string GetDisciplineScheduleReplacementName(int id)
    {
        var result = _disciplineScheduleReplacements.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Id}";
    }
}