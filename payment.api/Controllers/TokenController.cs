using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using payment.api.AppSettings;
using payment.entity.DbEntities;
using PaymentPackageTelco.api.Services.ModelApi.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TokenController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("get-token")]
        public async Task<IApiResponse> GenerateToken(CustomerAccountInfoRequest request)
        {
            return await GenerateToken(request);
        }

        private async Task<string> GenerateToken(CustomerAccountInfo customerAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConst.jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("id", customerAccount.CustomerId),
                new Claim("username", customerAccount.Fullname),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddHours(AppConst.partnerJwtExpired), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
