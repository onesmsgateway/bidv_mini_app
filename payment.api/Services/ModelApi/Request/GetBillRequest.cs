using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi.Request
{
    public class GetBillBodyRequest : IApiInput
    {
        [FromBody, JsonPropertyName("customer_id"), Required(ErrorMessage = "Thiếu thông tin trường customer_id")]
        public string CustomerId { get; set; }
        //[FromBody, JsonPropertyName("billnumber"), Required(ErrorMessage = "Thiếu thông tin trường billnumber")]
        //public string BillNumber { get; set; }
        [FromBody, JsonPropertyName("service_id"), Required(ErrorMessage = "Thiếu thông tin trường service_id")]
        public string ServiceId { get; set; }
        [FromBody, JsonPropertyName("checksum"), Required(ErrorMessage = "Thiếu thông tin trường checksum (billnumber, service)")]
        public string Checksum { get; set; }
    }
}
