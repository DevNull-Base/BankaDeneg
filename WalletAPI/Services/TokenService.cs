using System.Text;
using Newtonsoft.Json;
using WalletAPI.Models;

namespace WalletAPI.Services;

public interface ITokenService
{
    Task<string> GetNewTokenFromVtbApiAsync(string login, string password);
}

public class TokenService : ITokenService
{
    public async Task<string> GetNewTokenFromVtbApiAsync(string login, string password)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://auth.bankingapi.ru/auth/realms/kubernetes/protocol/openid-connect/token");
        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("grant_type", "client_credentials"));
        collection.Add(new("client_id", login));
        collection.Add(new("client_secret", password));
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var tokenResponse =  JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
        return tokenResponse.access_token;
    }
}