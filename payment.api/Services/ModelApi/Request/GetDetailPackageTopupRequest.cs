using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;
public class GetDetailPackageTopupRequest : IApiInput
    {
        [FromRoute(Name = "package_name"), JsonPropertyName("package_name")]
        public string package_name { get; set; }
    }
