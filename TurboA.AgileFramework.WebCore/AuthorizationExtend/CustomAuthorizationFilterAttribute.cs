using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.WebCore.AuthorizationExtend
{
    /// <summary>
    /// IAuthorizationFilter：请求刚进入MVC流程
    /// OnAuthorization来完成登录校验--以及权限检查
    /// </summary>
    public class CustomAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(item => item is AllowAnonymousAttribute))
            {
                return;//匿名 不检查
            }
            if (context.Filters.Any(f => f is IAllowAnonymousFilter))
            {
                return;//匿名 不检查  
            }

            string sUser = context.HttpContext.Request.Cookies["CurrentUser"];

            if (sUser == null)
            {
                context.Result = new RedirectResult("~/Home/Index");
            }
            else
            {
                //还应该检查下权限
                return;
            }
        }
    }

    public class CustomAsyncAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
