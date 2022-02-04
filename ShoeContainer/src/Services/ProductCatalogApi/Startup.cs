using System;
using System.Collections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductCatalogApi.Data;

namespace ProductCatalogApi
{
    public class Startup
    {
      
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public  IWebHostEnvironment Environment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services )
        {
            services.Configure<CatalogOptions>(Configuration.GetSection(CatalogOptions.Catalog));
            
            if (Environment.IsDevelopment())
            {
                services.AddDbContext<CatalogContext>(options => options.UseSqlServer(Configuration["ConnectionString"]));
            }
            else
            {
                services.AddDbContext<CatalogContext>(options => options.UseSqlServer(System.Environment.GetEnvironmentVariable("DataBaseConnection")));
            }
            
            // services.AddDbContext<CatalogContext>(options =>
            //     options.UseSqlServer(Configuration["ConnectionString"]));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "ProductCatalogApi",
                        Version = "v1"
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "ProductCatalogApi v1"));
                
                // TO DO
                // figure out how to run https localhost on docker and then move to main logic
                app.UseHttpsRedirection();
            }

           
            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}