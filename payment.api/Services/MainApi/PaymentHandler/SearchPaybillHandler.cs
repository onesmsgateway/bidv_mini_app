using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using payment.api.Services.ModelApi;
using payment.entity;
using PaymentPackageTelco.api.Services.ModelApi.Response;
using System.Net;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.MainApi.PaymentHandler
{
    public class SearchPaybillHandler : IRequestHandler<SearchPaybillRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public SearchPaybillHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(SearchPaybillRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var _combinedData = await _dbContext.ExternalRequests
                                        .Join(
                                            _dbContext.PayBills,
                                            externalRequest => externalRequest.BillNumber,
                                            payBill => payBill.BillNumber,
                                            (e, p) => new
                                            {
                                                ExternalRequest = e,
                                                PayBill = p
                                            }
                                        )
                                        .Where(combined => combined.ExternalRequest.PackageId.Contains(request.PackageId))
                                        .Distinct()
                                        .ToListAsync();

                return new ApiDataResponseBase {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success",
                    Data = JsonConvert.SerializeObject(_combinedData)
                };

            }
            catch (Exception ex)
            {
                return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadGateway, Message = "search bill failed", Details = ex.Message};
            }
        }
    }
}
