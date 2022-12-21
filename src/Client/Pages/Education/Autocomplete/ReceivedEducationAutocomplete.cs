using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class ReceivedEducationAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<ReceivedEducationAutocomplete> L { get; set; } = default!;
    [Inject]
    private IReceivedEducationsClient ReceivedEducationsClient { get; set; } = default!;
    [Inject]
    private IReceivedSpecialtiesClient ReceivedSpecialtiesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<ReceivedEducationDto> _receivedEducations = new();
    private List<ReceivedSpecialtyDto> _receivedSpecialties = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["ReceivedEducation"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchReceivedEducations;
        ToStringFunc = GetReceivedEducationName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one receivedEducation to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => ReceivedEducationsClient.GetAsync(_value), Snackbar) is { } receivedEducation)
        {
            _receivedEducations.Add(receivedEducation.Adapt<ReceivedEducationDto>());
            ForceRender(true);
        }

        _receivedSpecialties =
           (await ReceivedSpecialtiesClient.GetAllByIdRangeAsync(_receivedEducations.Select(x => x.ReceivedSpecialtyId)
                .Distinct())).Adapt<List<ReceivedSpecialtyDto>>();
    }

    private async Task<IEnumerable<int>> SearchReceivedEducations(string value)
    {
        var filter = new SearchReceivedEducationsRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => ReceivedEducationsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfReceivedEducationDto response)
        {
            _receivedEducations = response.Data.OrderBy(x => x.ReceivedSpecialtyId).ToList();
        }

        return _receivedEducations.Select(x => x.Id);
    }

    private string GetSpecialtyById(int specialtyId)
    {
        return _receivedSpecialties.Find(x => x.Id == specialtyId)?.Qualification ?? string.Empty;
    }

    private string GetReceivedEducationName(int id)
    {
        var result = _receivedEducations.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{GetSpecialtyById(result.ReceivedSpecialtyId)} {result.IsBudget} {result.StudyPeriodMonths}";
    }
}