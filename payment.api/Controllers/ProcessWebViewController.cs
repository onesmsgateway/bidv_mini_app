using MediatR;
using Microsoft.AspNetCore.Mvc;
using payment.api.Services.ModelApi.Request;
using payment.api.Services.ModelApi.Response;
using payment.api.Validator;
using PaymentPackageTelco.api.Attributes;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    [Produces("application/json")]
    public partial class ProcessWebViewController : ControllerBase
    {
        public ProcessWebViewController(IMediator mediator) => _mediator = mediator;

        private readonly IMediator _mediator;
        /// <summary>
        /// (1) Xử lý yêu cầu từ BIDV, tạo phiên giao dịch và trả về URL webview.
        /// </summary>
        /// <param name="bRequest">Thông tin giao dịch từ BIDV.</param>
        /// <returns>Đối tượng WebviewResponse chứa URL webview.</returns>
        [HttpPost("get-url")]
        public async Task<IApiResponse> GetUrl([FromHeader] WebViewHeaderRequest hRequest, [FromBody] WebViewBodyRequest bRequest, CancellationToken cancellation)
        {
            if(!bRequest.IsValidChecksumWith(hRequest))
            {
                return new WebViewResponse { Url = "", ResultCode = "002", ResultDesc = "checksum không hợp lệ"};
            }

            return await _mediator.Send(new WebViewRequest() { WebViewHeaderRequest = hRequest, WebViewBodyRequest = bRequest }, cancellation);
        }
    }
}
