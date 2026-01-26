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
            Console.WriteLine("Authorization header set"); // Debug
        }
        else
        {
            Console.WriteLine("No auth token found"); // Debug
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        SetAuthorizationHeader();

        Console.WriteLine($"GET: {Httpclient.BaseAddress}{endpoint}"); // Debug

        var response = await Httpclient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"GET failed: {response.StatusCode}"); // Debug
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
}












/*
namespace ApartmentManagementSystem.Web.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class ApiClient
{
    private readonly HttpClient Httpclient;
    private readonly IHttpContextAccessor HttpContextAccesor;       //API calls would be unauthenticated;;[Authorize] would fail;;Roles like SuperAdmin wouldn’t work
    private readonly JsonSerializerOptions JsonOptions;

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        Httpclient = httpClient;
        HttpContextAccesor = httpContextAccessor;
        JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private void SetAuthorizationHeader()
    {
        var token = HttpContextAccesor.HttpContext?.Request.Cookies["AuthToken"];

        Httpclient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrWhiteSpace(token))
        {
            Httpclient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
    // GET
       public async Task<T?> GetAsync<T>(string endpoint)
    {
        SetAuthorizationHeader();

        var response = await Httpclient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, JsonOptions);
    }
  
    // POST 
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        SetAuthorizationHeader();

        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Httpclient.PostAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"POST {endpoint} returned: {responseContent}");

        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseContent, JsonOptions);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to deserialize API response: {ex.Message}\nResponse Content: {responseContent}");
        }
    }
/*
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        SetAuthorizationHeader();

        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);

        /*  if (!response.IsSuccessStatusCode)
          {
              var error = await response.Content.ReadAsStringAsync();
              throw new HttpRequestException(
                  $"API Error ({response.StatusCode}): {error}");
          -----
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(
            responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result!;


       // var responseContent = await response.Content.ReadAsStringAsync();
       // return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
    }-----------

    
    // POST (Raw response)
    public async Task<HttpResponseMessage> PostAsyncRaw<TRequest>(string endpoint, TRequest data)
    {
        SetAuthorizationHeader();

        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return await Httpclient.PostAsync(endpoint, content);
    }
}
    

*/












