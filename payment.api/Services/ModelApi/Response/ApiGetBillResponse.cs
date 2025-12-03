using System.Text.Json.Serialization;

namespace PaymentPackageTelco.api.Services.ModelApi.Response
{
    public class ApiGetBillResponse
    {
        [JsonPropertyName("result_code")]
        public string ResultCode { get; set; }

        [JsonPropertyName("result_desc")]
        public string ResultDesc { get; set; }

        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; }

        [JsonPropertyName("customer_name")]
        public string CustomerName { get; set; }

        [JsonPropertyName("customer_addr")]
        public string CustomerAddr { get; set; }

        [JsonPropertyName("bill_id")]
        public string BillId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("total_amount")]
        public string TotalAmount { get; set; }

        [JsonPropertyName("data")]
        // Thuộc tính 'data' ngoài cùng là một mảng các đối tượng chứa chi tiết hóa đơn theo chu kỳ (Period)
        public List<PeriodData> Data { get; set; }
    }

    // --- Lớp: Dữ liệu theo Chu kỳ (Period) ---
    public class PeriodData
    {
        [JsonPropertyName("period")]
        public string Period { get; set; }

        [JsonPropertyName("data")]
        // Thuộc tính 'data' bên trong là một mảng các hóa đơn chi tiết
        public List<BillDetail> Bills { get; set; }
    }

    // --- Lớp: Chi tiết Hóa đơn (Bill Detail) ---
    public class BillDetail
    {
        [JsonPropertyName("bill_id")]
        public string BillId { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        // Lưu ý: Dữ liệu mẫu có 3 cấu trúc remark khác nhau.
        // Tôi sẽ giả định rằng chúng luôn là chuỗi (string)
        [JsonPropertyName("remark")]
        public string Remark { get; set; }
    }
}
