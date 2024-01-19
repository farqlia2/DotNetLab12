using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DotNetLab12.Data;
using DotNetLab12.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace DotNetLab12.Pages.Articles
{
    [Authorize(Roles = "Admin")]
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
        public Article Article { get; set; }

        [BindProperty, Display(Name = "Product Image")]
        public IFormFile ProductImage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            saveFile();
            _context.Article.Add(Article);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

      

        private void saveFile()
        {
            if (ProductImage != null)
            {
                string name = DateTime.Now.ToString("ddMMyyyyhhmmss") + ProductImage.FileName;
                string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "upload", name);

                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    ProductImage.CopyTo(fs);
                    Article.PictureName = name;
                }
            }
        }

    }


}


