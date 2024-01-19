using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetLab12.Pages.Shops
{
    [Authorize(Policy = "DisableAdminRole")]
    public class EmptyBasketModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
