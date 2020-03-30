using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YodaIM.Helpers;
using YodaIM.Models;

namespace YodaIM.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly Context context;

        public TokenService(IConfiguration configuration, Context context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        public JwtSecurityToken CreateIdentityToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var jwtCfg = configuration.GetJwtConfiguration();
            var now = DateTime.UtcNow;
            var expiresAt = now.AddDays(1);
            var creds = new SigningCredentials(jwtCfg.GetSecurityKey(), SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: jwtCfg.Issuer,
                notBefore: now,
                expires: expiresAt,
                claims: claims,
                signingCredentials: creds,
                audience: jwtCfg.Audience
                );
        }

        public async Task<Guid> CreateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken(user, TimeSpan.FromDays(configuration.GetJwtConfiguration().RefreshLifetime));
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();
            return refreshToken.Id;
        }

        public string Strigify(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
