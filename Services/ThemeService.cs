using Microsoft.JSInterop;

namespace StudentOs.Blazor.Services;

public class ThemeService
{
    private readonly IJSRuntime _js;

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    // Načte aktuálně uložený režim vzhledu
    public async Task<string> GetThemeAsync()
    {
        return await _js.InvokeAsync<string>("appTheme.get");
    }

    // Přepne aktuální režim vzhledu na opačný
    public async Task<string> ToggleThemeAsync()
    {
        return await _js.InvokeAsync<string>("appTheme.toggle");
    }

    // Nastaví konkrétní režim vzhledu
    public async Task SetThemeAsync(string theme)
    {
        await _js.InvokeVoidAsync("appTheme.set", theme);
    }
}

