using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.ModelApi.Request
{
    public class GetDetailPackageComboRequest : IApiInput
    {
        [FromRoute(Name = "package_name"), JsonPropertyName("package_name")]
        public string package_name { get; set; }
    }
}
