using MediatR;
using Microsoft.AspNetCore.Mvc;
using payment.api.Services.ModelApi.Request;
using payment.api.Services.ModelApi.Response;
using payment.api.Validator;
using PaymentPackageTelco.api.Attributes;
using PaymentPackageTelco.api.Validator;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace payment.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    [Produces("application/json")]
    public class PaymentController : ControllerBase
    {
        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;
        /// <summary>
        /// (3) API nhận sự kiện thanh toán (Webhook) từ NCCDV.
        /// Endpoint: POST /api/bidv/notify-payment-event
        /// </summary>
        /// <param name="request">Payload chứa thông tin thanh toán từ NCCDV.</param>
        /// <returns>HTTP 200 OK với Response Code "00" nếu xử lý thành công.</returns>
        [HttpPost("init")]
        public async Task<IApiResponse> Init([FromBody] BillBodyRequest request)
        {
            return await _mediator.Send(request);
        }

        /// <summary>
        /// (3) API nhận sự kiện thanh toán (Webhook) từ NCCDV.
        /// Endpoint: POST /api/bidv/notify-payment-event
        /// </summary>
        /// <param name="request">Payload chứa thông tin thanh toán từ NCCDV.</param>
        /// <returns>HTTP 200 OK với Response Code "00" nếu xử lý thành công.</returns>
        [HttpPost("get-bill")]
        public async Task<IApiResponse> GetBill([FromBody] GetBillBodyRequest request)
        {
            if (!request.IsValidChecksum())
            {
                return new WebViewResponse { Url = "", ResultCode = "002", ResultDesc = "checksum không hợp lệ" };
            }

            return await _mediator.Send(request);
        }

        //tailieu: bidv_tthd_taikhoandinhdan_thuhoSMB
        /// <summary>
        /// (5) API tiếp nhận thông báo giao dịch thành công từ BIDV, IPN
        /// </summary>
        [HttpPost("pay-bill")]  //tailieu: bidv_tthd_taikhoandinhdan_thuhoSMB
        public async Task<IApiResponse> PayBill([FromBody] PayBillRequest request)
        {
            if (!request.IsValidChecksum())
            {
                return new WebViewResponse { Url = "", ResultCode = "002", ResultDesc = "checksum không hợp lệ" };
            }

            return await _mediator.Send(request);
        }

        [HttpPost("inquiry-pay-bill-by-package-id")]
        public async Task<IApiResponse> inquiryPayBill([FromBody] SearchPaybillRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
