using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alert.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Alert.API.Data;
using Alert.API.Repositories;
using Alert.API.Services.SignalR;
using Microsoft.AspNetCore.Cors.Infrastructure;


namespace Alert.API
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
            var connection = @"Server=LAPTOP-2I2MR4H0;Database=LunaAlerts;Trusted_Connection=True;";
            services.AddDbContext<AlertDbContext>(options => options.UseSqlServer(connection));

            // Add framework services.
            services.AddMvc();
            services.AddSignalR();

            // Register application services.
            services.AddScoped<IRepository<Models.Alert>, AlertsRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        
                );

            app.UseSignalR(routes =>
            {
                routes.MapHub<AlertHub>("/alerthub");
                
            });

            app.UseMvc();
        }
    }
}
