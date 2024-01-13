using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DotNetLab12.Data;
using DotNetLab12.Models;
using DotNetLab12.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DotNetLab12.Pages.Articles
{
    public class CreateModel : PageModel
    {
        private readonly DotNetLab12.Data.ShopDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private static string DEFAULT_IMAGE = "default_image.jpg";

        public CreateModel(DotNetLab12.Data.ShopDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Name");
            return Page();
        }

        [BindProperty]
        public ArticleViewModel ArticleViewModel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string? fileName = saveFile(ArticleViewModel.Picture);
            Article article = new Article()
            {
                ArticleId = ArticleViewModel.ArticleId,
                Name = ArticleViewModel.Name,
                Price = ArticleViewModel.Price,
                PictureName = fileName,
                CategoryId = ArticleViewModel.CategoryId
            };

            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private string? saveFile(IFormFile? formFile)
        {
            if (formFile != null)
            {
                string name = DateTime.Now.ToString("ddMMyyyyhhmmss") + formFile.FileName;
                string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "upload", name);

                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    formFile.CopyTo(fs);
                }
                return name;
            }
            return null;
        }

    }


}


