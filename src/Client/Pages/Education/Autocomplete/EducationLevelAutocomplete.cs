using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class EducationLevelAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<EducationLevelAutocomplete> L { get; set; } = default!;
    [Inject]
    private IEducationLevelsClient EducationLevelsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<EducationLevelDto> _educationLevels = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["EducationLevel"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchEducationLevels;
        ToStringFunc = GetEducationLevelName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one educationLevel to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationLevelsClient.GetAsync(_value), Snackbar) is { } educationLevel)
        {
            _educationLevels.Add(educationLevel.Adapt<EducationLevelDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchEducationLevels(string value)
    {
        var filter = new SearchEducationLevelsRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationLevelsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfEducationLevelDto response)
        {
            _educationLevels = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _educationLevels.Select(x => x.Id);
    }

    private string GetEducationLevelName(int id)
    {
        var result = _educationLevels.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}