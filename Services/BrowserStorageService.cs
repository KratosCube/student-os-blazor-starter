using System.Text.Json;
using Microsoft.JSInterop;

namespace StudentOs.Blazor.Services;

public class BrowserStorageService
{
    private readonly IJSRuntime _js;

    // Nastavení serializace pro ukládání objektů jako JSON.
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public BrowserStorageService(IJSRuntime js)
    {
        _js = js;
    }

    // Načte textovou hodnotu z localStorage podle klíče
    public async Task<string?> GetStringAsync(string key)
    {
        return await _js.InvokeAsync<string?>("appStorage.get", key);
    }

    // Uloží textovou hodnotu do localStorage pod zadaným klíčem
    public async Task SetStringAsync(string key, string value)
    {
        await _js.InvokeVoidAsync("appStorage.set", key, value);
    }

    // Odstraní hodnotu z localStorage podle klíče
    public async Task RemoveAsync(string key)
    {
        await _js.InvokeVoidAsync("appStorage.remove", key);
    }

    // Načte objekt uložený jako JSON
    public async Task<T?> GetObjectAsync<T>(string key)
    {
        var json = await GetStringAsync(key);

        if (string.IsNullOrWhiteSpace(json))
            return default;

        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    // Uloží C# objekt do localStorage jako JSON
    public async Task SetObjectAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        await SetStringAsync(key, json);
    }
}

