using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotNetLab12.Data;
using DotNetLab12.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Authorization;

namespace DotNetLab12.Pages.Shops
{
    [Authorize(Policy = "DisableAdminRole")]
    public class ShopModel : PageModel
    {
        private readonly DotNetLab12.Data.ShopDbContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;
        private static string DEFAULT_IMAGE = "default_image.jpg";

        public ShopModel(DotNetLab12.Data.ShopDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IList<Article> Article { get;set; }


        IList<SelectListItem> getCategories(string? Category)
        {
            var shopDbContext = _context.Article.Include(a => a.Category);
            List<SelectListItem> categories = new List<SelectListItem>();
            categories.Add(new SelectListItem { Text = "All", Value = "-1" });

            foreach (var category in _context.Category)
            {
                categories.Add(new SelectListItem { Text = category.Name, Value = category.CategoryId.ToString() });
            }

            if (Category != null)
            {
                categories.Find(c => c.Value == Category).Selected = true;
            }
            
            return categories;
        }

        public async Task OnGetAsync(string? Category)
        {
            ClearCookies();
            Category = Category == null ? "-1" : Category;

            ViewData["Category"] = getCategories(Category);

            Article = await _context.Article
                 .Include(a => a.Category).ToListAsync();

            foreach (var art in Article)
            {
                art.PictureName = art.PictureName == null ? DEFAULT_IMAGE : art.PictureName;
            }
            if (Category != "-1")
            {
                Article = Article.Where(a => a.CategoryId.ToString() == Category).ToList();
            }
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
            return RedirectToPage();
        }

        
    }
}
