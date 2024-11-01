using System.Net.Http.Headers;
using WalletAPI.Services;

namespace WalletAPI.Handlers;

public class TokenHandler: DelegatingHandler
{
    private readonly ITokenService _tokenService;

    public TokenHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        return await base.SendAsync(request, cancellationToken);
    }
}