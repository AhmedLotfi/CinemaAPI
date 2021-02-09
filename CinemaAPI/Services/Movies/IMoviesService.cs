using CinemaAPI.DTOs.Movies;
using CinemaAPI.Utilites;
using System.Threading.Tasks;

namespace CinemaAPI.Services.Movies
{
    public interface IMoviesService
    {
        Task<APIResponse> GetAll();

        Task<APIResponse> GetById(long id);

        Task<APIResponse> Post(CreateMovieDto createMovieDto);

        Task<APIResponse> Put(EditMovieDto editMovieDto);

        Task<APIResponse> Delete(long id);
    }
}
