using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class StudentGroupAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<StudentGroupAutocomplete> L { get; set; } = default!;
    [Inject]
    private IStudentGroupsClient StudentGroupsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<StudentGroupDto> _studentGroups = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["StudentGroup"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchStudentGroups;
        ToStringFunc = GetStudentGroupName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one studentGroup to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => StudentGroupsClient.GetAsync(_value), Snackbar) is { } studentGroup)
        {
            _studentGroups.Add(studentGroup.Adapt<StudentGroupDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchStudentGroups(string value)
    {
        var filter = new SearchStudentGroupsRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => StudentGroupsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfStudentGroupDto response)
        {
            _studentGroups = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _studentGroups.Select(x => x.Id);
    }

    private string GetStudentGroupName(int id)
    {
        var result = _studentGroups.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}