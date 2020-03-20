using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services
{
    interface IAuth
    {
        Task<User> GetUser(ClaimsPrincipal principal);
    }
}
