using System.Net.Http.Headers;
using Newtonsoft.Json;
using SharedModels;

namespace WalletApp.Services;

public interface IAPIService
{
    Task<List<AccountResponse>> GetAccountsAsync();
    Task<List<TransactionResponse>> GetTransactionsAsync();
}

public class APIService : IAPIService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;
#if WINDOWS
    private const string BaseUrl = "http://localhost:5041/v1";
#else 
    private const string BaseUrl = "https://api.devnullteam.ru/v1";
#endif

    public APIService(IAuthService authService)
    {
        _httpClient = new HttpClient();
        _authService = authService;
    }

    public async Task<List<AccountResponse>> GetAccountsAsync()
    {
        var url = $"{BaseUrl}/accounts";
        return await SendGetRequestAsync<List<AccountResponse>>(url);
    }

    public async Task<List<TransactionResponse>> GetTransactionsAsync()
    {
        var url = $"{BaseUrl}/transactions";
        return await SendGetRequestAsync<List<TransactionResponse>>(url);
    }

    private async Task<T> SendGetRequestAsync<T>(string url)
    {
        var authToken = await _authService.GetAuthTokenAsync();
        if (string.IsNullOrEmpty(authToken))
        {
            throw new Exception("Authentication token is missing.");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseJson);
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var refreshSuccess = await _authService.RefreshTokenAsync();
            if (refreshSuccess)
            {
                return await SendGetRequestAsync<T>(url);
            }
            else
            {
                throw new Exception("Failed to refresh token.");
            }
        }
        else
        {
            throw new Exception($"Failed to get data from API. Status code: {response.StatusCode}");
        }
    }
}