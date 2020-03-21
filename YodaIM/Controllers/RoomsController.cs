using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class CreateRoomRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly Context context;
        public RoomsController(UserManager<User> userManager, Context context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Room>> Create([FromBody] CreateRoomRequest request)
        {
            var user = await userManager.GetUserAsyncOrFail(User);

            var room = new Room
            {
                Name = request.Name,
                Founder = user,
                Description = request.Description
            };
            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            return room;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<dynamic>> List()
        {
            return new
            {
                rooms = await context.Rooms.ToListAsync()
            };
        }
    }
}