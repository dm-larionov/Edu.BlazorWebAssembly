using System.Globalization;
using Edu.BlazorWebAssembly.Client;
using Edu.BlazorWebAssembly.Client.Infrastructure;
using Edu.BlazorWebAssembly.Client.Infrastructure.Common;
using Edu.BlazorWebAssembly.Client.Infrastructure.Preferences;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHotKeys();
builder.Services.AddClientServices(builder.Configuration);
builder.Services.AddMudMarkdownServices();

var host = builder.Build();

var storageService = host.Services.GetRequiredService<IClientPreferenceManager>();
if (storageService != null)
{
    CultureInfo culture;
    if (await storageService.GetPreference() is ClientPreference preference)
        culture = new CultureInfo(preference.LanguageCode);
    else
        culture = new CultureInfo(LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US");
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

await host.RunAsync();