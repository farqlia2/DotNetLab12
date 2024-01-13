using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotNetLab12.Data;
using DotNetLab12.Models;

namespace DotNetLab12.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly DotNetLab12.Data.ShopDbContext _context;

        public DetailsModel(DotNetLab12.Data.ShopDbContext context)
        {
            _context = context;
        }

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
    }
}
