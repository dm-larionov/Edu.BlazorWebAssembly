using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class EducationFormAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<EducationFormAutocomplete> L { get; set; } = default!;
    [Inject]
    private IEducationFormsClient EducationFormsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<EducationFormDto> _educationForms = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["EducationForm"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchEducationForms;
        ToStringFunc = GetEducationFormName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one educationForm to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationFormsClient.GetAsync(_value), Snackbar) is { } educationForm)
        {
            _educationForms.Add(educationForm.Adapt<EducationFormDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchEducationForms(string value)
    {
        var filter = new SearchEducationFormsRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationFormsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfEducationFormDto response)
        {
            _educationForms = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _educationForms.Select(x => x.Id);
    }

    private string GetEducationFormName(int id)
    {
        var result = _educationForms.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}