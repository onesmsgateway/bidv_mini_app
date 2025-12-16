using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace PaymentPackageTelco.api.Services.ModelApi.Response
{
    public class PackageDetailResponse
    {
        public long Id { get; set; }
        public string PackageName { get; set; }
        public string Duration { get; set; }
        public string TotalCapacity { get; set; }
        public string CapacityPerDay { get; set; }
        public int OriginalPrice { get; set; }
        public int SellingPrice { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalQuantitySold { get; set; }
        public string Description { get; set; }
        public string DescriptonData { get; set; }
        public string Status { get; set; }
    }

    public class PackageAreaDetailResponse : PackageDetailResponse
    {
        public string? RtmtqCountry { get; set; }
        public string? Resource { get; set; }
        public string? DescriptionData { get; set; }
    }

    public class PackageNetworkUtilityDetailResponse : PackageDetailResponse
    {
        public string DataSocialNetwork { get; set; }
    }

    public class PackageGroup
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("description"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; set; }
        [JsonPropertyName("packages")]
        public object Packages { get; set; }
    }

    public class DataPackageResponse : IApiResponse
    {
        [JsonPropertyName("status_code")]
        public string StatusCode { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}
