using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi.Response
{
    public class BillResponse : IApiResponse
    {
        public long StatusCode { get; set; }
        public string Message { get; set; }
        public DataBillingResponse Data { get; set; }
    }

    public class DataBillingResponse
    {
        public string BillNumber { get; set; }
        public string RedirectUrl { get; set; }
    }
}
