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
using Microsoft.AspNetCore.Authorization;

namespace DotNetLab12.Pages.Articles
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly DotNetLab12.Data.ShopDbContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;
        private static string DEFAULT_IMAGE = "default_image.jpg";

        public IndexModel(DotNetLab12.Data.ShopDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IList<Article> Article { get;set; }

        public async Task OnGetAsync()
        {   
            Article = await _context.Article
                .Include(a => a.Category).ToListAsync();
            foreach (var art in Article)
            {
                art.PictureName = art.PictureName == null ? DEFAULT_IMAGE : art.PictureName;
            }
        }
    }
}
