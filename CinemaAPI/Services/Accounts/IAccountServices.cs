using CinemaAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaAPI.Services.Accounts
{
    public interface IAccountServices
    {
        Task<(bool, string, ObjectResult)> Login(UserLoginDto userLoginDto);
    }
}
