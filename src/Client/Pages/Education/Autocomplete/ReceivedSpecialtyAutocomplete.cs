using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class ReceivedSpecialtyAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<ReceivedSpecialtyAutocomplete> L { get; set; } = default!;
    [Inject]
    private IReceivedSpecialtiesClient ReceivedSpecialtysClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<ReceivedSpecialtyDto> _receivedSpecialtys = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["ReceivedSpecialty"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchReceivedSpecialtys;
        ToStringFunc = GetReceivedSpecialtyName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one receivedSpecialty to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => ReceivedSpecialtysClient.GetAsync(_value), Snackbar) is { } receivedSpecialty)
        {
            _receivedSpecialtys.Add(receivedSpecialty.Adapt<ReceivedSpecialtyDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchReceivedSpecialtys(string value)
    {
        var filter = new SearchReceivedSpecialtiesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => ReceivedSpecialtysClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfReceivedSpecialtyDto response)
        {
            _receivedSpecialtys = response.Data.OrderBy(x => x.Qualification).ToList();
        }

        return _receivedSpecialtys.Select(x => x.Id);
    }

    private string GetReceivedSpecialtyName(int id)
    {
        var result = _receivedSpecialtys.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Qualification}";
    }
}