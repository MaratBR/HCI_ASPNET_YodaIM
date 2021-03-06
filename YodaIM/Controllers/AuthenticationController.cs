﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YodaIM.Helpers;
using YodaIM.Helpers.Errors;
using YodaIM.Models;
using YodaIM.Services;

namespace YodaIM.Controllers
{
    public class RegistrationResponse
    {
        public User User { get; set; }
    }

    public class RegistrationRequest
    {
        [Required] [MinLength(2, ErrorMessage = "UserName must be at least 2 characters")] [MaxLength(50, ErrorMessage = "UserName must be at most 50 characters")]
        public string UserName { get; set; }

        [Required] // судя по всему валидация пароля происходит где-то в закромах ASP.NET Identity, так что тут ничего не надо
        public string Password { get; set; }

        [Required] [EmailAddress]
        public string Email { get; set; }

        [Required] [Phone]
        public string PhoneNumber { get; set; }

        public byte Gender { get; set; } = 0;
    }

    public class AuthenticateRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AuthenticateResponse
    {
        public string AccessToken { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;

        public AuthenticationController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        // CREDITS: https://medium.com/@ozgurgul/asp-net-core-2-0-webapi-jwt-authentication-with-identity-mysql-3698eeba6ff8

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Gender = request.Gender
                };
                var result = await userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    return new RegistrationResponse
                    {
                        User = user
                    };
                }

                return BadRequest(new Error(
                    "Errors occurred",
                    "Errors occurred while trying to register a new user",
                    errors: result.Errors.Select(e => new Error($"{e.Code}: {e.Description}"))));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate([FromBody] AuthenticateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByLogin(request.Login);

            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                false
             );

                if (result.Succeeded)
                {
                    var token = tokenService.CreateIdentityToken(user);

                    return new AuthenticateResponse
                    {
                        AccessToken = tokenService.Strigify(token),
                        UserId = user.Id,
                        ExpiresAt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds((int)token.Payload.Exp)
                    };
                }
            }

            return Unauthorized(new Error("Invalid credentials", "We haven't found a user which credentials match to yours"));
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> CurrentUser()
        {
            var user = await userManager.GetUserAsyncOrFail(User);
            return Ok(user);
        }
    }
}