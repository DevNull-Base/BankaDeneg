using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace WalletAPI.Controllers;

public class MainPageController(IConfiguration configuration) : ControllerBase
{
    [HttpGet("v1/main/data")]
    public async Task<string> Validate()
    {
        string? login = configuration["Login"];
        string? password = configuration["Password"];
        
        var options = new RestClientOptions("https://auth.bankingapi.ru")
        {
            MaxTimeout = -1,
        };
        var client = new RestClient(options);
        var request = new RestRequest("/auth/realms/kubernetes/protocol/openid-connect/token", Method.Post);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddParameter("grant_type", "client_credentials");
        request.AddParameter("client_id", login);
        request.AddParameter("client_secret", password);
        RestResponse response = await client.ExecuteAsync(request);
        Console.WriteLine(response.Content);

        var r = JsonConvert.DeserializeObject<dynamic>(response.Content);
        string token = r.access_token;
        
        Console.WriteLine(token);
        
        var options2 = new RestClientOptions("https://hackaton.bankingapi.ru")
        {
            MaxTimeout = -1,
        };
        var client2 = new RestClient(options2);
        var request2 = new RestRequest("/extapi/aft/clientInfo/hackathon/v1/balances?page=0", Method.Get);
        request2.AddHeader("x-fapi-auth-date", "<string>");
        request2.AddHeader("x-fapi-customer-ip-address", "<string>");
        request2.AddHeader("x-fapi-interaction-id", "<string>");
        request2.AddHeader("x-customer-user-agent", "<string>");
        request2.AddHeader("Accept", "application/json");
        request2.AddHeader("Authorization", "Bearer " + token);
        RestResponse response2 = await client2.ExecuteAsync(request2);
        Console.WriteLine(response2.Content);
        
        var b = JsonConvert.DeserializeObject<dynamic>(response2.Content);

        string result = b.Data.Balance.Amount.amount + " " + b.Data.Balance.Amount.currency;
        
        return  result;
    }
}