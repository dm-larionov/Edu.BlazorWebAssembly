using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class StudentGroupNameChangeAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<StudentGroupNameChangeAutocomplete> L { get; set; } = default!;
    [Inject]
    private IStudentGroupNameChangesClient StudentGroupNameChangesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<StudentGroupNameChangeDto> _studentGroupNameChanges = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["StudentGroupNameChange"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchStudentGroupNameChanges;
        ToStringFunc = GetStudentGroupNameChangeName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one studentGroupNameChange to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => StudentGroupNameChangesClient.GetAsync(_value), Snackbar) is { } studentGroupNameChange)
        {
            _studentGroupNameChanges.Add(studentGroupNameChange.Adapt<StudentGroupNameChangeDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchStudentGroupNameChanges(string value)
    {
        var filter = new SearchStudentGroupNameChangesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => StudentGroupNameChangesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfStudentGroupNameChangeDto response)
        {
            _studentGroupNameChanges = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _studentGroupNameChanges.Select(x => x.Id);
    }

    private string GetStudentGroupNameChangeName(int id)
    {
        var result = _studentGroupNameChanges.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}