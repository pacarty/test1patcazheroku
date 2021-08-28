using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whirl6.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Whirl6
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            env = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment env { get; }

        public string getConnectionString()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string herokudb = getConnectionString();

            services.AddControllers();

            // TODO: Right now when launching with IIS Express and Docker the environment thinks both of these are Development.
            // In the future, find a way that the environment can know whether we are in docker or iis, but for now we just have to switch
            // between connection strings. When this runs in heroku it knows its in Production so there is no issue there.
            if (env.IsDevelopment())
            {
                services.AddDbContext<TodoContext>(options => options.UseNpgsql(Configuration.GetConnectionString("LocalConnection")));
            }
            if (env.IsProduction())
            {
                services.AddDbContext<TodoContext>(options => options.UseNpgsql(herokudb));
            }
            
            
        }

        public void Configure(IApplicationBuilder app)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
