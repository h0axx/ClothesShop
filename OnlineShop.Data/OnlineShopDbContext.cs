using Microsoft.EntityFrameworkCore;
using OnlineShop.Core;

namespace OnlineShop.Data
{
    public class OnlineShopDbContext : DbContext
    {
        public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options)
            :base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
