using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi.Response
{
    public class ApiAccessTokenResponse : IApiResponse
    {
        // Loại token, thường là "Bearer"
        public string token_type { get; set; }

        // Chuỗi token thực tế được dùng để gọi các API khác
        public string access_token { get; set; }

        // Phạm vi (scope) đã được cấp phép
        public string scope { get; set; }

        // Thời gian hiệu lực của token (tính bằng giây)
        public int expires_in { get; set; }

        // Thời điểm đồng ý cấp phép (timestamp Unix)
        public long consented_on { get; set; }
    }
}
