using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend_API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend_API
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
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));
            services.AddIdentity<User, UserRole>(options => { options.User.RequireUniqueEmail = true; }).
                AddEntityFrameworkStores<DBContext>();
            services.AddDbContext<DBContext>(cgf =>
            {
                cgf.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowMyOrigin"));
            });


            var secret = Configuration["JWTSettings:Secret"].ToString();
            var issuer = Configuration["JWTSettings:Issuer"].ToString();
            var audience = Configuration["JWTSettings:Audience"].ToString();

            services.AddAuthentication().AddCookie(
                cfg => cfg.SlidingExpiration = true).AddJwtBearer(
                cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowMyOrigin");
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
