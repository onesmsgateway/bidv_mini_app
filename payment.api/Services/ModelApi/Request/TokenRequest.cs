using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi.Request
{
    public class TokenRequest : IApiInput
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; } = "read";
    }
}
