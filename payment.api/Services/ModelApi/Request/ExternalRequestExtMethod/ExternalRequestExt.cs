using Newtonsoft.Json;
using payment.api.Services.ModelApi.Response;
using payment.entity.DbEntities;

namespace payment.api.Services.ModelApi.Request.ExternalRequestExtMethod
{
    public static class ExternalRequestExt
    {
        public static string CreatePayload(this ExternalRequest _externalRequest)
        {
            var _paymentPayload = new PayloadRequest
            {
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                Extra = new Payment
                {
                    Invoice = _externalRequest.BillNumber,
                    Amount = _externalRequest.Value.ToString(),
                    ServiceId = _externalRequest.ServiceId,
                    Note = "",
                    orderInfors = new List<OrderInfo>()
                }
            };
            return JsonConvert.SerializeObject(_paymentPayload);
        }
    }
}
