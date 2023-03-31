using InmobiliariaAlbornoz.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>//el sitio web valida con cookie
                {
                    options.LoginPath = "/Usuarios/Login";
                    options.LogoutPath = "/Usuarios/Logout";
                    options.AccessDeniedPath = "/Home/Denied";
                })
                .AddJwtBearer(options =>//la api web valida con token
				{
					options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = Configuration["TokenAuthentication:Issuer"],
						ValidAudience = Configuration["TokenAuthentication:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
							Configuration["TokenAuthentication:SecretKey"])),
					};

                   options.Events = new JwtBearerEvents
					{
						OnMessageReceived = context =>
						{	
							var accessToken = context.Request.Query["access_token"];
							var path = context.HttpContext.Request.Path;
							if (!string.IsNullOrEmpty(accessToken) &&
								path.StartsWithSegments("/API/Propietarios/mail"))
							{
								context.Token = accessToken;
							}
							return Task.CompletedTask;
						}
					};
				});   

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
                options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "SuperAdministrador"));
            });

            services.AddControllersWithViews();
            /* PARA MySql - usando Pomelo */
			services.AddDbContext<DataContext>(
				options => options.UseMySql(
					Configuration["ConnectionStrings:databaseMySql"],
					ServerVersion.AutoDetect(Configuration["ConnectionStrings:databaseMySql"])
				)
			);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "SearchInquilinoContratos",
                    pattern: "Pagos/Inquilino/{dni}", new { controller = "Pagos", action = "Inquilino" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
