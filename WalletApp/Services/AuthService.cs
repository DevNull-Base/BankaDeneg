using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SharedModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WalletApp.Services;

public interface IAuthService
{
    Task<bool> AuthenticateAsync();
    Task<bool> RefreshTokenAsync();
    Task<string> GetAuthTokenAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<bool> LogoutAsync();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

#if WINDOWS
    private const string AuthEndpoint = "http://localhost:5041/v1/auth/signup";
    private const string RefreshEndpoint = "http://localhost:5041/v1/auth/refresh";
#else 
    private const string AuthEndpoint = "https://api.devnullteam.ru/v1/auth/signup";
    private const string RefreshEndpoint = "https://api.devnullteam.ru/v1/auth/refresh";
#endif
    
    public AuthService()
    {
        _httpClient = new HttpClient();
    }
    
    private class Credentials
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    
    private (string Login, string Password) ReadCredentialsFromJson()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "WalletApp.credentials.json";

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            string result = reader.ReadToEnd();
            var credentials = JsonSerializer.Deserialize<Credentials>(result);
            return (credentials.Login, credentials.Password);
        }
    }
    

    public async Task<bool> AuthenticateAsync()
    {
        var credentials = ReadCredentialsFromJson();
        var login = credentials.Login;
        var password = credentials.Password;
        
        var authData = new { login, password };
        var json = JsonConvert.SerializeObject(authData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(AuthEndpoint, content);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var tokens = JsonConvert.DeserializeObject<AuthResponse>(responseJson);

            await SecureStorage.SetAsync("AuthToken", tokens.Token);
            await SecureStorage.SetAsync("RefreshToken", tokens.RefreshToken);

            return true;
        }

        return false;
    }

    public async Task<bool> RefreshTokenAsync()
    {
        await AuthenticateAsync();
        return true;
        var refreshToken = await SecureStorage.GetAsync("RefreshToken");
        if (string.IsNullOrEmpty(refreshToken))
        {
            return false;
        }

        var refreshData = new { refreshToken };
        var json = JsonConvert.SerializeObject(refreshData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(RefreshEndpoint, content);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var tokens = JsonConvert.DeserializeObject<AuthResponse>(responseJson);

            await SecureStorage.SetAsync("AuthToken", tokens.Token);
            await SecureStorage.SetAsync("RefreshToken", tokens.RefreshToken);

            return true;
        }

        return false;
    }

    public async Task<string> GetAuthTokenAsync()
    {
        var authToken = await SecureStorage.GetAsync("AuthToken");
        if (string.IsNullOrEmpty(authToken))
        {
            return null!;
        }

        return authToken;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var authToken = await GetAuthTokenAsync();
        return !string.IsNullOrEmpty(authToken);
    }

    public async Task<bool> LogoutAsync()
    {
        SecureStorage.Remove("AuthToken");
        SecureStorage.Remove("RefreshToken");
        return true;
    }
}