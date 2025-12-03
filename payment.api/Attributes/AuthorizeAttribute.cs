using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using payment.api.AppSettings;
using PaymentPackageTelco.api.Common;

namespace PaymentPackageTelco.api.Attributes
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymousAttribute = false;

            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);
                foreach (var item in actionAttributes)
                {
                    if (item is AllowAnonymousAttribute)
                    {
                        hasAllowAnonymousAttribute = true;
                        break;
                    }
                }
            }

            if (!hasAllowAnonymousAttribute)
            {
                var _token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (!(RedisHelper.Get(AppConst.bidvCacheTokenKey) == _token))
                {
                    context.Result = new JsonResult(new { message = "Unauthorized: token's invalid" }) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }
            }
        }
    }
}
