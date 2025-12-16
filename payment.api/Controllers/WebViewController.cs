using MediatR;
using Microsoft.AspNetCore.Mvc;
using payment.api.AppSettings;
using payment.api.Common;
using PaymentPackage.api.Services.ModelApi.Request;
using PaymentPackageTelco.api.Attributes;
using PaymentPackageTelco.api.Services.ModelApi.Request;
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

        //[HttpPost("get-package-telco")]
        //public async Task<IApiResponse> PackageTelco(SearchAreaPackageRequest request)
        //{
        //    return await _mediator.Send(request);
        //}

        [HttpGet("topup-data")]
        public async Task<IApiResponse> GetTopupData([FromQuery] FilterAreaPackageTopupRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("topup-data/{package_name}")]
        public async Task<IApiResponse> GetDetailPackageTopup([FromRoute] GetDetailPackageTopupRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("combo-data")]
        public async Task<IApiResponse> GetComboData([FromQuery] FilterComboDataPackageRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("combo-data/{package_name}")]
        public async Task<IApiResponse> GetDetailPackageCombo([FromRoute] GetDetailPackageComboRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("check-telco")]
        public async Task<IApiResponse> CheckTelco(CheckTelcoRequest request)
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
