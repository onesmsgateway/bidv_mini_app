using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPackageTelco.api.Services.ModelApi.Request;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TokenController : ControllerBase
    {
        public TokenController(IMediator mediator) => _mediator = mediator;
        private readonly IMediator _mediator;
        [AllowAnonymous]
        [HttpPost("get-token")]
        public async Task<IApiResponse> GenerateToken(CustomerAccountInfoRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
