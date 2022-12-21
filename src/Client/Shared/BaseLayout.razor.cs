using Edu.BlazorWebAssembly.Client.Infrastructure.Preferences;
using Edu.BlazorWebAssembly.Client.Infrastructure.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Shared;

public partial class BaseLayout
{
    [Inject]
    IJSRuntime JSRuntime { get; set; }

    private ClientPreference? _themePreference;
    private MudTheme _currentTheme = new LightTheme();
    private bool _themeDrawerOpen;
    private bool _hotKeyDrawerOpen;
    private bool _rightToLeft;

    protected override async Task OnInitializedAsync()
    {
        _themePreference = await ClientPreferences.GetPreference() as ClientPreference;
        if (_themePreference == null) _themePreference = new ClientPreference();
        SetCurrentTheme(_themePreference);

        Snackbar.Add("Shot on \"Oka\"", Severity.Normal, config =>
        {
            config.BackgroundBlurred = true;
            config.Icon = Icons.Filled.Train;
            config.Action = "Кто не в курсе";
            config.ActionColor = Color.Primary;
            config.Onclick = async prop => {
                await JSRuntime.InvokeVoidAsync("eval", $"let _discard_ = open(`https://www.youtube.com/watch?v=fA7r_ZYkA3M`, `_blank`)");
            };
        });
    }

    private async Task ThemePreferenceChanged(ClientPreference themePreference)
    {
        SetCurrentTheme(themePreference);
        await ClientPreferences.SetPreference(themePreference);
    }

    private void SetCurrentTheme(ClientPreference themePreference)
    {
        _currentTheme = themePreference.IsDarkMode ? new DarkTheme() : new LightTheme();
        _currentTheme.Palette.Primary = themePreference.PrimaryColor;
        _currentTheme.Palette.Secondary = themePreference.SecondaryColor;
        _currentTheme.LayoutProperties.DefaultBorderRadius = $"{themePreference.BorderRadius}px";
        _currentTheme.LayoutProperties.DefaultBorderRadius = $"{themePreference.BorderRadius}px";
        _rightToLeft = themePreference.IsRTL;
    }
}