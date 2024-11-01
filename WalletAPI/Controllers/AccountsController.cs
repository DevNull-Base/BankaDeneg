using Microsoft.AspNetCore.Mvc;
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
    public async Task<string> Index()
    {
        return await _openApiService.GetBalanceAsync("11139");
    }
}