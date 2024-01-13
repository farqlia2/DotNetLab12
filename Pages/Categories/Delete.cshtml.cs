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

namespace DotNetLab12.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly DotNetLab12.Data.ShopDbContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;

        public DeleteModel(DotNetLab12.Data.ShopDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Category.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (Category == null)
            {
                return NotFound();
            }


            return Page();
        }

        void removeAllArticles(IList<Article> articles)
        {
            foreach (Article art  in articles)
            {
                removeImage(art);
                _context.Article.Remove(art);
            }
        }

        void removeImage(Article article)
        {
            if (article.PictureName != null)
            {
                System.IO.File.Delete(Path.Combine(_hostEnvironment.WebRootPath, "upload", article.PictureName));
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Category.FindAsync(id);

            if (Category != null)
            {
                var articlesOfCategory = await _context.Article.Where(a => a.CategoryId == id).ToListAsync();
                removeAllArticles(articlesOfCategory);
                _context.Category.Remove(Category);
                await _context.SaveChangesAsync();
            }


            return RedirectToPage("./Index");
        }
    }
}
