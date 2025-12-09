using MediatR;
using Microsoft.EntityFrameworkCore;
using payment.api.Services.ModelApi;
using payment.entity;
using PaymentPackageTelco.api.Services.ModelApi.Response;
using System.Globalization;
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

                var _monthlyDetails = _searchData
                                    .GroupBy(er =>
                                    {
                                        if (DateTime.TryParseExact(er.TransactionDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime parsedDate))
                                            return new {Year = parsedDate.Year, Month = parsedDate.Month };
                                        return new {Year = 0, Month =0};
                                    })
                                    .Where(g => g.Key.Month >=1 && g.Key.Month <=12)
                                    .Select(g => new 
                                    {
                                        Period = $"Tháng {g.Key.Month}/{g.Key.Year}",
                                        Data = g.ToList().Select(er => new
                                        {
                                            TransactionDate = er.TransactionDate,
                                            Value = er.Value,
                                            PackageId = er.PackageId
                                        }).ToList()
                                    })
                                    .OrderBy(s => s.Period.Substring(s.Period.Length - 4))
                                    .ThenBy(s => s.Period)
                                    .ToList();
                                    
                return new ApiDataResponseBase {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success",
                    Data = _monthlyDetails
                };

            }
            catch (Exception ex)
            {
                return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.BadGateway, Message = "search bill failed", Details = ex.Message};
            }
        }
    }
}
