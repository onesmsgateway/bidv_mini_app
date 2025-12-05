using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using payment.api.AppSettings;
using payment.api.Services.ModelApi;
using payment.api.Services.ModelApi.Request;
using payment.api.Validator;
using payment.entity;
using PaymentPackageTelco.api.Services.ModelApi.Response;
using System.Net;
using System.Text;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.MainApi.PaymentHandler
{
    public class GetBillHandler : IRequestHandler<GetBillBodyRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public GetBillHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(GetBillBodyRequest request, CancellationToken cancellationToken)
        {
            if (!request.IsValidChecksum())
            {
                return new ApiDetailedResponseBase { StatusCode = HttpStatusCode.BadRequest, Message = "Invalid Checksum", Details = null };
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var _urlGetbill = AppConst.bidvGetBillUrl;
                    var _content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                    #region chở đấu nối bidv
                    /*
                    var _response = await httpClient.PostAsync(_urlGetbill, _content);
                    var _responseBody = await _response.Content.ReadAsStringAsync();

                    if (!_response.IsSuccessStatusCode)
                    {
                        return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Request không hợp lệ.", Details = "Request Parse Action Error" };
                    }

                    var _apiGetBillResponse = JsonConvert.DeserializeObject<ApiGetBillResponse>(_responseBody);
                    if (_apiGetBillResponse.ResultCode != "000")
                    {
                        return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Request không hợp lệ.", Details = _apiGetBillResponse.ResultDesc };
                    }
                    */
                    #endregion

                    var _billNumber = await _dbContext.ExternalRequests
                                        .Where(er => er.ServiceId == request.ServiceId && er.BillNumber == request.BillNumber)
                                        .ToListAsync();

                    return new ApiDataResponseBase { StatusCode = HttpStatusCode.OK, Message = "Success!", Data = JsonConvert.SerializeObject(_billNumber) /*JsonConvert.SerializeObject(_apiGetBillResponse)*/};
                }
                catch (Exception ex)
                {
                    return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Request không hợp lệ.", Details = ex.Message };
                }
            }
        }
    }
}
