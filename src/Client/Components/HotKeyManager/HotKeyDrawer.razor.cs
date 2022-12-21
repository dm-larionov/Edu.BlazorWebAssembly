using Edu.BlazorWebAssembly.Client.Infrastructure.Preferences;
using Edu.BlazorWebAssembly.Client.Infrastructure.Theme;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.HotKeys;

namespace Edu.BlazorWebAssembly.Client.Components.HotKeyManager;

public partial class HotKeyDrawer : IDisposable
{
    [Parameter]
    public bool HotKeyDrawerOpen { get; set; }

    [Parameter]
    public EventCallback<bool> HotKeyDrawerOpenChanged { get; set; }

    [Inject]
    private HotKeys HotKeys { get; set; }

    public HotKeysContext HotKeysContext { get; set; }


    private readonly List<string> _colors = CustomColors.ThemeColors;

    protected override async Task OnInitializedAsync()
    {
        HotKeysContext = HotKeys.CreateContext()
            .Add(ModKeys.None, Keys.K, async _ => await ToggleHotKeyDrawerOpen(!HotKeyDrawerOpen), "Показать/Скрыть горячие клавиши");
    }

    private async Task ToggleHotKeyDrawerOpen(bool isOpen)
    {
        HotKeyDrawerOpen = isOpen;
        await HotKeyDrawerOpenChanged.InvokeAsync(HotKeyDrawerOpen);
    }

    public void Dispose()
    {
        HotKeysContext.Dispose();
    }
}