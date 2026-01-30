using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.ModelApi.Response
{
    public class CheckTelcoResponse : IApiResponse
    {
        [JsonPropertyName("TransID")]
        public string TransId { get; set; }

        [JsonPropertyName("Telco")]
        public string Telco { get; set; }


        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}
