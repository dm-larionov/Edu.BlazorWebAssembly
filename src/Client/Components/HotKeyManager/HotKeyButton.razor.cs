using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Edu.BlazorWebAssembly.Client.Components.HotKeyManager;

public partial class HotKeyButton
{
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }
}