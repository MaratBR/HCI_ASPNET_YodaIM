﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var room = await roomService.CreateRoom(request);

            return Created($"/api/room/{room.Id}", room);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<dynamic>> List()
        {
            var user = await userManager.GetUserAsyncOrFail(User);
            var rooms = await roomService.ListRooms(user);

            return new
            {
                rooms = rooms
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom([FromRoute] Guid id)
            => await roomService.GetById(id);

        [HttpGet("{id}/messages")]
        public async Task<ActionResult<RoomMessagesResponse>> GetMessages([FromRoute] Guid id, [FromQuery] DateTime? before = null)
        {
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

    }
}