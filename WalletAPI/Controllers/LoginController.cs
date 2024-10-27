using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WalletAPI.Controllers;

public class LoginController(IConfiguration configuration) : ControllerBase
{
    
    [HttpPost("v1/auth/validate")]
    public IActionResult Validate()
    {
        if (false)
        {
            return Ok();
        }

        return NotFound();
    }

    private string GenerateJwtToken(string cardNumber)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, cardNumber),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var secretKey = configuration["SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.Now.AddMinutes(5);

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}