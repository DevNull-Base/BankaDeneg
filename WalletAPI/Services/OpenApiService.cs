namespace WalletAPI.Services;

public interface IOpenApiService
{
    Task<string> GetBalanceAsync(string accountId);
}

public class OpenApiService : IOpenApiService
{
    private readonly HttpClient _httpClient;

    public OpenApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.bankingapi.ru/extapi/aft/");
    }

    public async Task<string> GetBalanceAsync(string accountId)
    {
        var token = "23";
        
        var request = new HttpRequestMessage(HttpMethod.Get, $"clientInfo/hackathon/v1/accounts/{accountId}/balance");
        request.Headers.Add("x-fapi-auth-date", "<string>");
        request.Headers.Add("x-fapi-customer-ip-address", "<string>");
        request.Headers.Add("x-fapi-interaction-id", "<string>");
        request.Headers.Add("x-customer-user-agent", "<string>");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Authorization", "Bearer " + token);
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        return response.Content.ReadAsStringAsync().Result;
    }
}