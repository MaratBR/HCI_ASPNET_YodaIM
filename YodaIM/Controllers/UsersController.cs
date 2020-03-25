using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YodaIM.Models;

namespace YodaIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Context context;

        public UsersController(Context context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser([FromRoute] Guid id)
        {
            return await context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }
    }
}