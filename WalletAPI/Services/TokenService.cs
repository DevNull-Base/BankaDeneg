using System.Text;
using Newtonsoft.Json;

namespace WalletAPI.Services;

public interface ITokenService
{
    Task<string> GetTokenAsync();
    Task RefreshTokenAsync();
}

public class TokenService : ITokenService
{
    private string _accessToken;
    private DateTime _tokenExpiration;

    public async Task<string> GetTokenAsync()
    {
        if (_accessToken == null || DateTime.UtcNow >= _tokenExpiration)
        {
            await RefreshTokenAsync();
        }

        return _accessToken;
    }

    public async Task RefreshTokenAsync()
    {
        _accessToken = await GetNewTokenFromVtbApiAsync();
        _tokenExpiration = DateTime.UtcNow.AddSeconds(1700);
    }

    private async Task<string> GetNewTokenFromVtbApiAsync()
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://auth.bankingapi.ru/auth/realms/kubernetes/protocol/openid-connect/token");
        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("grant_type", "client_credentials"));
        collection.Add(new("client_id", "team030"));
        collection.Add(new("client_secret", "OjHjydIZCGuURBloGP00BIfr6jBdAAfY"));
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var tokenResponse =  JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
        return tokenResponse.access_token;
    }
}