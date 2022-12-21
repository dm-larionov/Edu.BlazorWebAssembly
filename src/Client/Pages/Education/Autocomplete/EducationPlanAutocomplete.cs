using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Extensions;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class EducationPlanAutocomplete<T> : MudAutocomplete<T>
{
    [Inject]
    private IStringLocalizer<EducationPlanAutocomplete<T>> L { get; set; } = default!;
    [Inject]
    private IEducationPlansClient EducationPlansClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<EducationPlanDto> _educationPlans = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["EducationPlan"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchEducationPlans;
        ToStringFunc = GetEducationPlanName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one educationPlan to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value is not null &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationPlansClient.GetAsync(_value.As<int>()), Snackbar) is { } educationPlan)
        {
            _educationPlans.Add(educationPlan.Adapt<EducationPlanDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<T>> SearchEducationPlans(string value)
    {
        var filter = new SearchEducationPlansRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => EducationPlansClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfEducationPlanDto response)
        {
            _educationPlans = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _educationPlans.Select(x => x.Id.As<T>());
    }

    private string GetEducationPlanName(T id)
    {
        var result = _educationPlans.Find(b => b.Id == Convert.ToInt32(id));
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}