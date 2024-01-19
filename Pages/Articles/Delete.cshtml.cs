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
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace DotNetLab12.Pages.Articles
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly DotNetLab12.Data.ShopDbContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;
        private static string DEFAULT_IMAGE = "default_image.jpg";

        public DeleteModel(DotNetLab12.Data.ShopDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article
                    .Include(a => a.Category).FirstOrDefaultAsync(m => m.ArticleId == id);
            Article.PictureName = (Article.PictureName == null) ? DEFAULT_IMAGE : Article.PictureName;

           
            if (Article == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article.FindAsync(id);

            if (Article != null)
            {
                removeImage(Article);
                _context.Article.Remove(Article);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        void removeImage(Article article)
        {
            if (article.PictureName != null)
            {
                System.IO.File.Delete(Path.Combine(_hostEnvironment.WebRootPath, "upload", article.PictureName));
            }
        }
    }
}
