using Newtonsoft.Json;
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

        var data = JsonConvert.DeserializeObject<dynamic>(content);

        var result = new Balance
        {
            Amount = data.Data.Balance.Amount.amount,
            Currency = data.Data.Balance.Amount.currency,
        };
        
        return result;
    }


    public async Task<List<Account>> GetAccountsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"clientInfo/hackathon/v1/accounts");
        request.Headers.Add("x-fapi-auth-date", "<string>");
        request.Headers.Add("x-fapi-customer-ip-address", "<string>");
        request.Headers.Add("x-fapi-interaction-id", "<string>");
        request.Headers.Add("x-customer-user-agent", "<string>");
        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();

        var data = JsonConvert.DeserializeObject<dynamic>(content);
        List<Account> result = new List<Account>();

        foreach (var account in data.Data.Account)
        {
            var tmp = new Account
            {
                Id = account.accountId,
                Status = Enum.Parse<AccountStatus>(account.status.ToString()),
                Currency = account.currency,
                Type = Enum.Parse<AccountType>(account.accountType.ToString()),
                SubType = Enum.Parse<AccountSubType>(account.accountSubType.ToString()),
                Description = account.accountDescription,
                SchemeName = EnumExtensions.ParseEnum<AccountSchemeName>(account.AccountDetails[0].schemeName.ToString()),
                Identification = account.AccountDetails[0].identification,
                Name = account.AccountDetails[0].name,
                Owner = account.Owner.name
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

        var data = JsonConvert.DeserializeObject<dynamic>(content);
        List<Transaction> result = new List<Transaction>();

        foreach (var t in data.Data.Transaction)
        {
            var tmp = new Transaction
            {
                AccountId = t.accountId,
                Id = t.transactionId,
                Reference = t.transactionReference,
                Type = Enum.Parse<TransactionType>(t.creditDebitIndicator.ToString()),
                Status = Enum.Parse<TransactionStatus>(t.status.ToString()),
                Amount = t.Amount.amount,
                Currency =t.Amount.currency,

            };
            result.Add(tmp);
        }

        return result;
        
    }
}