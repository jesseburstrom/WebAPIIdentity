using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPIIdentity.Areas.Identity.Data;
using WebAPIIdentity.Models;

namespace WebAPIIdentity.Data
{
    public class WebAPIIdentityContext : IdentityDbContext<WebAPIIdentityUser>
    {
        public WebAPIIdentityContext(DbContextOptions<WebAPIIdentityContext> options)
            : base(options)
        {
        }
        public DbSet<Highscore> Highscores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<OrderList> OrderLists { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<CategoryCategory> CategoryCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
