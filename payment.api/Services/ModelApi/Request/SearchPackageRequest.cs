using PaymentPackageTelco.entity.CommonModel;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackage.api.Services.ModelApi.Request
{
    public class SearchPackageRequest : ISearchModel, IFilterModel, IApiInput
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 0;

        public bool? IsCombo { get; set; }
        public bool? IsDomesticPackage { get; set; } //trongnuoc, quocte
        public bool? IsFlashSale { get; set; }

        public DurationCycle? DateUse { get; set; }
        public DataVolume? DataVolume { get; set; }
        public SocialNet? SocialNet { get; set; }
    }
}
