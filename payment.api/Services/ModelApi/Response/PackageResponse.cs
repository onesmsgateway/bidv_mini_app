using System.Text.Json.Serialization;

namespace PaymentPackageTelco.api.Services.ModelApi.Response
{
    public class PackageResponse
    {
        [JsonPropertyName("package_name")]
        public string PackageName { get; set; }
        [JsonPropertyName("telco")]
        public string Telco { get; set; }
        [JsonPropertyName("data")]
        public decimal Data { get; set; }
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
        [JsonPropertyName("date_use")]
        public int? DateUse { get; set; }

        [JsonPropertyName("is_combo")]
        public string IsCombo { get; set; }
        [JsonPropertyName("data_social_network")]
        public string? DataSocialNetwork { get; set; }
        [JsonPropertyName("package_type")]
        public string IsDomesticPackage { get; set; } //trongnuoc, quocte
        [JsonPropertyName("is_flash_sale")]
        public string IsFlashSale { get; set; }
        [JsonPropertyName("from_time_flash_sale")]
        public string? FromTimeFlashSale { get; set; }
        [JsonPropertyName("to_time_flash_sale")]
        public string? ToTimeFlashSale { get; set; }
        [JsonPropertyName("resource")]
        public string? Resouce { get; set; } //tai nguyen trong nuoc roaming data: 2GB data - Roaming 42 nước
        [JsonPropertyName("rmtq_country")]
        public string? RmtqCountry { get; set; }
        [JsonPropertyName("note")]
        public string? Note { get; set; }
    }
}
