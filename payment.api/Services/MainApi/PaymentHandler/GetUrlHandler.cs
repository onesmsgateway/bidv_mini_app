using MediatR;
using Microsoft.EntityFrameworkCore;
using payment.api.AppSettings;
using payment.api.Common;
using payment.api.Services.CommonServices;
using payment.api.Services.ModelApi.Request;
using payment.api.Services.ModelApi.Response;
using payment.entity;
using payment.entity.DbEntities;
using System.Text;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.MainApi.PaymentHandler
{
    public class GetUrlHandler : IRequestHandler<WebViewRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public GetUrlHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(WebViewRequest request, CancellationToken cancellationToken)
        {
           if (request.WebViewBodyRequest.Service !=  AppConst.partnerServiceId)
            {
                return new WebViewResponse { Url = "", ResultCode = "005", ResultDesc = "service không hợp lệ" };
            }
            var _customer = await _dbContext.CustomerAccountInfos.FirstOrDefaultAsync(c => c.CustomerId == request.WebViewBodyRequest.UserId);
            if (_customer == null)
            {
                _dbContext.Add(new CustomerAccountInfo
                {
                    CustomerId = request.WebViewBodyRequest.UserId,
                    CreateDate = DateTime.UtcNow.ToString()
                });
            }
            
            var _urlBuilder = new StringBuilder();
            _urlBuilder.Append(AppConst.webUrlBase); 
            _urlBuilder.Append($"&token={(await BidvAccountService.GetTokenAsynWithCache(AppConst.bidvAccessTokenClientId, AppConst.bidvAccessTokenClientSecret, AppConst.bidvCacheTokenKey)).access_token}");
            
            return new WebViewResponse { Url = Utils.Base64UrlEncode(Encoding.UTF8.GetBytes(_urlBuilder.ToString())), ResultCode = "000", ResultDesc = "Success!" };
        }
    }
}
