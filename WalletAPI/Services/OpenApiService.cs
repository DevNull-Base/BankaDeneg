using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedModels;
using WalletAPI.Extensions;

namespace WalletAPI.Services;

public interface IOpenApiService
{
    Task<Balance> GetBalanceAsync(string accountId);
    Task<List<Account>> GetAccountsAsync();
    Task<List<Transaction>> GetTransactionsAsync();
}

public class OpenApiService : IOpenApiService
{
    private readonly HttpClient _httpClient;

    public OpenApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.bankingapi.ru/extapi/aft/");
    }

    public async Task<Balance> GetBalanceAsync(string accountId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"clientInfo/hackathon/v1/accounts/{accountId}/balance");
        request.Headers.Add("x-fapi-auth-date", "<string>");
        request.Headers.Add("x-fapi-customer-ip-address", "<string>");
        request.Headers.Add("x-fapi-interaction-id", "<string>");
        request.Headers.Add("x-customer-user-agent", "<string>");
        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();

        var raw = JsonConvert.DeserializeObject<JObject>(content);
        dynamic data = raw.ConvertKeysToUpper();

        var result = new Balance
        {
            Amount = data.Data.Balance.Amount.Amount,
            Currency = data.Data.Balance.Amount.Currency,
        };
        
        return result;
    }


    public async Task<List<Account>> GetAccountsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"clientInfo/hackathon/v1/accounts?page=0");
        request.Headers.Add("x-fapi-auth-date", "<string>");
        request.Headers.Add("x-fapi-customer-ip-address", "<string>");
        request.Headers.Add("x-fapi-interaction-id", "<string>");
        request.Headers.Add("x-customer-user-agent", "<string>");
        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();

        var raw = JsonConvert.DeserializeObject<JObject>(content);
        dynamic data = raw.ConvertKeysToUpper();
        List<Account> result = new List<Account>();

        foreach (var account in data.Data.Account)
        {
            var tmp = new Account
            {
                Id = account.AccountId,
                Status = Enum.Parse<AccountStatus>(account.Status.ToString()),
                Currency = account.Currency,
                Type = Enum.Parse<AccountType>(account.AccountType.ToString()),
                SubType = Enum.Parse<AccountSubType>(account.AccountSubType.ToString()),
                SchemeName = EnumExtensions.ParseEnum<AccountSchemeName>(account.SchemeName.ToString()),
                Identification = account.Identification,
                Name = account.Name,
            };

            result.Add(tmp);
        }

        return result;
    }

    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"clientInfo/hackathon/v1/accounts/11139/transaction?page=0&fromBookingDateTime={DateTime.MinValue}&toBookingDateTime={DateTime.Now}");
        request.Headers.Add("x-fapi-auth-date", "<string>");
        request.Headers.Add("x-fapi-customer-ip-address", "<string>");
        request.Headers.Add("x-fapi-interaction-id", "<string>");
        request.Headers.Add("x-customer-user-agent", "<string>");
        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();

        var raw = JsonConvert.DeserializeObject<JObject>(content);
        dynamic data = raw.ConvertKeysToUpper();
        List<Transaction> result = new List<Transaction>();

        throw new Exception("Здесь вообще все сломано");
        
        foreach (var t in data.Data.Transaction)
        {
            var tmp = new Transaction
            {
                AccountId = t.AccountId,
                Id = t.TransactionId,
                //Reference = t.TransactionReference,
                Type = Enum.Parse<TransactionType>(t.CreditDebitIndicator.ToString()),
                //Status = Enum.Parse<TransactionStatus>(t.Status.ToString()),
                Amount = t.Amount.Amount,
                Currency =t.Amount.Currency,

            };
            result.Add(tmp);
        }

        return result;
        
    }
}