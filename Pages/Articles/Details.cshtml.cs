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

namespace DotNetLab12.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private readonly DotNetLab12.Data.ShopDbContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;
        private static string DEFAULT_IMAGE = "default_image.jpg";

        public DetailsModel(DotNetLab12.Data.ShopDbContext context)
        {
            _context = context;
        }

        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article
                .Include(a => a.Category).FirstOrDefaultAsync(m => m.ArticleId == id);
            

            if (Article == null)
            {
                return NotFound();
            }
            Article.PictureName = (Article.PictureName == null) ? DEFAULT_IMAGE : Article.PictureName;

            return Page();
        }
    }
}
