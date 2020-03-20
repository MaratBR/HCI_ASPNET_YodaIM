using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using YodaIM.Chat;
using YodaIM.Helpers;
using YodaIM.Models;
using YodaIM.Settings;

namespace YodaIM
{
    public class Startup
    {
        private readonly IWebHostEnvironment Env;
        public Startup(IConfiguration configuration, IWebHostEnvironment appEnv)
        {
            Configuration = configuration;
            Env = appEnv;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string connection;
            Action<DbContextOptionsBuilder> builder;

            if (!Env.IsDevelopment())
            {
                connection = Configuration.GetConnectionString("sqlserver");
                builder = opts => opts.UseSqlServer(connection);
            }
            else
            {
                connection = Configuration.GetConnectionString("sqlite");
                builder = opts => opts.UseSqlite(connection);
            }

            services.AddApplicationServices();
            services.AddDbContext<Context>(builder);

            {
                var section = Configuration.GetSection(nameof(JwtSettings));
                var jwtSettings = section.Get<JwtSettings>();

                services
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(x =>
                    {
                        x.IncludeErrorDetails = true;
                        x.ClaimsIssuer = jwtSettings.Issuer;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateActor = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = jwtSettings.GetSecurityKey(),
                            NameClaimType = ClaimTypes.NameIdentifier
                        };
                    });
            }

            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();
            });

            services.AddControllers();
            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Context context)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<YODAChat>("/api/chat");
                endpoints.MapControllers();
            });



            context.Database.EnsureCreated();
        }
    }
}