using Edu.BlazorWebAssembly.Client.Infrastructure.Theme;

namespace Edu.BlazorWebAssembly.Client.Infrastructure.Preferences;

public class ClientPreference : IPreference
{
    public bool IsDarkMode { get; set; } = true;
    public bool IsRTL { get; set; }
    public bool IsDrawerOpen { get; set; }
    public string PrimaryColor { get; set; } = CustomColors.Dark.Primary;
    public string SecondaryColor { get; set; } = CustomColors.Dark.Secondary;
    public double BorderRadius { get; set; } = 5;
    public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "ru-RU";
    public EduTablePreference TablePreference { get; set; } = new EduTablePreference();
}
