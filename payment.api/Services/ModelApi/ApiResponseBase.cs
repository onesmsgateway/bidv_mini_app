using Microsoft.AspNetCore.Mvc;
using payment.api.Services.ModelApi.Response;
using System.Net;
using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi
{
    public partial class ApiResponseBase : JsonResult
    {
        public ApiResponseBase() : base(new { })
        {

        }
    }

    public partial class ApiDataResponseBase : IApiResponse
    {
        [JsonPropertyName("result_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonPropertyName("success")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public object? Data { get; set; }
    }

    public partial class ApiDetailedResponseBase : IApiResponse
    {
        [JsonPropertyName("result_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("details")]
        public string Details { get; set; } 
    }
}
