using CinemaAPI.BaseControllers;
using CinemaAPI.DTOs.Movies;
using CinemaAPI.Models;
using CinemaAPI.Services.Movies;
using CinemaAPI.Utilites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    public class MoviesController : AuthBaseController
    {
        private readonly IMoviesService _moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public async Task<APIResponse> Get()
        {
            return await _moviesService.GetAll();
        }

        [HttpGet]
        [Route(nameof(GetPagged))]
        public async Task<APIResponse> GetPagged(string search, string sort = "desc", int pageNumber = 1, int pageSize = 5)
        {
            return await _moviesService.GetAll(search, sort, pageNumber, pageSize);
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public async Task<APIResponse> Get(long id)
        {
            return await _moviesService.GetById(id);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.AdminRole)]
        public async Task<APIResponse> Post([FromForm] CreateMovieDto createMovieDto)
        {
            return await _moviesService.Post(createMovieDto);
        }

        // PUT api/<MoviesController>
        [HttpPut]
        [Authorize(Roles = AppRoles.AdminRole)]
        public async Task<APIResponse> Put([FromForm] EditMovieDto editMovieDto)
        {
            return await _moviesService.Put(editMovieDto);
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = AppRoles.AdminRole)]
        public async Task<APIResponse> Delete(long id)
        {
            return await _moviesService.Delete(id);
        }
    }
}
