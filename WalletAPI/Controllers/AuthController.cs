using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedModels;
using WalletAPI.Factories;
using WalletAPI.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WalletAPI.Controllers;

public class AuthController : ControllerBase
{
    private readonly UserAccountCredentialsFactory _userACFactory;
    private readonly IConfiguration _configuration;
    private readonly IUserAccountService _userAccountService;

    public AuthController(IConfiguration configuration, UserAccountCredentialsFactory userAccountCredentialsFactory,
        IUserAccountService userAccountService)
    {
        _configuration = configuration;
        _userACFactory = userAccountCredentialsFactory;
        _userAccountService = userAccountService;
    }


    //TODO: Test method
    [HttpPost("v1/auth/login")]
    [Authorize]
    public async Task<IActionResult> Login()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return NotFound(new { message = "User not found." });
        var user = _userAccountService.GetUserById(userId);
        
        return Ok(new {VTBtoken = await user.GetAccessTokenAsync()});
    }

    [HttpPost("v1/auth/signup")]
    public async Task<IActionResult> SignUp([FromBody] AuthRequest dataRequest)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post,
            "https://auth.bankingapi.ru/auth/realms/kubernetes/protocol/openid-connect/token");
        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("grant_type", "client_credentials"));
        collection.Add(new("client_id", dataRequest.Login));
        collection.Add(new("client_secret", dataRequest.Password));
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await client.SendAsync(request);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Unauthorized();
        }

        var userId = Guid.NewGuid().ToString();
        var userAccount = _userACFactory.Create(userId, dataRequest.Login, dataRequest.Password);
        _userAccountService.AddUser(userAccount);
        
        return Ok(new {Token = GenerateJwtToken(userAccount.Id), RefreshToken = GenerateJwtRefreshToken(userAccount.Id) });
    }

    private string GenerateJwtToken(string userId)
    {
        var claims = new[]
        {
            new Claim("UserId", userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var secretKey = _configuration["SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.Now.AddMinutes(3);

        var token = new JwtSecurityToken(
            issuer: _configuration["ISSUER"],
            audience: _configuration["AUDIENCE"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private string GenerateJwtRefreshToken(string userId)
    {
        var claims = new[]
        {
            new Claim("UserId", userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var secretKey = _configuration["SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.Now.AddMonths(1);

        var token = new JwtSecurityToken(
            issuer: _configuration["ISSUER"],
            audience: _configuration["AUDIENCE"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}