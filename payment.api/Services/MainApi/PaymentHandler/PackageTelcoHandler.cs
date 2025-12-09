using MediatR;
using Microsoft.EntityFrameworkCore;
using payment.entity;
using payment.entity.BusinessEntities;
using PaymentPackage.api.Services.ModelApi.Request;
using PaymentPackageTelco.api.Services.ModelApi.Response;
using PaymentPackageTelco.entity.CommonModel;
using System.Net;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackage.api.Services.MainApi.PaymentHandler
{
    public class PackageTelcoHandler : IRequestHandler<SearchPackageRequest, IApiResponse>
    {
        private readonly PaymentPackageTelcoDbContext _dbContext;
        public PackageTelcoHandler(PaymentPackageTelcoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(SearchPackageRequest request, CancellationToken cancellationToken)
        {

            IQueryable<PackageTelco> _packageTecos = _dbContext.PackageTelcos.AsQueryable();

            if (request.IsCombo.HasValue)
            {
                _packageTecos = _packageTecos.Where(p => p.IsCombo == ((request.IsCombo == true) ? 'y' : 'n'));
            }

            if (request.IsDomesticPackage.HasValue)
            {
                _packageTecos = _packageTecos.Where(p => p.IsDomesticPackage == ((request.IsDomesticPackage == true) ? 'y' : 'n'));
            }

            if (request.IsFlashSale.HasValue)
            {
                _packageTecos = _packageTecos.Where(p => p.IsFlashSale == ((request.IsFlashSale == true) ? 'y' : 'n'));
            }

            if (request.DateUse.HasValue)
            {
                var durationCycle = request.DateUse.Value;
                switch (request.DateUse.Value)
                {
                    case DurationCycle.Time:
                        _packageTecos = _packageTecos.Where(p => p.DateUse.HasValue && p.DateUse.Value <= 1);
                        break;
                    case DurationCycle.OneToSevenDays:
                        _packageTecos = _packageTecos.Where(p => p.DateUse.HasValue && p.DateUse.Value > 1 && p.DateUse.Value <= 7);
                        break;
                    case DurationCycle.SevenToFiveteenDays:
                        _packageTecos = _packageTecos.Where(p => p.DateUse.HasValue && p.DateUse.Value > 7 && p.DateUse.Value <= 15);
                        break;
                    case DurationCycle.FiveteenToThirtyDays:
                        _packageTecos = _packageTecos.Where(p => p.DateUse.HasValue && p.DateUse.Value > 15 && p.DateUse.Value <= 30);
                        break;
                    default: break;
                }
            }

            if (request.DataVolume.HasValue)
            {
                var durationCycle = request.DataVolume.Value;
                switch (durationCycle)
                {
                    case DataVolume.SmallThan1GB:
                        _packageTecos = _packageTecos.Where(p => p.Data < 1024);
                        break;
                    case DataVolume.From1GbTo2Gb:
                        _packageTecos = _packageTecos.Where(p => p.Data >= 1024 && p.Data < 2048);
                        break;
                    case DataVolume.From2GbTo4Gb:
                        _packageTecos = _packageTecos.Where(p => p.Data >= 2048 && p.Data < 4096);
                        break;
                    case DataVolume.From4GbTo8Gb:
                        _packageTecos = _packageTecos.Where(p => p.Data >= 4096 && p.Data < 8192);
                        break;
                    case DataVolume.MoreThan8Gb:
                        _packageTecos = _packageTecos.Where(p => p.Data >= 8192);
                        break;
                    default: break;
                }
            }

            if (request.SocialNet.HasValue)
            {
                _packageTecos = _packageTecos.Where(p => p.DataSocialNetwork.Contains(request.SocialNet.Value.ToString().ToLower()));  
            }
            
            var skipCount = (request.PageIndex - 1) * request.PageSize;
            if (skipCount < 0) skipCount = 0;

            var totalCount = await _packageTecos.CountAsync();
            var packages = await _packageTecos
                .Skip(skipCount)
                .Take(request.PageSize)
                .Select(p => new PackageResponse
                  { 
                    PackageName = p.PackageName ?? "",
                    Telco = p.Telco ?? "",
                    Data = p.Data ?? 0M,
                    Amount = p.Amount ?? "",
                    DateUse = p.DateUse ?? 0,
                    IsCombo = p.IsCombo.Equals('n').ToString(),
                    DataSocialNetwork = p.DataSocialNetwork,
                    IsDomesticPackage = p.IsDomesticPackage.Equals('y').ToString(),
                    IsFlashSale = p.IsFlashSale.Equals('y').ToString(),
                    FromTimeFlashSale = p.FromTimeFlashSale,
                    ToTimeFlashSale = p.ToTimeFlashSale,
                    Resouce = p.Resouce,
                    RmtqCountry = p.RmtqCountry,
                    Note = p.Note
                }).ToListAsync();

            return new SearchPackageResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Success!",
                Data = packages,
                PageIndex = request.PageIndex,
                TotalCount = totalCount,
            };
        }
    }
}
