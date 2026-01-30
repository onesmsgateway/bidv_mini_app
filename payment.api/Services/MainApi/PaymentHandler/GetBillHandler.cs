using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using payment.api.AppSettings;
using payment.api.Services.ModelApi;
using payment.api.Services.ModelApi.Request;
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

            var _customer = await _dbContext.CustomerAccountInfos.FirstOrDefaultAsync(t => t.CustomerId == request.CustomerId);
            if (_customer == null)
            {
                return new ApiDetailedResponseBase { StatusCode = HttpStatusCode.BadRequest, Message = "không tồn tại CustomerId", Details = null };
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

                    var externalRequests = await _dbContext.ExternalRequests
                                        .Where(er => er.ServiceId == request.ServiceId && er.CustomerId == request.CustomerId)
                                        .ToListAsync();

                    var monthlyDetails = externalRequests
                           .GroupBy(er =>
                           {
                               if (DateTime.TryParse(er.CreateDate, out DateTime date))
                                   return new { Year = date.Year, Month = date.Month };
                               return new { Year = 0, Month = 0 };
                           })
                           .Where(g => g.Key.Month >= 1 && g.Key.Month <= 12)
                           .Select(g => new PeriodData
                           {
                               Period = $"Tiền dịch vụ tháng {g.Key.Month}/{g.Key.Year}",

                               Data = g.ToList().Select(er => new BillDetail
                               {
                                   BillId = er.BillNumber,
                                   Amount = er.Value,
                                   Remark = $"tiền dịch vụ {er.ServiceId}"
                               }).ToList()
                           })
                           .OrderBy(s => s.Period.Substring(s.Period.Length - 4))
                           .ThenBy(s => s.Period)
                           .ToList();

                    var sumAmount = externalRequests.Sum(er =>
                    {
                        if (decimal.TryParse(er.Value, out decimal amount))
                            return amount;
                        return 0m;
                    });

                    return new ApiGetBillResponse
                    {
                        ResultCode = "000",
                        ResultDesc = "success",
                        CustomerId = _customer.CustomerId,
                        CustomerName = _customer.Fullname,
                        CustomerAddr = "",
                        Type = "0",
                        BillId = "",
                        TotalAmount = sumAmount,
                        Data = monthlyDetails
                    };
                }
                catch (Exception ex)
                {
                    return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Request không hợp lệ.", Details = ex.Message };
                }
            }
        }
    }
}
