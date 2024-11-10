using Newtonsoft.Json;

namespace SharedModels;

public struct AuthResponse
{
    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("refreshToken")]
    public string RefreshToken { get; set; }
}