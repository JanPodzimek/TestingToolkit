using System.Text.Json.Serialization;

namespace TradeApiCaller.Authentication
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
