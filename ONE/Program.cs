using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ONE.Data;
using ONE.Models;
using ONE.Services;

namespace ONE
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            #region AddDbCntext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AddDbContext>(option =>
               option.UseSqlServer(connectionString));
            #endregion
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<YouTubeServiceApi>();
            builder.Services.AddScoped<UserManager<ApplicationUser>>();

            builder.Services.AddHttpClient();

            // add identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opeion =>
            {
                opeion.Password.RequiredLength = 4;
                opeion.Password.RequireDigit = false;
                opeion.Password.RequireNonAlphanumeric = false;

            })
             .AddEntityFrameworkStores<AddDbContext>();
            builder.Services.AddScoped<FavoriteService>();

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
