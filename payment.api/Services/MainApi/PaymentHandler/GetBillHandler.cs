using MediatR;
using Microsoft.EntityFrameworkCore;
using payment.api.Services.ModelApi;
using payment.api.Services.ModelApi.Request;
using payment.api.Validator;
using payment.entity;
using System.Net;
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

            var _externalRequest = await _dbContext.ExternalRequests.FirstOrDefaultAsync(t=>t.BillNumber == request.BillNumber);
            if(_externalRequest == null)
                return new ApiDetailedResponseBase { StatusCode = HttpStatusCode.NotFound, Message = "Bill not found", Details = null };

            return  new ApiDataResponseBase
            {
               StatusCode = HttpStatusCode.OK,
                Message = "Success!",
                Data = new
                {
                    service_id = _externalRequest.ServiceId,
                    customer_id = "",
                    customer_name = "",
                    customer_addr = "",
                    bill_id = _externalRequest.BillNumber,
                    amount = _externalRequest.Value
                }
            };
        }
    }
}
