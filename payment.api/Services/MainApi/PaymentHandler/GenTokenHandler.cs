using MediatR;
using Microsoft.EntityFrameworkCore;
using payment.api.AppSettings;
using payment.api.Common;
using payment.api.Services.ModelApi;
using payment.entity;
using PaymentPackageTelco.api.Services.ModelApi.Request;
using System.Net;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.MainApi.PaymentHandler
{
    public class GenTokenHandler : IRequestHandler<CustomerAccountInfoRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public GenTokenHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(CustomerAccountInfoRequest request, CancellationToken cancellationToken)
        {
            var _customer = await _dbContext.CustomerAccountInfos.FirstOrDefaultAsync(t => t.CustomerId == request.CustomerId && t.Fullname == request.Fullname);
            if (_customer == null)
            {
                return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "UserId or Fullname is wrong", Details = null };
            }
            var _accessToken = (await JwtUtils.GenerateToken(_customer.CustomerId, _customer?.Fullname ?? "", AppConst.partnerJwtExpired));
            return new ApiDataResponseBase(){ StatusCode = HttpStatusCode.OK, Message ="Success", Data = new { access_token = _accessToken, TimeSpan_hours = AppConst.partnerJwtExpired} };
        }
    }
}
