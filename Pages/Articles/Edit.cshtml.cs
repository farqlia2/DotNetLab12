using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetLab12.Data;
using DotNetLab12.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace DotNetLab12.Pages.Articles
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly DotNetLab12.Data.ShopDbContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;
        private static string DEFAULT_IMAGE = "default_image.jpg";

        public EditModel(DotNetLab12.Data.ShopDbContext context, IWebHostEnvironment hostEnvironment)
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

            if (Article == null)
            {
                return NotFound();
            }
            Article.PictureName = (Article.PictureName == null) ? DEFAULT_IMAGE : Article.PictureName;
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.ArticleId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.ArticleId == id);
        }
    }
}
