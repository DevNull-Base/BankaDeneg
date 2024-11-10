using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedModels;
using WalletAPI.Services;

namespace WalletAPI.Controllers;

public class APIController : ControllerBase
{
    private readonly IOpenApiService _openApiService;
    private readonly IUserAccountService _userAccountService;

    public APIController(IOpenApiService openApiService, IUserAccountService userAccountService)
    {
        _openApiService = openApiService;
        _userAccountService = userAccountService;
    }
    
    /*[HttpGet("v1/balance")]
    [Authorize]
    public async Task<IActionResult> Balance(string id)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return NotFound(new { message = "User not found." });
        var user = _userAccountService.GetUserById(userId);
        
        var r = await _openApiService.GetBalanceAsync(user,id);
        return Ok(new
        {
            Balance = new
            {
                Amount = r.Amount,
                Curreny = r.Currency,
            }
        });
    }*/
    
    [HttpGet("v1/accounts")]
    [Authorize]
    public async Task<IActionResult> Accounts()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return NotFound(new { message = "User not found." });
        var user = _userAccountService.GetUserById(userId);
        
        var accounts = await _openApiService.GetAccountsAsync(user);

        List<AccountResponse> response = new List<AccountResponse>();
        
        List<String> bankName = new List<string>{"ВТБ","СБЕР БАНК","Т БАНК"};

        for (var i = 0; i < accounts.Count; i++)
        {
            var ac = accounts[i];
            var balance = await _openApiService.GetBalanceAsync(user, ac.Id);

            var tmp = new AccountResponse
            {
                AccountId = ac.Id,
                Amount = balance.Amount,
                Currency = balance.Currency,
                BankName = bankName[i]
            };
            
            response.Add(tmp);
        }

        return Ok(response);
    }
    
    [HttpGet("v1/transactions")]
    [Authorize]
    public async Task<IActionResult> Transactions()
    {       
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return NotFound(new { message = "User not found." });
        var user = _userAccountService.GetUserById(userId);
        
        //TODO: изменить в случае если Transaction будет выдавать больше данных 
        var accounts = await _openApiService.GetAccountsAsync(user);
        
        List<TransactionResponse> response = new List<TransactionResponse>();
        List<String> bankName = new List<string>{"ВТБ","СБЕР БАНК","Т БАНК"};

        for (var i = 0; i < accounts.Count; i++)
        {
            var ac = accounts[i];
            var transactions = await _openApiService.GetTransactionsAsync(user, ac.Id);

            var tmp = new TransactionResponse
            {
                Amount = transactions[0].Amount,
                Currency = transactions[0].Currency,
                BankName = bankName[i],
                Type = "Покупка",
                Title = "null"
            };
            
            response.Add(tmp);
        }

        return Ok(response);
    }
}