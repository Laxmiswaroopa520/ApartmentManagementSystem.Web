namespace ApartmentManagementSystem.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient Httpclient;

        public ApiClient(HttpClient client)
        {
            Httpclient = client;
        }

        public async Task<T> PostAsync<T>(string url, object data)
        {
            var response = await Httpclient.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}