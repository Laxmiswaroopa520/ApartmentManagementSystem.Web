// Web/Services/ApiClient.cs

namespace ApartmentManagementSystem.Web.Services;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class ApiClient
{
    private readonly HttpClient Httpclient;
    private readonly IHttpContextAccessor HttpContextAccesor;
    private readonly JsonSerializerOptions JsonOptions;

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        Httpclient = httpClient;
        HttpContextAccesor = httpContextAccessor;
        JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // ⭐ Debug: Log the base URL when ApiClient is created
        Console.WriteLine($"ApiClient created with BaseAddress: {Httpclient.BaseAddress}");
    }

    private void SetAuthorizationHeader()
    {
        var token = HttpContextAccesor.HttpContext?.Request.Cookies["AuthToken"];
        Httpclient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrWhiteSpace(token))
        {
            Httpclient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine("Authorization header set"); 
        }
        else
        {
            Console.WriteLine("No auth token found");
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        SetAuthorizationHeader();

        Console.WriteLine($"GET: {Httpclient.BaseAddress}{endpoint}"); 

        var response = await Httpclient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"GET failed: {response.StatusCode}"); 
            return default;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, JsonOptions);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        SetAuthorizationHeader();

        var json = JsonSerializer.Serialize(data);
        Console.WriteLine($"POST: {Httpclient.BaseAddress}{endpoint}");
        Console.WriteLine($"POST Body: {json}");

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await Httpclient.PostAsync(endpoint, content);

        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"POST Response ({response.StatusCode}): {responseContent}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"API returned {response.StatusCode}: {responseContent}");
        }

        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseContent, JsonOptions);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to deserialize API response: {ex.Message}\nResponse Content: {responseContent}");
        }
    }

    public async Task<HttpResponseMessage> PostAsyncRaw<TRequest>(string endpoint, TRequest data)
    {
        SetAuthorizationHeader();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await Httpclient.PostAsync(endpoint, content);
    }

    // Add this method to your existing ApiClient class if it doesn't already have DeleteAsync.
    public async Task<TResponse?> DeleteAsync<TResponse>(string endpoint)
    {
        try
        {
            SetAuthorizationHeader();
            Console.WriteLine($"DELETE: {Httpclient.BaseAddress}{endpoint}");
            var response = await Httpclient.DeleteAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"DELETE Response ({response.StatusCode}): {content}");
            return JsonSerializer.Deserialize<TResponse>(content, JsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DELETE {endpoint} failed: {ex.Message}");
            return default;
        }
    }

}












