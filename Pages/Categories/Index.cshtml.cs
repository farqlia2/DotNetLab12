using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotNetLab12.Data;
using DotNetLab12.Models;
using Microsoft.AspNetCore.Authorization;

namespace DotNetLab12.Pages.Categories
{
    // [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly DotNetLab12.Data.ShopDbContext _context;

        public IndexModel(DotNetLab12.Data.ShopDbContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; }

        public async Task OnGetAsync()
        {
            Category = await _context.Category.ToListAsync();
        }
    }
}
