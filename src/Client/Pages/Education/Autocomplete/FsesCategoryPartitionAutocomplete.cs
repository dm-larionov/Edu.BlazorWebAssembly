using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class FsesCategoryPartitionAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<FsesCategoryPartitionAutocomplete> L { get; set; } = default!;
    [Inject]
    private IFsesCategoryPartitionsClient FsesCategoryPartitionsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<FsesCategoryPartitionDto> _fsesCategoryPartitions = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["FsesCategoryPartition"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchFsesCategoryPartitions;
        ToStringFunc = GetFsesCategoryPartitionName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one fsesCategoryPartition to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => FsesCategoryPartitionsClient.GetAsync(_value), Snackbar) is { } fsesCategoryPartition)
        {
            _fsesCategoryPartitions.Add(fsesCategoryPartition.Adapt<FsesCategoryPartitionDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchFsesCategoryPartitions(string value)
    {
        var filter = new SearchFsesCategoryPartitionsRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => FsesCategoryPartitionsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfFsesCategoryPartitionDto response)
        {
            _fsesCategoryPartitions = response.Data.ToList();
        }

        return _fsesCategoryPartitions.Select(x => x.Id);
    }

    private string GetFsesCategoryPartitionName(int id)
    {
        var result = _fsesCategoryPartitions.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.FirstPartNumber}.{result.SecondPartNumber}.{result.ThirdPathNumber} - {result.Name}";
    }
}