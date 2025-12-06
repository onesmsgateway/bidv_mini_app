using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using payment.api.Services.ModelApi;
using payment.entity;
using PaymentPackage.api.Services.ModelApi.Request;
using PaymentPackageTelco.api.Services.ModelApi.Response;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackage.api.Services.MainApi.PaymentHandler
{
    public class PackageTelcoHandler : IRequestHandler<GetPackageRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public PackageTelcoHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(GetPackageRequest request, CancellationToken cancellationToken)
        {
            var packages = await _dbContext.PackageTelcos
               .Select(p => new PackageResponse {
               PackageName =  p.PackageName ?? "",
               Telco =  p.Telco ?? "",
               Data =  p.Data ?? 0,
               Amount = p.Amount ?? "",
            }).ToListAsync();

            return new ApiDataResponseBase
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Success!",
                Data = packages
            };
        }
    }
}
