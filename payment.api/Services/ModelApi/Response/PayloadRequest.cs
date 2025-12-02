using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.ModelApi.Response
{
    public class PayloadRequest
    {
        public string Timestamp { get; set; }
        public Payment Extra { get; set; }
    }

    public class Payment
    {
        public string Invoice { get; set; }
        public string Amount { get; set; }
        public string ServiceId { get; set; }
        public string Note { get; set; }
        public IList<OrderInfo> orderInfors { get; set; }
    }

    public class OrderInfo
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
