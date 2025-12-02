using MediatR;
using payment.api.AppSettings;
using payment.api.Common;
using payment.api.Services.CommonServices;
using payment.api.Services.ModelApi.Request;
using payment.api.Services.ModelApi.Response;
using payment.entity;
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



            var _urlBuilder = new StringBuilder();
            _urlBuilder.Append(AppConst.webUrlBase); 
            _urlBuilder.Append($"&token={(await BidvAccountService.GetAccessTokenAsync()).AccessToken}");
            
            return (new WebViewResponse { Url = Utils.Base64UrlEncode(Encoding.UTF8.GetBytes(_urlBuilder.ToString())), ResultCode = "000", ResultDesc = "Success!" });
        }
    }
}
