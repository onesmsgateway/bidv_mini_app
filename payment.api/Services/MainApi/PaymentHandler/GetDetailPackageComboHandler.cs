using MediatR;
using Microsoft.EntityFrameworkCore;
using payment.api.Common;
using payment.api.Services.ModelApi;
using payment.entity;
using PaymentPackageTelco.api.Common;
using PaymentPackageTelco.api.Services.ModelApi.Request;
using PaymentPackageTelco.api.Services.ModelApi.Response;
using System.Net;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.MainApi.PaymentHandler
{
    public class GetDetailPackageComboHandler : IRequestHandler<GetDetailPackageComboRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public GetDetailPackageComboHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(GetDetailPackageComboRequest request, CancellationToken cancellationToken)
        {
            var _package = await _dbContext.PackageTelcos.FirstOrDefaultAsync(t => t.PackageName.ToLower() == request.package_name.ToLower() && t.PackageType == (int)PackageType.combo);
            var _externalRequest = _dbContext.ExternalRequests.AsQueryable();

            if (_package == null)
            {
                return new ApiDetailedResponseBase() { StatusCode = HttpStatusCode.NotFound, Message = "gói cước không tồn tại.", Details = null };
            }

            var detailPackage = new PackageAreaDetailResponse()
            {
                Id = _package.Id,
                PackageName = _package.PackageName.GetValueOrDefault(),
                Duration = $"{_package.DateUse.GetValueOrDefault()} ngày",
                TotalCapacity = $"{_package.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                CapacityPerDay = $"{_package.TotalCapacity.GetValueOrDefault().ToGbPerDay(_package.DateUse.GetValueOrDefault())} GB/ngày",
                OriginalPrice = _package.Amount.ParseToIntOrDefault(),
                SellingPrice = _package.SellingPrice.GetValueOrDefault(),
                RtmtqCountry = _package.RmtqCountry.GetValueOrDefault(),
                Resource = _package.Resource.GetValueOrDefault(),
                Description = _package.Description.GetValueOrDefault(),
                DescriptionData = _package.DescriptionData.GetValueOrDefault(),
                TotalQuantity = _package.TotalQuanlity.GetValueOrDefault(),
                TotalQuantitySold = _externalRequest.Count(t => t.PackageId.ToLower() == _package.PackageName),
                Status = _package.Status
            };

            return new DataPackageResponse()
            {
                StatusCode = "000",
                Message = "success",
                Data = detailPackage,
            };
        }
    }

}
