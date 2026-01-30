using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi
{
    public partial class ApiResponseBase : JsonResult, IApiResponse
    {
        public ApiResponseBase() : base(new { })
        {

        }
    }

    public partial class ApiDataResponseBase : IApiResponse
    {
        [JsonPropertyName("result_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonPropertyName("message")]
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
