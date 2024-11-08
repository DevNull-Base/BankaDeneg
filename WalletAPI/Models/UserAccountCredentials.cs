using WalletAPI.Services;

namespace WalletAPI.Models;

//TODO: Кажется это исключительно костыль для авторизации на платформе через OpenID Connect
//Это все по-хорошему надо заменить на OAuth 2.0 PKCE, но возможности нет
public class UserAccountCredentials
{
    private readonly ITokenService _tokenService;
    
    public string Id { get; set; }
    public string Login { private get; set; }
    public string Password { private get; set; } //Вот без этого никак
    private DateTime TokenExpiration { get; set; }
    private string _accessToken;

    public UserAccountCredentials(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }
    
    public async Task<string> GetAccessTokenAsync()
    {
        if (_accessToken == null || DateTime.UtcNow >= TokenExpiration)
        {
            _accessToken = await _tokenService.GetNewTokenFromVtbApiAsync(Login, Password);
            TokenExpiration = DateTime.UtcNow.AddSeconds(1700);
        }

        return _accessToken;
    }
}