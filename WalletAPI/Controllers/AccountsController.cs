using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedModels;
using WalletAPI.Services;

namespace WalletAPI.Controllers;

public class AccountsController : ControllerBase
{
    private readonly IOpenApiService _openApiService;
    private readonly IUserAccountService _userAccountService;

    public AccountsController(IOpenApiService openApiService, IUserAccountService userAccountService)
    {
        _openApiService = openApiService;
        _userAccountService = userAccountService;
    }
    
    [HttpGet("v1/balance")]
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
    }
    
    [HttpGet("v1/accounts")]
    [Authorize]
    public async Task<IActionResult> Accounts()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return NotFound(new { message = "User not found." });
        var user = _userAccountService.GetUserById(userId);
        
        var a = await _openApiService.GetAccountsAsync(user);
        
        string tmp = JsonConvert.SerializeObject(a, Formatting.Indented);
        return Ok(tmp);
    }
    
    [HttpGet("v1/transactions")]
    [Authorize]
    public async Task<IActionResult> Transactions()
    {       
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return NotFound(new { message = "User not found." });
        var user = _userAccountService.GetUserById(userId);
        
        var a = await _openApiService.GetTransactionsAsync(user);
        string tmp = "";

        foreach (var i in a)
        {
            tmp += JsonConvert.SerializeObject(a, Formatting.Indented);
        }

        return Ok(tmp);
    }
}