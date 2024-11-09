using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedModels;
using WalletAPI.Extensions;
using WalletAPI.Models;

namespace WalletAPI.Services;

public interface IOpenApiService
{
    Task<Balance> GetBalanceAsync(UserCredentials user, string accountId);
    Task<List<Account>> GetAccountsAsync(UserCredentials user);
    Task<List<Transaction>> GetTransactionsAsync(UserCredentials user);
}

public class OpenApiService : IOpenApiService
{
    private readonly HttpClient _httpClient;
    private IOpenApiService _openApiServiceImplementation;

    public OpenApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.bankingapi.ru/extapi/aft/");
    }

    public async Task<Balance> GetBalanceAsync(UserCredentials user, string accountId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"clientInfo/hackathon/v1/accounts/{accountId}/balance");
        request.Headers.Add("x-fapi-auth-date", "<string>");
        request.Headers.Add("x-fapi-customer-ip-address", "<string>");
        request.Headers.Add("x-fapi-interaction-id", "<string>");
        request.Headers.Add("x-customer-user-agent", "<string>");
        request.Headers.Add("Accept", "application/json");

        request.Headers.Add("Authorization", "Bearer " + await user.GetAccessTokenAsync());

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


    public async Task<List<Account>> GetAccountsAsync(UserCredentials user)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"clientInfo/hackathon/v1/accounts?page=0");
        request.Headers.Add("x-fapi-auth-date", "<string>");
        request.Headers.Add("x-fapi-customer-ip-address", "<string>");
        request.Headers.Add("x-fapi-interaction-id", "<string>");
        request.Headers.Add("x-customer-user-agent", "<string>");
        request.Headers.Add("Accept", "application/json");

        request.Headers.Add("Authorization", "Bearer " + await user.GetAccessTokenAsync());

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
                Currency = account.currency,
                Type = Enum.Parse<AccountType>(account.accountType.ToString()),
                SubType = Enum.Parse<AccountSubType>(account.accountSubType.ToString()),
                Name = account.Owner.name,
                ServiceProviderSchemeName = EnumExtensions.ParseEnum<AccountSchemeName>(account.ServiceProvider.schemeName.ToString()),
                ServiceProviderIdentification = account.ServiceProvider.identification.ToString()
            };

            foreach (var detail in account.AccountDetails)
            {
                tmp.SchemeName = EnumExtensions.ParseEnum<AccountSchemeName>(detail.schemeName.ToString());
                tmp.Identification = detail.identification;
            }
            
            result.Add(tmp);
        }

        return result;
    }

    public async Task<List<Transaction>> GetTransactionsAsync(UserCredentials user)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"clientInfo/hackathon/v1/accounts/11139/transaction?page=0&fromBookingDateTime={DateTime.MinValue}&toBookingDateTime={DateTime.Now}");
        request.Headers.Add("x-fapi-auth-date", "<string>");
        request.Headers.Add("x-fapi-customer-ip-address", "<string>");
        request.Headers.Add("x-fapi-interaction-id", "<string>");
        request.Headers.Add("x-customer-user-agent", "<string>");
        request.Headers.Add("Accept", "application/json");

        request.Headers.Add("Authorization", "Bearer " + await user.GetAccessTokenAsync());

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
                //Reference = t.TransactionReference,
                Type = Enum.Parse<TransactionType>(t.creditDebitIndicator.ToString()),
                //Status = Enum.Parse<TransactionStatus>(t.Status.ToString()),
                Amount = t.Amount.amount,
                Currency = t.Amount.currency,
            };
            result.Add(tmp);
        }

        return result;
    }
}