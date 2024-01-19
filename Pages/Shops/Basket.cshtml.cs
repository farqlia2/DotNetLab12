using DotNetLab12.Data;
using DotNetLab12.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetLab12.Pages.Shops
{
    [Authorize(Policy = "DisableAdminRole")]
    public class BasketModel : PageModel
    {
        private readonly ShopDbContext _context;
        private string DEFAULT_IMAGE = "default_image.jpg";

        public BasketModel(ShopDbContext context) {
            _context = context;
        }

        public IList<BasketItem> BasketItems { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            BasketItems = await getBasketItems();
            
            if (BasketItems.Count == 0)
            {
                return RedirectToPage("EmptyBasket");
            }
            
            return Page();
        }

        public IActionResult OnPostSubmit()
        {
            return RedirectToPage("Summary");
        }

        public async Task<List<BasketItem>> getBasketItems()
        {
            ClearCookies();
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

        public void ClearCookies()
        {
            foreach (var cookie in Request.Cookies)
            {
                int value;
                if (cookie.Value == "0" || (Int32.TryParse(cookie.Key, out value) && !_context.Article.Any(a => a.ArticleId == value)))
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
        }
        public void SetOrUpdateArticleCookie(int articleId, int addValue = 1, int numberOfDays = 7)
        {

            string artId = articleId.ToString();
            int newValue = addValue;
            if (Request.Cookies.ContainsKey(artId))
            {
                newValue += int.Parse(Request.Cookies[artId]);
                Response.Cookies.Delete(artId);
            }

            if (newValue > 0)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(numberOfDays);
                Response.Cookies.Append(artId, newValue.ToString(), option);
            }
          

        }

        public IActionResult OnPostAddToBasket(int id)
        {
            SetOrUpdateArticleCookie(id);
            return RedirectToPage("Basket");
        }

        public IActionResult OnPostRemoveFromBasket(int id)
        {
            SetOrUpdateArticleCookie(id, addValue: -1);
            return RedirectToPage("Basket");
        }


    }

}
