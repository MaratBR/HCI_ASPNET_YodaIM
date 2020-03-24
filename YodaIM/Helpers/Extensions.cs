using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YodaIM.Chat;
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
            return services
                .AddSingleton<IChatHandler, ChatHandler>()
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<IMessageService, MessageService>()
                .AddScoped<IRoomService, RoomService>();
        }


        public static async Task<User> FindByUserNameOrEmail(this UserManager<User> manager, string login)
        {
            return await manager.Users.Where(u => u.Email == login || u.UserName == login).SingleOrDefaultAsync();
        }

        public static JwtSettings GetJwtConfiguration(this IConfiguration configuration)
        {
            return configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        }

        public static async Task<User> GetUserAsyncOrFail(this UserManager<User> manager, ClaimsPrincipal principal) => (await manager.GetUserAsync(principal)) ?? throw new ApplicationException("User hasn't been found in the database");
    }
}
