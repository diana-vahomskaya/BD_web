using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Workers.Account
{
    public class Handler : AuthorizationHandler<Claim>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
       Claim requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                if (context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value == requirement.Role) 
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
