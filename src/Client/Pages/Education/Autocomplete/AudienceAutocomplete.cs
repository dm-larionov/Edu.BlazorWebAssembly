using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Extensions;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class AudienceAutocomplete<T> : MudAutocomplete<T>
{
    [Inject]
    private IStringLocalizer<AudienceAutocomplete<T>> L { get; set; } = default!;
    [Inject]
    private IAudiencesClient AudiencesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<AudienceDto> _audiences = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Audience"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchAudiences;
        ToStringFunc = GetAudienceName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one audience to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != null &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => AudiencesClient.GetAsync(_value.As<int>()), Snackbar) is { } audience)
        {
            _audiences.Add(audience.Adapt<AudienceDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<T>> SearchAudiences(string value)
    {
        var filter = new SearchAudiencesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => AudiencesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfAudienceDto response)
        {
            _audiences = response.Data.OrderBy(x => x.Number).ToList();
        }

        return _audiences.Select(x => x.Id.As<T>());
    }

    private string GetAudienceName(T id)
    {
        var result = _audiences.Find(b => b.Id == id.As<int>());
        if (result is null)
            return string.Empty;
        return $"{result.Number}";
    }
}