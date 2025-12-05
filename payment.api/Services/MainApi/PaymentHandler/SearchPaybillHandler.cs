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
                string filterPackageId = request.PackageId?.ToLower() ?? string.Empty;
                var _searchData = await _dbContext.ExternalRequests
                                        .Join(
                                            _dbContext.PayBills,
                                            e => e.BillNumber,
                                            p => p.BillNumber,
                                            (e, p) => new SearchPaybillResponse
                                            {
                                                TransactionDate = p.TransactionDate,
                                                Value = p.Value,
                                                PackageId = e.PackageId,
                                                CustomerId = p.CustomerId
                                            }
                                        )
                                        .Where(dto => dto.CustomerId == request.CustomerId && dto.PackageId.ToLower().Contains(filterPackageId))
                                        .Distinct()
                                        .ToListAsync();

                return new ApiDataResponseBase {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success",
                    Data = JsonConvert.SerializeObject(_searchData)
                };

            }
            catch (Exception ex)
            {
                return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadGateway, Message = "search bill failed", Details = ex.Message};
            }
        }
    }
}
