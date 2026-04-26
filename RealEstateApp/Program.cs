using Application;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace RealEstateApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromMinutes(60);
                opt.Cookie.HttpOnly = true;
            });

            builder.Services.AddPersistenceLayerIoc(builder.Configuration);
            builder.Services.AddApplicationLayerIoc();
            builder.Services.AddIdentityLayerIocForWebApp(builder.Configuration);
            builder.Services.AddSharedLayerIoc(builder.Configuration);
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var identityContext = scope.ServiceProvider.GetRequiredService<Infrastructure.Identity.Contexts.IdentityContext>();
                identityContext.Database.Migrate();
                var appContext = scope.ServiceProvider.GetRequiredService<Infrastructure.Persistence.Contexts.RealEstateAppContext>();
                appContext.Database.Migrate();
            }

            await app.Services.RunIdentitySeedAsync();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}
