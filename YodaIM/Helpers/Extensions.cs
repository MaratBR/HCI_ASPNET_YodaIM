using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;
using YodaIM.Services;
using YodaIM.Services.Database;
using YodaIM.Services.Security;
using YodaIM.Settings;

namespace YodaIM.Helpers
{
    static class Extensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services.AddScoped<ITokenService, TokenService>();
        }


        public static async Task<User> FindByUserNameOrEmail(this UserManager<User> manager, string login)
        {
            return await manager.Users.Where(u => u.Email == login || u.UserName == login).SingleOrDefaultAsync();
        }

        public static JwtSettings GetJwtConfiguration(this IConfiguration configuration)
        {
            return configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        }
    }
}
