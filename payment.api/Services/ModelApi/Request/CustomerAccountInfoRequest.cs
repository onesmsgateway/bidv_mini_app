using System.ComponentModel.DataAnnotations.Schema;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.ModelApi.Request
{
    public class CustomerAccountInfoRequest : IApiInput
    {
        public string CustomerId { get; set; }
        public string Fullname { get; set; }
    }
}
