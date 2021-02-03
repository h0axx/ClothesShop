using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Core;

namespace OnlineShop.Data
{
    public class OnlineShopDbContext : IdentityDbContext
    {
        public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options)
            :base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<IdentityUser> IdentityUsers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
    }
}
