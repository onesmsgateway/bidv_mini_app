using System.Text.Json.Serialization;

public class SmartBankingRequest
{
    [JsonPropertyName("a")]
    public string Action { get; set; } = "payment";

    [JsonPropertyName("d")]
    public string EncryptedData { get; set; }

    [JsonPropertyName("s")]
    public string Signature { get; set; }

    [JsonPropertyName("n")]
    public string RequestId { get; set; } = "uuid-12345";

    [JsonPropertyName("p")]
    public string PartnerCode { get; set; } = "partner-001";
}