using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Extensions;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class EducationModuleAutocomplete<T> : MudAutocomplete<T>
{
    [Inject]
    private IStringLocalizer<EducationModuleAutocomplete<T>> L { get; set; } = default!;
    [Inject]
    private IEducationModulesClient EducationModulesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<EducationModuleDto> _educationModules = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["EducationModule"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchEducationModules;
        ToStringFunc = GetEducationModuleName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one educationModule to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value is not null &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationModulesClient.GetAsync(_value.As<int>()), Snackbar) is { } educationModule)
        {
            _educationModules.Add(educationModule.Adapt<EducationModuleDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<T>> SearchEducationModules(string value)
    {
        var filter = new SearchEducationModulesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationModulesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfEducationModuleDto response)
        {
            _educationModules = response.Data.OrderBy(x => x.EducationModuleIndex).ToList();
        }

        return _educationModules.Select(x => x.Id.As<T>());
    }

    private string GetEducationModuleName(T id)
    {
        var result = _educationModules.Find(b => b.Id == id.As<int>());
        if (result is null)
            return string.Empty;
        return $"{result.EducationModuleIndex} - {result.Name}";
    }
}