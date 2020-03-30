using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YodaIM.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string SecretKeyFile { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int RefreshLifetime { get; set; } = 30;

        private bool secretKeyFileLoaded = false;

        public SymmetricSecurityKey GetSecurityKey()
        {
            if (!string.IsNullOrWhiteSpace(SecretKeyFile) && !secretKeyFileLoaded)
            {
                Secret = File.ReadAllText(SecretKeyFile);
                secretKeyFileLoaded = true;
            }
            return new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Secret));
        }
    }
}
