using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.ModelApi.Request
{
    public class CheckTelcoRequest : IApiInput
    {
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
