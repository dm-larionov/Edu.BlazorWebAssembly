using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class EducationCycleAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<EducationCycleAutocomplete> L { get; set; } = default!;
    [Inject]
    private IEducationCyclesClient EducationCyclesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<EducationCycleDto> _educationCycles = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["EducationCycle"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchEducationCycles;
        ToStringFunc = GetEducationCycleName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one educationCycle to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationCyclesClient.GetAsync(_value), Snackbar) is { } educationCycle)
        {
            _educationCycles.Add(educationCycle.Adapt<EducationCycleDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchEducationCycles(string value)
    {
        var filter = new SearchEducationCyclesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationCyclesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfEducationCycleDto response)
        {
            _educationCycles = response.Data.OrderBy(x => x.EducationCycleIndex).ToList();
        }

        return _educationCycles.Select(x => x.Id);
    }

    private string GetEducationCycleName(int id)
    {
        var result = _educationCycles.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.EducationCycleIndex} - {result.Name}";
    }
}