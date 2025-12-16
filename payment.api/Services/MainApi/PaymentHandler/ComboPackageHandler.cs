using MediatR;
using Microsoft.EntityFrameworkCore;
using payment.api.Common;
using payment.entity;
using payment.entity.DbEntities;
using PaymentPackageTelco.api.Common;
using PaymentPackageTelco.api.Services.ModelApi.Request;
using PaymentPackageTelco.api.Services.ModelApi.Response;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.MainApi.PaymentHandler
{
    public class ComboPackageHandler : IRequestHandler<FilterComboDataPackageRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public ComboPackageHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(FilterComboDataPackageRequest request, CancellationToken cancellationToken)
        {
            var _packageTecos = _dbContext.PackageTelcos.Where(p => p.PackageType == (int)PackageType.combo).AsQueryable();
            var _socialPackage = _packageTecos.Where(p => p.UtilityType == 1 && p.DataSocialNetwork != null && p.DataSocialNetwork != "");
            var _externalRequests = _dbContext.ExternalRequests.AsQueryable();
            short fourItems = 4;

            var _popularPackages = await _packageTecos
            .Select(p => new PackageDetailResponse
            {
                Id = p.Id,
                PackageName = p.PackageName.GetValueOrDefault(),
                Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                OriginalPrice = p.Amount.ParseToIntOrDefault(),
                SellingPrice = p.SellingPrice.GetValueOrDefault(),
                TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                Description = p.Description.GetValueOrDefault(),
                DescriptonData = p.DescriptionData.GetValueOrDefault(),
                Status = p.Status.GetValueOrDefault()
            })
            .ToListAsync();

            var _socialNetwork = await _socialPackage
                .Select(p => new PackageNetworkUtilityDetailResponse
                {
                    Id = p.Id,
                    PackageName = p.PackageName.GetValueOrDefault(),
                    Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                    TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                    CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                    OriginalPrice = p.Amount.ParseToIntOrDefault(),
                    SellingPrice = p.SellingPrice.GetValueOrDefault(),
                    TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                    TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                    Description = p.Description.GetValueOrDefault(),
                    DescriptonData = p.DescriptionData.GetValueOrDefault(),
                    DataSocialNetwork = p.DataSocialNetwork.GetValueOrDefault(),
                    Status = p.Status
                })
                .ToListAsync();

            var packageGroups = new List<PackageGroup>();
            if (_popularPackages.Any())
                packageGroups.Add(new PackageGroup { Category = "Gói combo data giá tốt", Description = "Gói combo data giá tốt", Packages = _popularPackages });
            if (_socialNetwork.Any())
                packageGroups.Add(new PackageGroup { Category = "Gói giá trị, mạng xã hội xả láng", Description = "Gói giá trị, mạng xã hội xả láng", Packages = _socialNetwork });

            return new DataPackageResponse()
            {
                StatusCode = "000",
                Message = "success",
                Data = packageGroups
            };
        }
    }
}