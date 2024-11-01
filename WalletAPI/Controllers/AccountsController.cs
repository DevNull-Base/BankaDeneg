using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedModels;
using WalletAPI.Services;

namespace WalletAPI.Controllers;

public class AccountsController : ControllerBase
{
    private readonly IOpenApiService _openApiService;

    public AccountsController(IOpenApiService openApiService)
    {
        _openApiService = openApiService;
    }
    
    [HttpGet("v1/balance")]
    public async Task<string> Balance()
    {
        var r = await _openApiService.GetBalanceAsync("11139");
        return r.Amount + " " + r.Currency;
    }
    
    [HttpGet("v1/accounts")]
    public async Task<string> Accounts()
    {
        var a = await _openApiService.GetAccountsAsync();
        string tmp = "";

        foreach (var i in a)
        {
            tmp += JsonConvert.SerializeObject(a, Formatting.Indented);
        }

        return tmp;
    }
    
    [HttpGet("v1/transactions")]
    public async Task<string> Transactions()
    {
        var a = await _openApiService.GetTransactionsAsync();
        string tmp = "";

        foreach (var i in a)
        {
            tmp += JsonConvert.SerializeObject(a, Formatting.Indented);
        }

        return tmp;
    }
}