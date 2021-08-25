using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.WebCore.AuthorizationExtend.Requirement
{
    /// <summary>
    /// DateOfBirth---支持传入
    /// </summary>
    public class DateOfBirthRequirement : IAuthorizationRequirement
    {
    }

    public class DateOfBirthRequirementHandler : AuthorizationHandler<DateOfBirthRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DateOfBirthRequirement requirement)
        {
            if (context.User != null && context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                context.Succeed(requirement);//也可以比较具体规则
            }
            return Task.CompletedTask;
        }
    }
}
