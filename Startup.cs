using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ProjetoEscala.Context;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;


namespace ProjetoEscala
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
            services.AddControllersWithViews();

            //Conexao MySql para Entity Framework
            services.AddDbContext<Contexto>(options =>
                options.UseMySql(Configuration["ConexaoMySql:DefaultConnection"]));                

            //Sql Server
            //services.AddDbContext<Contexto>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
                
            //Conexao Sql Server LocalDB 
            //services.AddDbContext<Contexto>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("SqlServerLocalDB")));

            //Conexao SqLite (integrado)
            //services.AddDbContext<Contexto>(options =>
                //options.UseSqlite(Configuration["ConexaoSqLite:DefaultConnection"]));
            
            //Conexao Sql Server Compact (integrado)
            //services.AddDbContext<Contexto>(options =>
                //options.UseSqlite(Configuration["ConexaoSqlCompact:DefaultConnection"]));                

            //Conexao Firebird Embbed (integrado)
            //services.AddDbContext<Contexto>(options =>
                //options.UseFirebird(Configuration["ConexaoFirebird:DefaultConnection"]));

                        
            
          
            //Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Usuario", policy => policy.RequireClaim("Usuario"));
            });

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
            //session
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            
        }
    }
}
