using Microsoft.JSInterop;

namespace StudentOs.Blazor.Services;

public class ThemeService
{
    private readonly IJSRuntime _js;

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<string> GetThemeAsync()
    {
        return await _js.InvokeAsync<string>("appTheme.get");
    }

    public async Task<string> ToggleThemeAsync()
    {
        return await _js.InvokeAsync<string>("appTheme.toggle");
    }

    public async Task SetThemeAsync(string theme)
    {
        await _js.InvokeVoidAsync("appTheme.set", theme);
    }
}