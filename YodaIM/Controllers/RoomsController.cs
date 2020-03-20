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
using YodaIM.Helpers.Errors;
using YodaIM.Models;
using YodaIM.Services;

namespace YodaIM.Controllers
{
    public class CreateRoomRequest : UpdateRoomRequest
    {

    }

    public class UpdateRoomRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public void Apply(Room room)
        {
            room.Name = Name;
            room.Description = Description;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IRoomsService roomService;
        private readonly Context context;
        public RoomsController(UserManager<User> userManager, Context context, IRoomsService roomService)
        {
            this.userManager = userManager;
            this.context = context;
            this.roomService = roomService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> Get([FromRoute] int id)
        {
            var room = await context.Rooms.Where(r => r.Id == id).SingleOrDefaultAsync();
            if (room == null)
                return RoomNotFound(id);

            return room;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Room>> Create([FromBody] CreateRoomRequest request)
        {
            var user = await userManager.GetUserAsyncOrFail(User);

            var room = new Room
            {
                Founder = user
            };
            request.Apply(room);
            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            return Created($"/api/rooms/{room.Id}", room);
        }

        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateRoomRequest request)
        {
            var room = await roomService.GetById(id);
            if (room == null)
                return RoomNotFound(id);

            request.Apply(room);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<dynamic>> List()
        {
            return new
            {
                rooms = await roomService.ListRooms(
                    await userManager.GetUserAsyncOrFail(User))
            };
        }

        public NotFoundObjectResult RoomNotFound(int id) => NotFound(new Error("Not found", $"Room with id = {id} not found"));
    }
}