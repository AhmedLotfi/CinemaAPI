using CinemaAPI.DTOs.Reservations;
using CinemaAPI.Utilites;
using System.Threading.Tasks;

namespace CinemaAPI.Services.Reservations
{
    public interface IReservationService
    {
        Task<APIResponse> GetAll();

        Task<APIResponse> GetAll(string search, string sort = "desc", int pageNumber = 1, int pageSize = 5);

        Task<APIResponse> GetById(long id);

        Task<APIResponse> Post(CreateReservationDto createReservationDto);
    }
}
