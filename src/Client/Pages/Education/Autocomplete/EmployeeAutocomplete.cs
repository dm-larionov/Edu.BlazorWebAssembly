using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class EmployeeAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<EmployeeAutocomplete> L { get; set; } = default!;
    [Inject]
    private IEmployeesClient EmployeesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<EmployeeDto> _employees = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Employee"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchEmployees;
        ToStringFunc = GetEmployeeName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one employee to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => EmployeesClient.GetAsync(_value), Snackbar) is { } employee)
        {
            _employees.Add(employee.Adapt<EmployeeDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchEmployees(string value)
    {
        var filter = new SearchEmployeesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => EmployeesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfEmployeeDto response)
        {
            _employees = response.Data.OrderBy(x => x.Lastname).ToList();
        }

        return _employees.Select(x => x.Id);
    }

    private string GetEmployeeName(int id)
    {
        var result = _employees.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Lastname} {result.Firstname} {result.Middlename}";
    }
}