using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services
{
    public interface ITokenService
    {
        public JwtSecurityToken CreateIdentityToken(User user);
        public Task<Guid> CreateRefreshToken(User user);
        public string Strigify(JwtSecurityToken token);
    }
}
