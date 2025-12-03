using MediatR;
using Microsoft.AspNetCore.Mvc;
using payment.api.AppSettings;
using payment.api.Common;
using PaymentPackage.api.Services.ModelApi.Request;
using PaymentPackageTelco.api.Attributes;
using System.Text;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WebViewController : ControllerBase
    {
        public WebViewController(IMediator mediator) => _mediator = mediator;
        private readonly IMediator _mediator;

        [HttpPost("get-package-telco")]
        public async Task<IApiResponse> PackageTelco(GetPackageRequest request)
        {
            return await _mediator.Send(request);
        }

        /// <summary>
        /// (6)API cung cấp URL để đóng Webview và quay lại ứng dụng ngân hàng.
        /// (Thường được gọi bởi giao diện Webview của NCCDV khi nút 'Đóng' được nhấn)
        /// Endpoint: GET /nccdv/webview/get-close-url
        /// </summary>
        [HttpGet("close-url")]
        public async Task<IActionResult> CloseWebviewUrl()
        {
            return Ok(new
            {
                CloseUrl = Utils.Base64UrlEncode(Encoding.UTF8.GetBytes(AppConst.bidvNavigatorUrl)),
                Description = "Sử dụng URL này để điều hướng Webview về App BIDV."
            });
        }
    }
}
