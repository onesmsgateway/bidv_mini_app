using MediatR;
using Newtonsoft.Json;
using payment.api.AppSettings;
using payment.api.Common;
using payment.api.Services.CommonServices;
using payment.api.Services.ModelApi;
using payment.api.Services.ModelApi.Request;
using payment.api.Services.ModelApi.Request.ExternalRequestExtMethod;
using payment.api.Services.ModelApi.Response;
using payment.entity;
using payment.entity.DbEntities;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.MainApi.PaymentHandler
{
    public class InitPaymentHandler : IRequestHandler<BillBodyRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public InitPaymentHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(BillBodyRequest request, CancellationToken cancellationToken)
        {
            var _datetime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var _externalRequest = new ExternalRequest()
            {
                BillNumber = $"Conek{request.System + _datetime}",
                ServiceId = request.ServiceId,
                CarrierId = request.CarrierId,
                PackageId = request.PackageId,
                DataVolume = request.DataVolume,
                Value = request.Value,
                DiscountCode = request.DiscountCode,
                TotalPaymentAmount = request.TotalPaymentAmount,
                IssueCoporateInvoice = (bool)request.IssueCorporateInvoice ? "true": "false",
                System = request.System,
                CreateDate = DateTime.UtcNow.ToString(),
            };

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var _request = new HttpRequestMessage(HttpMethod.Post, AppConst.bidvSmartBankPaymentUrl);
                    
                    _request.Headers.Add("Content-Type", "application/json");
                    _request.Headers.Add("Channel", AppConst.partnerChannel);
                    _request.Headers.Add("User-Agent", AppConst.partnerUserAgent);
                    _request.Headers.Add("X-API-Interaction-ID", Utils.GenerateRandomDigitWith(12));
                    _request.Headers.Add("X-Client-Certificate", AppConst.partnerXCientCertificate);
                    //X-Idempotency-Key
                    _request.Headers.Add("Timestamp", DateTime.UtcNow.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
                    _request.Headers.Add("X-Customer-IP-Address", AppConst.partnerCustomerIPAddress);
                    _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", (await BidvAccountService.GetTokenAsynWithCache(AppConst.bidvAccessTokenClientId, AppConst.bidvAccessTokenClientSecret, AppConst.bidvCacheTokenKey)).access_token);

                    var _requestObj = new SmartBankingRequest()
                    {
                        Action = "payment", // Hành động
                        EncryptedData = Utils.dEnscryptedData(_externalRequest.CreatePayload()), // Chuỗi ENC(xxxxx)
                        RequestId = Guid.NewGuid().ToString(), // Tạo UUID mới cho mỗi request
                        PartnerCode = AppConst.bidvPartnerCode // Mã đối tác của bạn
                    };
                    _requestObj.Signature = Utils.SHA256withRSA(_requestObj.Action, _requestObj.EncryptedData, _requestObj.RequestId, _requestObj.PartnerCode); //Signature = "signature", // Chuỗi SIG(yyyy)
                    var _payload = BidvJweRequestHelper.GenJWE(JsonConvert.SerializeObject(_requestObj));
                    _request.Headers.Add("X-JWS-Signature", BidvJweRequestHelper.SignJWS(_payload));
                    _request.Content = new StringContent(_payload, Encoding.UTF8, "application/json");
                    
                    var _response = await httpClient.SendAsync(_request);
                    var _responseBody = await _response.Content.ReadAsStringAsync();

                    if (!_response.IsSuccessStatusCode)
                    {
                        return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Request không hợp lệ.", Details = "Request Parse Action Error" };
                    }

                    await _dbContext.AddAsync(_externalRequest);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Request không hợp lệ.", Details = ex.Message};
                }

                return new BillResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success!",
                    Data = new DataBillingResponse()
                    {
                        BillNumber = _externalRequest.BillNumber,
                        RedirectUrl = "trangthanhtoan smartbanking"
                    }
                };
            }
        }
    }
}
