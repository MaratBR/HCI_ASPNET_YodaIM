using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YodaIM.Helpers;
using YodaIM.Models;

namespace YodaIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly Context context;
        private readonly UserManager<User> userManager;

        public UsersController(Context context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public class UsersResponse
        {
            public List<User> Users { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<UsersResponse>> GetUsers()
        {
            return new UsersResponse
            {
                Users = await context.Users.ToListAsync()
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser([FromRoute] int id)
        {
            return await context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        [HttpGet("name/{userName}")]
        public async Task<ActionResult<User>> GetUserByUsername([FromRoute] string userName)
        {
            return await context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }

        public class UpdateUserRequest
        {
            public string UserName { get; set; }

            public string Status { get; set; }

            public byte? Gender { get; set; }

            public void Apply(User user)
            {
                if (UserName != null)
                {
                    user.UserName = UserName;
                    user.NormalizedUserName = UserName.ToUpper();
                }

                if (Gender != null)
                    user.Gender = (byte)Gender;

                if (Status != null)
                    user.Status = Status;
            }
        }

        [HttpPut("you")]
        public async Task<ActionResult> UpdateYourself([FromBody] UpdateUserRequest r)
        {
            var user = await userManager.GetUserAsyncOrFail(User);
            return await UpdateUser(r, user);
        }

        private async Task<ActionResult> UpdateUser(UpdateUserRequest r, User user)
        {

            if (user == null)
                return NotFound();

            r.Apply(user);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}