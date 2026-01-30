using System.Net;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.ModelApi.Response
{
    public class SearchPackageResponse : IApiResponse
    {
      public HttpStatusCode StatusCode { get; set; }
      public string Message { get; set; }
      public object Data { get; set; }
      public int PageIndex { get; set; }
      public int TotalCount { get; set; }
    }
}
