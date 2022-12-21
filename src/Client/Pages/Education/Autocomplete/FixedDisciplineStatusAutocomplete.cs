using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class FixedDisciplineStatusAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<FixedDisciplineStatusAutocomplete> L { get; set; } = default!;
    [Inject]
    private IFixedDisciplineStatusesClient FixedDisciplineStatusesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<FixedDisciplineStatusDto> _fixedDisciplineStatuss = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["FixedDisciplineStatus"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchFixedDisciplineStatuses;
        ToStringFunc = GetFixedDisciplineStatusName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one fixedDisciplineStatus to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => FixedDisciplineStatusesClient.GetAsync(_value), Snackbar) is { } fixedDisciplineStatus)
        {
            _fixedDisciplineStatuss.Add(fixedDisciplineStatus.Adapt<FixedDisciplineStatusDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchFixedDisciplineStatuses(string value)
    {
        var filter = new SearchFixedDisciplineStatusesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => FixedDisciplineStatusesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfFixedDisciplineStatusDto response)
        {
            _fixedDisciplineStatuss = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _fixedDisciplineStatuss.Select(x => x.Id);
    }

    private string GetFixedDisciplineStatusName(int id)
    {
        var result = _fixedDisciplineStatuss.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}