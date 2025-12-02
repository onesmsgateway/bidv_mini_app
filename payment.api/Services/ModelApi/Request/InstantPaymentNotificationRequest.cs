using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi.Request
{
    public class InstantPaymentNotificationRequest : IApiInput
    {
        [JsonPropertyName("trans_id")]
        public string TransactionId { get; set; }
        
        [JsonPropertyName("trans_bidv")]
        public string TransactionBidv { get; set; }

        [JsonPropertyName("trans_date")]
        public string TransactionDate { get; set; }

        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; }

        [JsonPropertyName("service_id")]
        public string ServiceId { get; set; }

        [JsonPropertyName("bill_id")]
        public string BillNumber { get; set; }

        [JsonPropertyName("amount")]
        public string Value { get; set; }

        [JsonPropertyName("checksum")]
        public string Checksum { get; set; }
    }
}
