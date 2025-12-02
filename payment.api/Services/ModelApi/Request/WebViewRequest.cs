using Microsoft.AspNetCore.Mvc;
using payment.api.Validator;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi.Request
{
    public class WebViewRequest : IApiInput
    {
        public WebViewHeaderRequest WebViewHeaderRequest { get; set; }
        public WebViewBodyRequest WebViewBodyRequest { get; set; }
    }

    public class WebViewHeaderRequest
    {
        [FromHeader, JsonPropertyName("X-Client-ID"), Required(ErrorMessage = "thiếu thông tin trường X-Client-ID")]
        public string XClientID { get; set; }

        [FromHeader, JsonPropertyName("X-Client-Secret"), Required(ErrorMessage = "thiếu thông tin trường X-Client-Secret")]
        public string XClientSecret { get; set; }

        [FromHeader, JsonPropertyName("Content-Type"), Required(ErrorMessage = $"thiếu thông tin trường Content-Type")]
        public string ContentType { get; set; }

        [FromHeader, JsonPropertyName("User-Agent"), Required(ErrorMessage = $"thiếu thông tin trường User-Agent")]
        public string UserAgent { get; set; }

        [FromHeader, JsonPropertyName("Channel"), Required(ErrorMessage = $"thiếu thông tin trường Channel")]
        public string Channel { get; set; }

        // 1. Mandatory: Thời gian gửi request (Ex: 2023-02-21T08:09:09.336Z)
        [FromHeader, JsonPropertyName("Timestamp"), TimestampValidator]
        public string Timestamp { get; set; }

        // 2. Mandatory: Request ID duy nhất để trace log
        [FromHeader, JsonPropertyName("X-API-Interaction-ID"), XApiInteractionIdValidator]
        public string XApiInteractionId { get; set; }

        [FromHeader, JsonPropertyName("X-Idempotency-Key")]
        public string? XIdempotencyKey { get; set; }

        // 3. Optional: Mã đối tác xác định danh cho BIDV (Ex: SSMedia)
        [FromHeader, JsonPropertyName("X-Customer-IP-Address")]
        public string? XCustomerIPAddress { get; set; }

        //https://devbidvapi.ssmedia.vn/bidv-ssmedia/getUrl
        [FromHeader, Required, ApiUrlValidator]
        public string ApiUrl { get; set; }
        [FromHeader, JsonPropertyName("Partner-Code")]
        public string? PartnerCode { get; set; }
    }

    public class WebViewBodyRequest
    {
        /// <summary>
        /// Số GT TT của khách hàng (Bắt buộc - M)
        /// </summary>
        //[JsonPropertyName("user_id")]
        //[Required(ErrorMessage = "thiếu thông tin UserId")]
        [FromBody, JsonPropertyName("user_id"), Required(ErrorMessage = "thiếu thông tin trường user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Mã định danh tiện ích của đối tác (Bắt buộc - M)
        /// </summary>
        [FromBody, JsonPropertyName("service"), Required(ErrorMessage = "thiếu thông tin trường service")]
        public string Service { get; set; }

        /// <summary>
        /// Thông tin KH khác (JSON String) (Tùy chọn - O)
        /// </summary>
        [FromBody, JsonPropertyName("data")]
        public string? Data { get; set; } // Dùng '?' để cho phép giá trị null

        /// <summary>
        /// Ngôn ngữ KH sử dụng (Tùy chọn - O)
        /// </summary>
        [FromBody, JsonPropertyName("language")]
        public string? Language { get; set; } // Dùng '?' để cho phép giá trị null

        /// <summary>
        /// Checksum xác thực (Bắt buộc - M)
        /// </summary>
        [FromBody, JsonPropertyName("checksum"), ChecksumInputValidator]
        public string Checksum { get; set; }
    }
}