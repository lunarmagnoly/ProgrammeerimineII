using Microsoft.EntityFrameworkCore;
using System.Xml;
using WebAppShop.Core.Domain;


namespace WebAppShop.Data
{
    //nimetasime classi WebAppShop.Context, mis pärib DbContext klassi
    public class WebAppShopContext : DbContext
    {
        //tegime konteksti, mis pärib DbContext klassi
        public WebAppShopContext(DbContextOptions<WebAppShopContext > options)
            : base(options) { }


        //vaja lisada dbSet, mis on seotud meie domain klassiga Spaceship
        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }        
        public DbSet<FileToDatabase> FileToDatabases { get; set; }
    }
}