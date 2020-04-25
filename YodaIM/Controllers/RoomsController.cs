using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YodaIM.Chat;
using YodaIM.Chat.DTO;
using YodaIM.Helpers;
using YodaIM.Models;
using YodaIM.Services;

namespace YodaIM.Controllers
{
    public class CreateRoomRequest : IRoomInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RoomMessagesResponse
    {
        public List<ChatMessageDto> Messages { get; set; }
    }

    public class RoomMembersResponse
    {
        public List<ChatMembershipDto> Users { get; set; }
    }

    public class ListOfRoomsResponse
    {
        public List<Room> Rooms { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IRoomService roomService;
        private readonly IMessageService messageService;

        public RoomsController(UserManager<User> userManager, IRoomService roomService, IMessageService messageService)
        {
            this.userManager = userManager;
            this.roomService = roomService;
            this.messageService = messageService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Room>> Create([FromBody] CreateRoomRequest request)
        {
            var user = await userManager.GetUserAsyncOrFail(User);
            var room = await roomService.CreateRoom(request);

            await roomService.JoinRoom(user, room);


            return Created($"/api/rooms/{room.Id}", room);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ListOfRoomsResponse>> List()
        {
            var user = await userManager.GetUserAsyncOrFail(User);
            var rooms = await roomService.ListRooms(user);

            return new ListOfRoomsResponse
            {
                Rooms = rooms
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom([FromRoute] Guid id)
            => await roomService.GetById(id);

        [HttpGet("{id}/messages")]
        public async Task<ActionResult<RoomMessagesResponse>> GetMessages([FromRoute] Guid id, [FromQuery] DateTime? before = null)
        {
            var user = await userManager.GetUserAsyncOrFail(User);

            if (!await roomService.InRoom(user, id))
            {
                return Forbid();
            }

            var messages = (await messageService.GetMessages(id, beforeUtc: before))
                .Select(m =>
                {
                    return Dto.CreateMessage(m, m.MessageAttachments.Select(a => a.FileModel).ToList());
                })
                .ToList();

            return new RoomMessagesResponse
            {
                Messages = messages
            };
        }

        [HttpPost("membership/{id}")]
        public async Task<ActionResult> JoinRoom([FromRoute] Guid id)
        {
            var user = await userManager.GetUserAsyncOrFail(User);
            var room = await roomService.GetById(id);

            if (room == null)
                return NotFound();

            if (!await roomService.InRoom(user, room))
            {
                await roomService.JoinRoom(user, room);
            }

            return NoContent();
        }

        [HttpDelete("membership/{id}")]
        public async Task<ActionResult> LeaveRoom([FromRoute] Guid id)
        {
            var user = await userManager.GetUserAsyncOrFail(User);
            var room = await roomService.GetById(id);

            if (room == null)
                return NotFound();

            if (await roomService.InRoom(user, room))
            {
                await roomService.LeaveRoom(user, room);
            }

            return NoContent();
        }

        [HttpGet("{id}/members")]
        public async Task<ActionResult<RoomMembersResponse>> GetRoomMembers([FromRoute] Guid id, [FromServices] IChatState chatState)
        {
            var room = await roomService.GetById(id);

            if (room == null)
                return NotFound();

            var users = await roomService.GetUsersFromRoom(room);
            return new RoomMembersResponse
            {
                Users = users.Select(r => new ChatMembershipDto { User = r.User, IsOnline = chatState.IsOnline(r.User) }).ToList()
            };
        }

    }
}