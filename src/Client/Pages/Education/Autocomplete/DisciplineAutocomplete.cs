using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class DisciplineAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<DisciplineAutocomplete> L { get; set; } = default!;
    [Inject]
    private IDisciplinesClient DisciplinesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<DisciplineDto> _disciplines = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Discipline"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchDisciplines;
        ToStringFunc = GetDisciplineName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one discipline to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => DisciplinesClient.GetAsync(_value), Snackbar) is { } discipline)
        {
            _disciplines.Add(discipline.Adapt<DisciplineDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchDisciplines(string value)
    {
        var filter = new SearchDisciplinesRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => DisciplinesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfDisciplineDto response)
        {
            _disciplines = response.Data.OrderBy(x => x.DisciplineIndex).ToList();
        }

        return _disciplines.Select(x => x.Id);
    }

    private string GetDisciplineName(int id)
    {
        var result = _disciplines.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.DisciplineIndex} - {result.Name}";
    }
}