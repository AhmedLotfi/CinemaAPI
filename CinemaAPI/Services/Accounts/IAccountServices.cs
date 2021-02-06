using CinemaAPI.DTOs;
using System.Threading.Tasks;

namespace CinemaAPI.Services.Accounts
{
    public interface IAccountServices
    {
        Task<(bool, string, object)> Login(UserLoginDto userLoginDto);
    }
}
