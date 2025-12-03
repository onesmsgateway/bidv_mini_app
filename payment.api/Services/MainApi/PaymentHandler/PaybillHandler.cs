using MediatR;
using payment.api.Services.ModelApi;
using payment.api.Services.ModelApi.Request;
using payment.entity;
using System.Net;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Services.MainApi.PaymentHandler
{
    public class PaybillHandler : IRequestHandler<PayBillRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public PaybillHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IApiResponse> Handle(PayBillRequest request, CancellationToken cancellationToken)
        {
            var hasIpn = _dbContext.PayBills.Any(ipn => ipn.TransactionId == request.TransactionId || ipn.TransactionBidv == request.TransactionBidv ||
                                                                     ipn.BillNumber.Equals(request.BillNumber));
            if (hasIpn)
            {
                return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Hóa đơn đã gạch nợ rồi (mỗi hóa đơn chỉ gạch nợ 1 lần)", Details = null };
            }

            try
            {
                _dbContext.PayBills.Add(new entity.DbEntities.PayBill
                {
                    TransactionId = request.TransactionId,
                    TransactionBidv = request.TransactionBidv,
                    TransactionDate = request.TransactionDate,
                    CustomerId = request.CustomerId,
                    ServiceId = request.ServiceId,
                    BillNumber = request.BillNumber,
                    Value = request.Value,
                    Checksum = request.Checksum,
                    CreateDate = DateTime.UtcNow.ToString(),
                });
                _dbContext.SaveChanges();

                return new ApiDetailedResponseBase()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "success",
                    Details = null
                };
            }
            catch (Exception ex)
            {
                return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Request không hợp lệ.", Details = ex.Message };
            }
        }
    }
}