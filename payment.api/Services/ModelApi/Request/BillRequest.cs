using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi.Request
{
    public class BillBodyRequest: IApiInput
    {
        [FromBody, JsonPropertyName("service_id"), Required(ErrorMessage ="thiếu thong tin trường ServiceId")]
        public string? ServiceId { get; set; } // Dịch vụ (Data)

        [FromBody, JsonPropertyName("carrier_id"), Required(ErrorMessage = "thiếu thong tin trường CarrierId")]
        public string? CarrierId { get; set; } // Nhà mạng (Mobifone, Viettel)

        [FromBody, JsonPropertyName("package_id"), Required(ErrorMessage = "thiếu thong tin trường PackageId")]
        public string? PackageId { get; set; } // Gói cước (GT30D)

        [FromBody, JsonPropertyName("data_volume"), Required(ErrorMessage = "thiếu thong tin trường DataVolumn")]
        public string? DataVolume { get; set; } // Dung lượng (500MB/2 giờ)

        [FromBody, JsonPropertyName("value"), Required(ErrorMessage = "thiếu thong tin trường Value")]
        public string? Value { get; set; } // Mệnh giá (10.000)

        // Thông tin thanh toán
        [FromBody, JsonPropertyName("discount_code"), Required(ErrorMessage = "thiếu thong tin trường DiscountCode")]
        public string? DiscountCode { get; set; } // Mã giảm giá

        [FromBody, JsonPropertyName("total_payment_amount"), Required(ErrorMessage = "thiếu thong tin trường TotalPaymentAmount")]
        public string? TotalPaymentAmount { get; set; } // Số tiền thanh toán (10,000 VND)

        [FromBody, JsonPropertyName("total_payment_amount"), Required(ErrorMessage = "thiếu thong tin trường TotalPaymentAmount")]
        public bool? IssueCorporateInvoice { get; set; } // Xuất hóa đơn doanh nghiệp (Sử dụng bool nếu là dạng bật/tắt)

        [FromBody, JsonPropertyName("system"), Required(ErrorMessage = "thiếu thong tin trường System")]
        public string? System { get; set; }
    }

    public class BillHeaderRequest : IApiInput
    {

    }
}
