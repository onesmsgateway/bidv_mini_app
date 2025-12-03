using MediatR;
using payment.api.AppSettings;
using payment.api.Services.CommonServices;
using payment.api.Services.ModelApi.Request;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.MainApi.PaymentHandler
{
    public class GetTokenHandler : IRequestHandler<TokenRequest, IApiResponse>
    {
        public async Task<IApiResponse> Handle(TokenRequest request, CancellationToken cancellationToken)
        {
            return await BidvAccountService.GetTokenAsynWithCache(AppConst.bidvAccessTokenClientId, AppConst.bidvAccessTokenClientSecret, AppConst.bidvCacheTokenKey);
        }
    }
}
