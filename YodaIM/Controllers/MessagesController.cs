using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YodaIM.Models;
using YodaIM.Services;

namespace YodaIM.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet("{roomId}")]
        public async Task<ActionResult<List<Message>>> GetMessages([FromRoute] Guid roomId, [FromQuery] DateTime? before = null)
        {
            return await messageService.GetMessages(roomId, beforeUtc: before);
        }
    }
}