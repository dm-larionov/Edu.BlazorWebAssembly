using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Extensions;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class CathedraAutocomplete<T> : MudAutocomplete<T>
{
    [Inject]
    private IStringLocalizer<CathedraAutocomplete<T>> L { get; set; } = default!;
    [Inject]
    private ICathedrasClient CathedrasClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<CathedraDto> _cathedras = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Cathedra"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchCathedras;
        ToStringFunc = GetCathedraName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one cathedra to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value is not null &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => CathedrasClient.GetAsync(_value.As<int>()), Snackbar) is { } cathedra)
        {
            _cathedras.Add(cathedra.Adapt<CathedraDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<T>> SearchCathedras(string value)
    {
        var filter = new SearchCathedrasRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => CathedrasClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfCathedraDto response)
        {
            _cathedras = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _cathedras.Select(x => x.Id.As<T>());
    }

    private string GetCathedraName(T id)
    {
        var result = _cathedras.Find(b => b.Id == id.As<int>());
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}