using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class ReceivedEducationFormAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<ReceivedEducationFormAutocomplete> L { get; set; } = default!;
    [Inject]
    private IReceivedEducationFormsClient ReceivedEducationFormsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<ReceivedEducationFormDto> _receivedEducationForms = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["ReceivedEducationForm"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchReceivedEducationForms;
        ToStringFunc = GetReceivedEducationFormName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one receivedEducationForm to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => ReceivedEducationFormsClient.GetAsync(_value), Snackbar) is { } receivedEducationForm)
        {
            _receivedEducationForms.Add(receivedEducationForm.Adapt<ReceivedEducationFormDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchReceivedEducationForms(string value)
    {
        var filter = new SearchReceivedEducationFormsRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => ReceivedEducationFormsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfReceivedEducationFormDto response)
        {
            _receivedEducationForms = response.Data.OrderBy(x => x.EducationFormId).ToList();
        }

        return _receivedEducationForms.Select(x => x.Id);
    }

    private string GetReceivedEducationFormName(int id)
    {
        var result = _receivedEducationForms.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Id}";
    }
}