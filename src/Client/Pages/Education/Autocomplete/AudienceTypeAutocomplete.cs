using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class AudienceTypeAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<AudienceTypeAutocomplete> L { get; set; } = default!;
    [Inject]
    private IAudienceTypesClient AudienceTypesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<AudienceTypeDto> _audienceTypes = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["AudienceType"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchAudienceTypes;
        ToStringFunc = GetAudienceTypeName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one audienceType to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => AudienceTypesClient.GetAsync(_value), Snackbar) is { } audienceType)
        {
            _audienceTypes.Add(audienceType.Adapt<AudienceTypeDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchAudienceTypes(string value)
    {
        var filter = new SearchAudienceTypesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => AudienceTypesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfAudienceTypeDto response)
        {
            _audienceTypes = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _audienceTypes.Select(x => x.Id);
    }

    private string GetAudienceTypeName(int id)
    {
        var result = _audienceTypes.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}