using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi.Response
{
    public class WebViewResponse : IApiResponse
    {
        /// <summary>
        /// Link Url của Webview (Bắt buộc - M)
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Mã lỗi (Bắt buộc - M). Ví dụ: "00" cho Thành công.
        /// </summary>
        [JsonPropertyName("result_code")]
        public string ResultCode { get; set; }

        /// <summary>
        /// Mô tả lỗi (Bắt buộc - M). Ví dụ: "Thành công" hoặc "Lỗi xác thực".
        /// </summary>
        [JsonPropertyName("result_desc")]
        public string ResultDesc { get; set; }
    }
}
