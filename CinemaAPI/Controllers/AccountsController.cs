using AuthenticationPlugin;
using AutoMapper;
using CinemaAPI.Data;
using CinemaAPI.DTOs;
using CinemaAPI.Models;
using CinemaAPI.Services.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly CinemaDbContext _cinemaDbContext;
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;

        public AccountsController(CinemaDbContext cinemaDbContext, IAccountServices accountServices, IMapper mapper)
        {
            _mapper = mapper;
            _accountServices = accountServices;
            _cinemaDbContext = cinemaDbContext;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto user)
        {
            try
            {
                if (await IsAnotherUserHasMyEmail(user.Email)) return BadRequest($"{user.Email} is already used!");

                var userMapped = _mapper.Map<User>(user);

                userMapped.Role = GetDefaultUserRole();
                userMapped.Password = SecurePasswordHasherHelper.Hash(userMapped.Password);

                await _cinemaDbContext.User.AddAsync(userMapped);

                await _cinemaDbContext.SaveChangesAsync();

                return Created(HttpContext.Request.Path, user);
            }
            catch (Exception x)
            {
                return BadRequest(x.InnerException?.Message ?? x.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var loginResult = await _accountServices.Login(userLoginDto);

            if (loginResult.Item1) return Ok(loginResult.Item3);

            return BadRequest(loginResult.Item2);
        }

        private async Task<bool> IsAnotherUserHasMyEmail(string email)
        {
            return await _cinemaDbContext.User.AnyAsync(user => user.Email.Equals(email));
        }

        private string GetDefaultUserRole() => "USERS";
    }
}
