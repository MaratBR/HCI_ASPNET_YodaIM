using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YodaIM.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Secret));
        }
    }
}
