using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPackageTelco.api.Services.ModelApi.Request
{
    public class CustomerAccountInfoRequest
    {
        public string CustomerId { get; set; }
        public string Fullname { get; set; }
    }
}
