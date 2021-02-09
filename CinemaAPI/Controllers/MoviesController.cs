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
