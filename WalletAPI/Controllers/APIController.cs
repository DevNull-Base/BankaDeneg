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
    
    /// <summary>
    /// Получить список счетов в которых авторизован пользователь
    /// </summary>
    /// <returns>Список счетов</returns>
    /// <response code="200">Возвращает список счетов</response>
    /// <response code="401">Если пользователь не авторизован</response>
    /// <response code="404">Если пользователь не найден</response>
    /// <response code="502">Если произошла ошибка при получении данных</response>
    [HttpGet("v1/accounts")]
    [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    [Authorize]
    public async Task<IActionResult> Accounts()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return NotFound(new { message = "User not found." });
        var user = _userAccountService.GetUserById(userId);

        List<AccountResponse> response = new List<AccountResponse>();
        
        try
        {
            var accounts = await _openApiService.GetAccountsAsync(user);
            
        
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status502BadGateway, "Произошла ошибка при получении данных");
        }

        return Ok(response);
    }
    
    
    /// <summary>
    /// Получить список транзакций по всем счетам
    /// </summary>
    /// <returns>Список транзакций</returns>
    /// <response code="200">Возвращает список транзакций</response>
    /// <response code="401">Если пользователь не авторизован</response>
    /// <response code="404">Если пользователь не найден</response>
    /// <response code="502">Если произошла ошибка при получении данных</response>
    [HttpGet("v1/transactions")]
    [ProducesResponseType(typeof(IEnumerable<TransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    [Authorize]
    public async Task<IActionResult> Transactions()
    {       
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return NotFound(new { message = "User not found." });
        var user = _userAccountService.GetUserById(userId);
        
        List<TransactionResponse> response = new List<TransactionResponse>();

        try
        {
            //TODO: изменить в случае если Transaction будет выдавать больше данных 
            var accounts = await _openApiService.GetAccountsAsync(user);
        
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status502BadGateway, "Произошла ошибка при получении данных");
        }

        return Ok(response);
    }

}