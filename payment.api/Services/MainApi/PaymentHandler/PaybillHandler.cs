using MediatR;
using Microsoft.EntityFrameworkCore;
using payment.api.Services.ModelApi;
using payment.api.Services.ModelApi.Request;
using payment.entity;
using payment.entity.DbEntities;
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
            var count = await _dbContext.PayBills.CountAsync(ipn => ipn.BillNumber == request.BillNumber || ipn.TransactionId == request.TransactionId 
                                                                               || ipn.TransactionBidv == request.TransactionBidv, cancellationToken);
            if (count > 0)
            {
                return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Hóa đơn đã gạch nợ rồi (mỗi hóa đơn chỉ gạch nợ 1 lần)", Details = null };
            }

            try
            {
                var _payBill = new PayBill()
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
                };
                await _dbContext.PayBills.AddAsync(_payBill, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new ApiDataResponseBase()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "success",
                    Data = new { _payBill.BillNumber, _payBill.ServiceId, _payBill.Value, _payBill.CreateDate}
                };
            }
            catch (Exception ex)
            {
                return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadRequest, Message = "Request không hợp lệ.", Details = ex.Message };
            }
        }
    }
}