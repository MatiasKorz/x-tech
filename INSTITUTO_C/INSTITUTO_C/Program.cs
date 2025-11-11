using INSTITUTO_C.Data; // tu DbContext
using INSTITUTO_C.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace INSTITUTO_C
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var usingInMemory = false;

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var hostname = Environment.GetEnvironmentVariable("COMPUTERNAME") ?? Environment.GetEnvironmentVariable("HOSTNAME");

            if (hostname?.Equals("SEIDOR106", StringComparison.OrdinalIgnoreCase) == true)
            {
                // Ejecutar logica para Ezequiel
                builder.Services.AddDbContext<InstitutoContext>(options =>
                    options.UseInMemoryDatabase("InstitutoDb"));

                usingInMemory = true;
            }
            else
            {

                //builder.Services.AddDbContext<InstitutoContext>(options => options.UseInMemoryDatabase("InstitutoDb"));
                builder.Services.AddDbContext<InstitutoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("InstitutoDBCS")));
            }

            //identity

            builder.Services.AddIdentity<Persona, IdentityRole<int>>().AddEntityFrameworkStores<InstitutoContext>();

            builder.Services.Configure<IdentityOptions>(opciones =>
            {
                opciones.Password.RequireNonAlphanumeric = false;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequireUppercase = false;
                opciones.Password.RequireDigit = false;
                opciones.Password.RequiredLength = 5;

                //Password1!
            }

            );

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                opciones =>
                {
                    opciones.LoginPath = "/Account/Iniciarsesion";
                    opciones.AccessDeniedPath = "/Account/Accesodenegado";
                    opciones.Cookie.Name = "IdentidadInstitutoApp";

                });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<InstitutoContext>();

                if (usingInMemory)
                {
                    // Para InMemory Database - NO usar Migrate()
                    dbContext.Database.EnsureCreated();
                }
                else
                {
                    // Para SQL Server - usar Migrate()
                    dbContext.Database.Migrate();
                }
            }

                app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //siempre auten antes de autor
            app.UseAuthorization();  //eso es mucho muy importante

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
