using Microsoft.EntityFrameworkCore;
using WebAppShop.Core.Domain;


namespace WebAppShop.Data
{
    //teha sellest classist DbContext, et saaks andmebaasi kasutada
    public class WebAppShopContext : DbContext
    {
        public WebAppShopContext(DbContextOptions<WebAppShopContext> options)
            : base(options)
        {
        }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<Kindergarten> Kindergartens { get; set; }
    }
}
