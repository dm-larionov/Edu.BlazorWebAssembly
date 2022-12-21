using Edu.BlazorWebAssembly.Client.Infrastructure.Preferences;
using Edu.BlazorWebAssembly.Client.Infrastructure.Theme;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.HotKeys;

namespace Edu.BlazorWebAssembly.Client.Components.ThemeManager;

public partial class ThemeDrawer : IDisposable
{
    [Parameter]
    public bool ThemeDrawerOpen { get; set; }

    [Parameter]
    public EventCallback<bool> ThemeDrawerOpenChanged { get; set; }

    [EditorRequired]
    [Parameter]
    public ClientPreference ThemePreference { get; set; } = default!;

    [EditorRequired]
    [Parameter]
    public EventCallback<ClientPreference> ThemePreferenceChanged { get; set; }

    [Inject]
    private HotKeys HotKeys { get; set; }

    private HotKeysContext HotKeysContext { get; set; }

    private readonly List<string> _colors = CustomColors.ThemeColors;

    protected override async Task OnInitializedAsync()
    {
        HotKeysContext = HotKeys.CreateContext()
            .Add(ModKeys.None, Keys.N, async _ => await ToggleDarkLightMode(!ThemePreference.IsDarkMode), "do foo bar.");
    }

    private async Task UpdateThemePrimaryColor(string color)
    {
        if (ThemePreference is not null)
        {
            ThemePreference.PrimaryColor = color;
            await ThemePreferenceChanged.InvokeAsync(ThemePreference);
        }
    }

    private async Task UpdateThemeSecondaryColor(string color)
    {
        if (ThemePreference is not null)
        {
            ThemePreference.SecondaryColor = color;
            await ThemePreferenceChanged.InvokeAsync(ThemePreference);
        }
    }

    private async Task UpdateBorderRadius(double radius)
    {
        if (ThemePreference is not null)
        {
            ThemePreference.BorderRadius = radius;
            await ThemePreferenceChanged.InvokeAsync(ThemePreference);
        }
    }

    private async Task ToggleDarkLightMode(bool isDarkMode)
    {
        if (ThemePreference is not null)
        {
            ThemePreference.IsDarkMode = isDarkMode;
            await ThemePreferenceChanged.InvokeAsync(ThemePreference);
        }
    }

    private async Task ToggleEntityTableDense(bool isDense)
    {
        if (ThemePreference is not null)
        {
            ThemePreference.TablePreference.IsDense = isDense;
            await ThemePreferenceChanged.InvokeAsync(ThemePreference);
        }
    }

    private async Task ToggleEntityTableStriped(bool isStriped)
    {
        if (ThemePreference is not null)
        {
            ThemePreference.TablePreference.IsStriped = isStriped;
            await ThemePreferenceChanged.InvokeAsync(ThemePreference);
        }
    }

    private async Task ToggleEntityTableBorder(bool hasBorder)
    {
        if (ThemePreference is not null)
        {
            ThemePreference.TablePreference.HasBorder = hasBorder;
            await ThemePreferenceChanged.InvokeAsync(ThemePreference);
        }
    }

    private async Task ToggleEntityTableHoverable(bool isHoverable)
    {
        if (ThemePreference is not null)
        {
            ThemePreference.TablePreference.IsHoverable = isHoverable;
            await ThemePreferenceChanged.InvokeAsync(ThemePreference);
        }
    }

    public void Dispose()
    {
        HotKeysContext.Dispose();
    }
}