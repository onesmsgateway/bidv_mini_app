using Microsoft.AspNetCore.Mvc;
using PaymentPackageTelco.api.Common;
using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackage.api.Services.ModelApi.Request
{
    public class FilterAreaPackageTopupRequest : IFilterable, IApiInput
    {
        [FromQuery(Name ="area_type"), JsonPropertyName("area_type")]
        public AreaType? AreaType { get; set; }
        [FromQuery(Name ="date_use"), JsonPropertyName("date_use")]
        public DateUse? DateUse { get; set; }
        [FromQuery(Name = "date_volume"), JsonPropertyName("date_volume")]
        public DataVolume? DataVolume { get; set; }
        [FromQuery(Name = "social_net"), JsonPropertyName("social_net")]
        public SocialNet? SocialNet { get; set; }
    }
}
