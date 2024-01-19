using Microsoft.AspNetCore.Authorization;

namespace DotNetLab12.Areas.Identity
{
    public class NonAdminRequirement : IAuthorizationRequirement
    {
    }
}
