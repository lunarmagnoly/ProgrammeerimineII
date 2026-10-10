using Microsoft.EntityFrameworkCore;
using WebAppShop.ApplicationServices.Services;
using WebAppShop.Core.ServiceInterface;
using WebAppShop.Data;

namespace WebAppShop

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ISpaceshipServices, SpaceshipServices>();

            builder.Services.AddScoped<IFileServices, FileServices>();

            //selleks, et tuleb installida Microsoft.EntityFrameworkCore.SqlServer
            //ja Microsoft.EntityFrameworkCore.Tools NuGet paketid
            //kui installitud, siis viidata namespacesis Microsoft.EntityFrameworkCore-le
            builder.Services.AddDbContext<WebAppShopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
