using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;



namespace ApartmentManagementSystem.Web.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private void SetAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

   
    // GET
   
    public async Task<T?> GetAsync<T>(string endpoint)
    {
        SetAuthorizationHeader();

        var response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }

    
    // POST 

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        SetAuthorizationHeader();

        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"POST {endpoint} returned: {responseContent}");

        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
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
    }*/

    
    // POST (Raw response)
    public async Task<HttpResponseMessage> PostAsyncRaw<TRequest>(string endpoint, TRequest data)
    {
        SetAuthorizationHeader();

        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return await _httpClient.PostAsync(endpoint, content);
    }
}
    















/*


public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private void SetAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        SetAuthorizationHeader();
        var response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }

  /*  public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        SetAuthorizationHeader();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API Error: {response.StatusCode} - {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
    }----
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        SetAuthorizationHeader();

        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"POST {endpoint} returned: {responseContent}");

        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
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

        return await _httpClient.PostAsync(endpoint, content);
    }
}
*/