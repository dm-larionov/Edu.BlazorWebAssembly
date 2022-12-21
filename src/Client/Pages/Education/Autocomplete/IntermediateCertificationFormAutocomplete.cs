using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Extensions;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class IntermediateCertificationFormAutocomplete<T> : MudAutocomplete<T>
{
    [Inject]
    private IStringLocalizer<IntermediateCertificationFormAutocomplete<T>> L { get; set; } = default!;
    [Inject]
    private IIntermediateCertificationFormsClient IntermediateCertificationFormsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<IntermediateCertificationFormDto> _intermediateCertificationForms = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["IntermediateCertificationForm"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchIntermediateCertificationForms;
        ToStringFunc = GetIntermediateCertificationFormName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one intermediateCertificationForm to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value is not null &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => IntermediateCertificationFormsClient.GetAsync(_value.As<int>()), Snackbar) is { } intermediateCertificationForm)
        {
            _intermediateCertificationForms.Add(intermediateCertificationForm.Adapt<IntermediateCertificationFormDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<T>> SearchIntermediateCertificationForms(string value)
    {
        var filter = new SearchIntermediateCertificationFormsRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => IntermediateCertificationFormsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfIntermediateCertificationFormDto response)
        {
            _intermediateCertificationForms = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _intermediateCertificationForms.Select(x => x.Id.As<T>());
    }

    private string GetIntermediateCertificationFormName(T id)
    {
        if (id is null)
            return string.Empty;

        var result = _intermediateCertificationForms.Find(b => b.Id == Convert.ToInt32(id));
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}