using DotNetLab12.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DotNetLab12.Pages.Shops
{

    [Authorize(Roles = "User", Policy = "DisableAdminRole")]
    public class PaymentConfirmationModel : PageModel
    {

        private readonly ShopDbContext _context;

        public PaymentConfirmationModel(ShopDbContext context)
        {
            _context = context;
        }

        public string Address { get; set; }
        public string Name { get; set; }
        public string PaymentMethod { get; set; }

        public IActionResult OnGet()
        {
            Address = Request.Cookies["Address"];
            Name = Request.Cookies["Name"];
            PaymentMethod = Request.Cookies["PaymentMethod"];
            clearCookies();
            return Page();
        }

        public void clearCookies()
        {
            Response.Cookies.Delete("Address");
            Response.Cookies.Delete("Name");
            Response.Cookies.Delete("PaymentMethod");
            foreach (var cookie in Request.Cookies)
            {
                int value;
                if (cookie.Value == "0" || (Int32.TryParse(cookie.Key, out value) && _context.Article.Any(a => a.ArticleId == value)))
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
        }
    }
}
