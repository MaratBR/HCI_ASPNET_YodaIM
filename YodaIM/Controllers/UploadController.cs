﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YodaIM.Models;
using YodaIM.Services;

namespace YodaIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly UserManager<User> userManager;

        public UploadController(IFileService fileService, UserManager<User> userManager)
        {
            _fileService = fileService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<FileModel>> Upload()
        {
            if (Request.Form.Files.Count == 0)
            {
                ModelState.AddModelError("file", "File data not specified");
                return BadRequest(ModelState);
            }

            var file = Request.Form.Files[0];

            if (file.Length > 0 && file.Length < 1024 * 1024 * 1024)
            {
                return await _fileService.Upload(file, await userManager.GetUserAsync(User), FileType.Generic);
            }
            else
            {
                ModelState.AddModelError("file", "Too big, at most 1M");
                return BadRequest();
            }

        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FileModel>> GetFileModel([FromRoute] Guid id)
        {
            var file = await _fileService.Get(id);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpGet("{id}/download")]
        [Authorize]
        public async Task<ActionResult> DownloadFile([FromRoute] Guid id)
        {
            var file = await _fileService.Get(id);

            if (file == null)
            {
                return NotFound();
            }

            Response.Headers.Add("Content-Disposition", "filename=" + file.FileName);
            Response.Headers.Add("File-Id", file.Id.ToString());
            Response.Headers.Add("File-UploaderId", file.UserId.ToString());

            return File(file.Data, file.ContentType);
        }

    }
}