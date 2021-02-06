using AutoMapper;
using CinemaAPI.Data;
using CinemaAPI.DTOs;
using CinemaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CinemaDbContext _cinemaDbContext;

        private readonly IMapper _mapper;

        public UsersController(CinemaDbContext cinemaDbContext, IMapper mapper)
        {
            _mapper = mapper;
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

                await _cinemaDbContext.User.AddAsync(userMapped);

                await _cinemaDbContext.SaveChangesAsync();

                return Created(HttpContext.Request.Path, user);
            }
            catch (Exception x)
            {
                return BadRequest(x.InnerException?.Message ?? x.Message);
            }
        }

        private async Task<bool> IsAnotherUserHasMyEmail(string email)
        {
            return await _cinemaDbContext.User.AnyAsync(user => user.Email.Equals(email));
        }

        private string GetDefaultUserRole() => "USERS";
    }
}
