using MediatR;
using Microsoft.EntityFrameworkCore;
using payment.api.Common;
using payment.entity;
using PaymentPackage.api.Services.ModelApi.Request;
using PaymentPackageTelco.api.Common;
using PaymentPackageTelco.api.Services.ModelApi.Response;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackage.api.Services.MainApi.PaymentHandler
{
    public class TopupDataPackageHandler : IRequestHandler<FilterAreaPackageTopupRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public TopupDataPackageHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(FilterAreaPackageTopupRequest request, CancellationToken cancellationToken)
        {
            short _fourItemsDefault = 4; short _twoItems = 2;
            short _maxIitems = 1000;

            bool isUseFilter = request.DateUse.HasValue || request.SocialNet.HasValue || request.DataVolume.HasValue;
            var isDomesticArea = !request.AreaType.HasValue || request.AreaType.Value == AreaType.domestic;
            var _packageTecos = _dbContext.PackageTelcos.Where(p=> p.PackageType == (int)PackageType.topupdata || p.PackageType == null).AsQueryable();
            var _socialNet = _packageTecos.Where(p => p.UtilityType == (int)UtilityType.social_network);
            
            var _externalRequests = _dbContext.ExternalRequests.AsQueryable();

            if (request.AreaType.HasValue)
            {
                switch (request.AreaType.Value)
                {
                    case AreaType.domestic:
                        _packageTecos = _packageTecos.Where(p => p.AreaPackage == null || p.AreaPackage == 0);
                        break;
                    case AreaType.internation:
                        _packageTecos = _packageTecos.Where(p => p.AreaPackage > 0);
                        break;
                    default: break;
                }
            }   
            
            if (request.DateUse.HasValue)
            {
                switch (request.DateUse.Value)
                {
                    case DateUse.time:
                        _packageTecos = _packageTecos.Where(p => p.DateUse.HasValue && p.DateUse.Value <= 1);
                        _socialNet = _socialNet.Where(p => p.DateUse.HasValue && p.DateUse.Value <= 1); ;
                        break;
                    case DateUse.onetosevendays:
                        _packageTecos = _packageTecos.Where(p => p.DateUse.HasValue && p.DateUse.Value > 1 && p.DateUse.Value <= 7);
                        _socialNet = _socialNet.Where(p => p.DateUse.HasValue && p.DateUse.Value > 1 && p.DateUse.Value <= 7); ;
                        break;
                    case DateUse.seventofiveteendays:
                        _packageTecos = _packageTecos.Where(p => p.DateUse.HasValue && p.DateUse.Value > 7 && p.DateUse.Value <= 15);
                        _socialNet = _socialNet.Where(p => p.DateUse.HasValue && p.DateUse.Value > 7 && p.DateUse.Value <= 15); ;
                        break;
                    case DateUse.fiveteentothirtydays:
                        _packageTecos = _packageTecos.Where(p => p.DateUse.HasValue && p.DateUse.Value > 15 && p.DateUse.Value <= 30);
                        _socialNet = _socialNet.Where(p => p.DateUse.HasValue && p.DateUse.Value > 15 && p.DateUse.Value <= 30);
                        break;
                    default: break;
                }
            }

            if (request.DataVolume.HasValue)
            {
                switch (request.DataVolume.Value)
                {
                    case DataVolume.smallthan1gb:
                        _packageTecos = _packageTecos.Where(p => p.Data < 1024);
                        break;
                    case DataVolume.from1gbto2gb:
                        _packageTecos = _packageTecos.Where(p => p.Data >= 1024 && p.Data < 2048);
                        break;
                    case DataVolume.from2gbto4gb:
                        _packageTecos = _packageTecos.Where(p => p.Data >= 2048 && p.Data < 4096);
                        break;
                    case DataVolume.from4gbto8gb:
                        _packageTecos = _packageTecos.Where(p => p.Data >= 4096 && p.Data < 8192);
                        break;
                    case DataVolume.morethan8gb:
                        _packageTecos = _packageTecos.Where(p => p.Data >= 8192);
                        break;
                    default: break;
                }
            }

            if (isDomesticArea)
            {
                if (request.SocialNet.HasValue)
                {
                    _socialNet = _socialNet.Where(p => p.DataSocialNetwork.ToLower().Contains(request.SocialNet.Value.ToString().ToLower()));
                }

                var _popularPackages = await _packageTecos
                .Where(p => p.UtilityType  == null || p.UtilityType == 0) // loc goi cuoc pho bien khong phai tien ich
                .OrderByDescending(p => _externalRequests.Where(t => t.PackageId == p.PackageName).Count())
                .Take(isUseFilter ? _maxIitems : _fourItemsDefault)
                .Select(p => new PackageDetailResponse
                {
                    Id = p.Id,
                    PackageName = p.PackageName.GetValueOrDefault(),
                    Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                    TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                    CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                    DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                    PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                    OriginalPrice = p.Amount.ParseToIntOrDefault(),
                    SellingPrice = p.SellingPrice.GetValueOrDefault(),
                    TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                    TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                    Description = p.Description.GetValueOrDefault(),
                    DescriptonData = p.DescriptionData.GetValueOrDefault(),
                    Status = p.Status.GetValueOrDefault()
                })
                .ToListAsync();

                 var _socialNet1 =  await _socialNet
                .Take(isUseFilter ? _maxIitems : _fourItemsDefault)
                .Select(p => new PackageNetworkUtilityDetailResponse
                {
                    Id = p.Id,
                    PackageName = p.PackageName.GetValueOrDefault(),
                    Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                    TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                    CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                    DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                    PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                    OriginalPrice = p.Amount.ParseToIntOrDefault(),
                    SellingPrice = p.SellingPrice.GetValueOrDefault(),
                    TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                    TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                    Description = p.Description.GetValueOrDefault(),
                    DescriptonData = p.DescriptionData.GetValueOrDefault(),
                    DataSocialNetwork = p.DataSocialNetwork.GetValueOrDefault(),
                    Status = p.Status
                }).ToListAsync();

                var _1dayPackages = await _packageTecos.Where(p => p.DateUse == 1 && (p.UtilityType == null || p.UtilityType ==0))
                    .Take(isUseFilter ? _maxIitems : _twoItems)
                    .Select( p => new PackageDetailResponse
                    {
                        Id = p.Id,
                        PackageName = p.PackageName.GetValueOrDefault(),
                        Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                        TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                        CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                        DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                        PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                        OriginalPrice = p.Amount.ParseToIntOrDefault(),
                        SellingPrice = p.SellingPrice.GetValueOrDefault(),
                        TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                        TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                        Description = p.Description.GetValueOrDefault(),
                        DescriptonData = p.DescriptionData.GetValueOrDefault(),
                        Status = p.Status.GetValueOrDefault()
                    }).ToListAsync();

                var _3dayPackages = await _packageTecos.Where(p => p.DateUse == 3  && (p.UtilityType == null || p.UtilityType == 0))
                    .Take(isUseFilter ? _maxIitems : _twoItems)
                    .Select(p => new PackageDetailResponse
                    {
                        Id = p.Id,
                        PackageName = p.PackageName.GetValueOrDefault(),
                        Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                        TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                        CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                        DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                        PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                        OriginalPrice = p.Amount.ParseToIntOrDefault(),
                        SellingPrice = p.SellingPrice.GetValueOrDefault(),
                        TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                        TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                        Description = p.Description.GetValueOrDefault(),
                        DescriptonData = p.DescriptionData.GetValueOrDefault(),
                        Status = p.Status.GetValueOrDefault()
                    }).ToListAsync();
                
                var _7dayPackages = await _packageTecos.Where(p => p.DateUse == 7 && (p.UtilityType == null || p.UtilityType == 0))
                    .Take(isUseFilter ? _maxIitems : _fourItemsDefault)
                    .Select(p => new PackageDetailResponse
                    {
                        Id = p.Id,
                        PackageName = p.PackageName.GetValueOrDefault(),
                        Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                        TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                        CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                        DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                        PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                        OriginalPrice = p.Amount.ParseToIntOrDefault(),
                        SellingPrice = p.SellingPrice.GetValueOrDefault(),
                        TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                        TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                        Description = p.Description.GetValueOrDefault(),
                        DescriptonData = p.DescriptionData.GetValueOrDefault(),
                        Status = p.Status.GetValueOrDefault()
                    }).ToListAsync();
                
                var _20dayPackages = await _packageTecos.Where(p => p.DateUse == 20 && (p.UtilityType == null || p.UtilityType == 0))
                    .Take(isUseFilter ? _maxIitems : _fourItemsDefault)
                    .Select(p => new PackageDetailResponse
                    {
                        Id = p.Id,
                        PackageName = p.PackageName.GetValueOrDefault(),
                        Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                        TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                        CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                        DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                        PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                        OriginalPrice = p.Amount.ParseToIntOrDefault(),
                        SellingPrice = p.SellingPrice.GetValueOrDefault(),
                        TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                        TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                        Description = p.Description.GetValueOrDefault(),
                        DescriptonData = p.DescriptionData.GetValueOrDefault(),
                        Status = p.Status.GetValueOrDefault()
                    }).ToListAsync();
                
                var _30dayPackages = await _packageTecos.Where(p => p.DateUse == 30)
                    .Take(isUseFilter ? _maxIitems : _fourItemsDefault)
                    .Select(p => new PackageDetailResponse
                     {
                         Id = p.Id,
                         PackageName = p.PackageName.GetValueOrDefault(),
                         Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                         TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                         CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                         DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                         PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                         OriginalPrice = p.Amount.ParseToIntOrDefault(),
                         SellingPrice = p.SellingPrice.GetValueOrDefault(),
                         TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                         TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                         Description = p.Description.GetValueOrDefault(),
                         DescriptonData = p.DescriptionData.GetValueOrDefault(),
                         Status = p.Status.GetValueOrDefault()
                     }).ToListAsync();

                var packageGroups = new List<PackageGroup>();
                if (_popularPackages.Any())
                    packageGroups.Add(new PackageGroup { Category = "Gói cước phổ biến", Description = "Gói cước data phổ biến", Packages = _popularPackages});
                if (_socialNet1.Any())
                    packageGroups.Add(new PackageGroup { Category = "Gói cước tiện ích", Description = "Gói cước data tiện ích", Packages = _socialNet1 });
                if (_1dayPackages.Any())
                    packageGroups.Add(new PackageGroup { Category = "Gói data 1 ngày", Packages = _1dayPackages });
                if (_3dayPackages.Any())
                    packageGroups.Add(new PackageGroup { Category = "Gói data 3 ngày", Packages = _3dayPackages });
                if (_7dayPackages.Any())
                    packageGroups.Add(new PackageGroup { Category = "Gói data 7 ngày", Packages = _7dayPackages });
                if (_20dayPackages.Any())
                    packageGroups.Add(new PackageGroup { Category = "Gói data 20 ngày", Packages = _20dayPackages });
                if (_30dayPackages.Any()) 
                    packageGroups.Add(new PackageGroup { Category = "Gói data 30 ngày", Packages = _30dayPackages });
                
                return new DataPackageResponse()
                {
                    StatusCode = "000",
                    Message = "success",
                    Data = packageGroups
                };
            }
            else if (request.AreaType == AreaType.internation) //area.internation
            {
                //goi combo quoc gia
                var _nationPackages = await _packageTecos.Where(p => p.AreaPackage == (int)AreaInternation.nation)
                .Take(isUseFilter ? _maxIitems : _fourItemsDefault)
                .Select(p => new PackageAreaDetailResponse
                {
                    Id = p.Id,
                    PackageName = p.PackageName.GetValueOrDefault(),
                    Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                    TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                    CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                    DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                    PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                    OriginalPrice = p.Amount.ParseToIntOrDefault(),
                    SellingPrice = p.SellingPrice.GetValueOrDefault(),
                    RtmtqCountry = p.RmtqCountry.GetValueOrDefault(),
                    Resource = p.Resource.GetValueOrDefault(),
                    Description = p.Description.GetValueOrDefault(),
                    DescriptionData = p.DescriptionData.GetValueOrDefault(),
                    TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                    TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                    Status = p.Status
                })
                .ToListAsync();

                //goi data khu vuc
                var _areaPackages = await _packageTecos.Where(p => p.AreaPackage == (int)AreaInternation.area)
                .Take(isUseFilter ? _maxIitems : _fourItemsDefault)
                .Select(p => new PackageAreaDetailResponse
                {
                    Id = p.Id,
                    PackageName = p.PackageName.GetValueOrDefault(),
                    Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                    TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                    CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                    DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                    PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                    OriginalPrice = p.Amount.ParseToIntOrDefault(),
                    SellingPrice = p.SellingPrice.GetValueOrDefault(),
                    RtmtqCountry = p.RmtqCountry.GetValueOrDefault(),
                    Resource = p.Resource.GetValueOrDefault(),
                    Description = p.Description.GetValueOrDefault(),
                    DescriptionData = p.DescriptionData.GetValueOrDefault(),
                    TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                    TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                    Status = p.Status
                })
                .ToListAsync();

                //goi data quoc te
                var _internationalPackages = await _packageTecos.Where(p => p.AreaPackage == (int)AreaType.internation)
                .Take(isUseFilter ? _maxIitems : _fourItemsDefault)
                .Select(p => new PackageAreaDetailResponse
                {
                    Id = p.Id,
                    PackageName = p.PackageName.GetValueOrDefault(),
                    Duration = $"{p.DateUse.GetValueOrDefault()} ngày",
                    TotalCapacity = $"{p.TotalCapacity.GetValueOrDefault().ToGb()} GB",
                    CapacityPerDay = $"{p.TotalCapacity.GetValueOrDefault().ToGbPerDay(p.DateUse.GetValueOrDefault())} GB/ngày",
                    DataType = EnumUtil.ToUtilityTypeName(p.UtilityType.GetValueOrDefault()),
                    PackageType = EnumUtil.ToPackageTypeName(p.PackageType.GetValueOrDefault()),
                    OriginalPrice = p.Amount.ParseToIntOrDefault(),
                    SellingPrice = p.SellingPrice.GetValueOrDefault(),
                    RtmtqCountry = p.RmtqCountry.GetValueOrDefault(),
                    Resource = p.Resource.GetValueOrDefault(),
                    Description = p.Description.GetValueOrDefault(),
                    DescriptionData = p.DescriptionData.GetValueOrDefault(),
                    TotalQuantity = p.TotalQuanlity.GetValueOrDefault(),
                    TotalQuantitySold = _externalRequests.Count(t => t.PackageId.ToLower() == p.PackageName.ToLower()),
                    Status = p.Status
                })
                .ToListAsync();

                var packageGroups = new List<PackageGroup>();
                if (_nationPackages.Any())
                    packageGroups.Add(new PackageGroup { Category = "Gói data quốc gia", Packages = _nationPackages });
                if (_areaPackages.Any())
                    packageGroups.Add(new PackageGroup { Category = "Gói data khu vực", Packages = _areaPackages });
                if (_internationalPackages.Any())
                    packageGroups.Add(new PackageGroup { Category = "Gói data quốc tế", Packages = _internationalPackages });

                return new DataPackageResponse()
                {
                    StatusCode = "000",
                    Message = "success",
                    Data = packageGroups
                };
            }

            return new DataPackageResponse()
            {
                StatusCode = "001",
                Message = "lỗi khu vực nhập gói cước không đúng",
                Data = null
            };
        }
    }
}
