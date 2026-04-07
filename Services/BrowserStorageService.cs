using System.Text.Json;
using Microsoft.JSInterop;

namespace StudentOs.Blazor.Services;

public class BrowserStorageService
{
    private readonly IJSRuntime _js;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public BrowserStorageService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<string?> GetStringAsync(string key)
    {
        return await _js.InvokeAsync<string?>("appStorage.get", key);
    }

    public async Task SetStringAsync(string key, string value)
    {
        await _js.InvokeVoidAsync("appStorage.set", key, value);
    }

    public async Task RemoveAsync(string key)
    {
        await _js.InvokeVoidAsync("appStorage.remove", key);
    }

    public async Task<T?> GetObjectAsync<T>(string key)
    {
        var json = await GetStringAsync(key);

        if (string.IsNullOrWhiteSpace(json))
            return default;

        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task SetObjectAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        await SetStringAsync(key, json);
    }
}