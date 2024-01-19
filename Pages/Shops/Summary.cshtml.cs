using DotNetLab12.Data;
using DotNetLab12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DotNetLab12.Pages.Shops
{
    [Authorize(Roles = "User", Policy = "DisableAdminRole")]
    public class SummaryModel : PageModel
    {
        private readonly ShopDbContext _context;
        private string DEFAULT_IMAGE = "default_image.jpg";

        public SummaryModel(ShopDbContext context)
        {
            _context = context;
        }

        public IList<BasketItem> BasketItems { get; set; }

        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public PaymentMethod PaymentMethod { get; set; }

        public IActionResult OnPost()
        {
            AddCookieValue("Address", Address);
            AddCookieValue("Name", Name);
            AddCookieValue("PaymentMethod", PaymentMethod.ToString());
            return RedirectToPage("PaymentConfirmation");
        }

        public void AddCookieValue(string name, string value, int numberOfDays = 7)
        {

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(numberOfDays);
            Response.Cookies.Append(name,value, option);
          
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["PaymentMethod"] = PaymentMethod.GetValues<PaymentMethod>();
            BasketItems = await getBasketItems();

            if (BasketItems.Count == 0)
            {
                return RedirectToPage("EmptyBasket");
            }
            return Page();
        }

        public async Task<List<BasketItem>> getBasketItems()
        {
            var shopDbContext = _context.Article.Include(a => a.Category);
            var articles = await shopDbContext.ToListAsync();
            articles.ForEach(a => { a.PictureName = a.PictureName == null ? DEFAULT_IMAGE : a.PictureName; });

            var basketItems = new List<BasketItem>();

            double summary = 0;

            foreach (var item in articles)
            {
                if (Request.Cookies.ContainsKey(item.ArticleId.ToString()))
                {
                    var itemCount = int.Parse(Request.Cookies[item.ArticleId.ToString()]);
                    double itemValue = itemCount * item.Price;
                    var basketItem = new BasketItem(item.ArticleId, itemCount);
                    basketItem.Article = item;
                    basketItems.Add(basketItem);
                    summary += itemValue;
                }
            }

            ViewData["summary"] = String.Format("{0:0.00}", summary);


            return basketItems;
        }
    }
}
