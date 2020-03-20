using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services.Auth
{
    public class Auth : IAuth
    {
        public Auth(Context)
        {

        }

        public Task<User> GetUser(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }
    }
}
