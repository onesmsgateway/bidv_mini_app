using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static payment.api.Services.ModelApi.ApiModelBase;

public class SearchPaybillRequest : IApiInput
{
    [FromBody, JsonPropertyName("package_id"), Required(ErrorMessage ="Thiếu thông tin PackageName")]
    public string PackageId { get; set; }

    [FromBody, JsonPropertyName("customer_id"), Required(ErrorMessage = "Thiếu thông tin CustomerId")]
    public string CustomerId { get; set; }
}