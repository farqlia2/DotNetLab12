using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DotNetLab12.Areas.Identity
{
    public class NonAdminRequirementHandler : AuthorizationHandler<NonAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NonAdminRequirement requirement)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Fail();
            } else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
