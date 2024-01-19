using DotNetLab12.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DotNetLab12.Data
{
    public class ShopDbContext:IdentityDbContext
    {

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Article> Article { get; set; }

        public DbSet<BasketItem> BasketItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // create tables for Identity
            // modelBuilder.Seed();
        }
    }
}
