using Microsoft.AspNetCore.Authorization;
namespace BlazorAdmin.Annotations
{
    using Services;
    /// <summary>
    /// Authorize attribute that takes Roles from BlazorAdminService
    /// </summary>
    internal class AdminAuthorizeAttribute:AuthorizeAttribute
    {
        internal AdminAuthorizeAttribute():base()
        {
            Roles = BlazorAdminService.main.Roles;
        }
    }
}
