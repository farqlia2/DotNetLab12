using DotNetLab12.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DotNetLab12.Data
{
    public class ShopDbContext:DbContext
    {

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Article> Article { get; set; }

        public DbSet<BasketItem> BasketItem { get; set; }


    }
}
