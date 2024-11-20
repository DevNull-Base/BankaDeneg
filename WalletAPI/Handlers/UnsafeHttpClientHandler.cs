namespace WalletAPI.Handlers;

public class UnsafeHttpClientHandler : HttpClientHandler
{
    public UnsafeHttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true;
    }
}