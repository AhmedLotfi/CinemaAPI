using AutoMapper;
using CinemaAPI.Data;
using CinemaAPI.DTOs.Movies;
using CinemaAPI.Models;
using CinemaAPI.Utilites;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CinemaAPI.Services.Movies
{
    public class MoviesService : IMoviesService
    {
        private readonly CinemaDbContext _cinemaDbContext;
        private readonly IMapper _mapper;

        public MoviesService(CinemaDbContext cinemaDbContext, IMapper mapper)
        {
            _cinemaDbContext = cinemaDbContext;
            _mapper = mapper;
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                var data = await _cinemaDbContext.Movies.ToListAsync();

                var mappedData = _mapper.Map<GetAllMovieDto>(data);

                return APIResponse.GetAPIResponse((int)HttpStatusCode.OK, string.Empty, mappedData);
            }
            catch (Exception x)
            {
                return APIResponse.GetAPIResponse((int)HttpStatusCode.BadRequest, x.InnerException?.Message ?? x.Message, null);
            }
        }

        public async Task<APIResponse> GetAll(string search, string sort = "desc", int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var data = _cinemaDbContext.Movies.Where(movie => string.IsNullOrEmpty(search) || movie.Name.Contains(search));

                data = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                switch (sort)
                {
                    case "desc":
                        return APIResponse.GetAPIResponse(
                            (int)HttpStatusCode.OK,
                            string.Empty,
                            _mapper.Map<GetAllMovieDto>(await data.OrderByDescending(movie => movie.Rate).ToListAsync()));
                    case "asc":
                        return APIResponse.GetAPIResponse(
                            (int)HttpStatusCode.OK,
                            string.Empty,
                            _mapper.Map<GetAllMovieDto>(await data.OrderByDescending(movie => movie.Rate).ToListAsync()));

                    default:
                        return APIResponse.GetAPIResponse(
                                (int)HttpStatusCode.OK,
                                string.Empty,
                                _mapper.Map<GetAllMovieDto>(data));
                }

            }
            catch (Exception x)
            {
                return APIResponse.GetAPIResponse((int)HttpStatusCode.BadRequest, x.InnerException?.Message ?? x.Message, null);
            }
        }

        public async Task<APIResponse> GetById(long id)
        {
            try
            {
                var currentMovie = await _cinemaDbContext.Movies.FindAsync(id);

                var mappedData = _mapper.Map<MovieDto>(currentMovie);

                return APIResponse.GetAPIResponse((int)HttpStatusCode.OK, string.Empty, mappedData);
            }
            catch (Exception x)
            {
                return APIResponse.GetAPIResponse((int)HttpStatusCode.BadRequest, x.InnerException?.Message ?? x.Message, null);
            }
        }

        public async Task<APIResponse> Post(CreateMovieDto createMovieDto)
        {
            try
            {
                if (createMovieDto.Image != null)
                {
                    using (var fileStream = new FileStream(GetDefaultMoviePath(), FileMode.Create))
                    {
                        await createMovieDto.Image.CopyToAsync(fileStream);

                        createMovieDto.ImageURL = GetDefaultMoviePath();
                    }
                }

                var movie = _mapper.Map<Movie>(createMovieDto);

                _cinemaDbContext.Movies.Add(movie);
                await _cinemaDbContext.SaveChangesAsync();

                return APIResponse.GetAPIResponse((int)HttpStatusCode.Created, "Movie Created Successfully", movie);
            }
            catch (Exception x)
            {
                return APIResponse.GetAPIResponse((int)HttpStatusCode.BadRequest, x.InnerException?.Message ?? x.Message, null);
            }
        }

        public async Task<APIResponse> Put(EditMovieDto editMovieDto)
        {
            try
            {
                var currentMovie = await _cinemaDbContext.Movies.FindAsync(editMovieDto.Id);

                if (editMovieDto.Image != null)
                {
                    using (var fileStream = new FileStream(GetDefaultMoviePath(), FileMode.Create))
                    {
                        await editMovieDto.Image.CopyToAsync(fileStream);

                        editMovieDto.ImageURL = GetDefaultMoviePath();
                    }
                }

                _mapper.Map(editMovieDto, currentMovie);

                _cinemaDbContext.Movies.Update(currentMovie);
                await _cinemaDbContext.SaveChangesAsync();

                return APIResponse.GetAPIResponse((int)HttpStatusCode.OK, "Movie Updated Successfully", currentMovie);
            }
            catch (Exception x)
            {
                return APIResponse.GetAPIResponse((int)HttpStatusCode.BadRequest, x.InnerException?.Message ?? x.Message, null);
            }
        }

        public async Task<APIResponse> Delete(long id)
        {
            try
            {
                var currentMovie = await _cinemaDbContext.Movies.FindAsync(id);

                _cinemaDbContext.Movies.Remove(currentMovie);

                await _cinemaDbContext.SaveChangesAsync();

                return APIResponse.GetAPIResponse((int)HttpStatusCode.OK, "Movie Deleted Successfully", null);
            }
            catch (Exception x)
            {
                return APIResponse.GetAPIResponse((int)HttpStatusCode.BadRequest, x.InnerException?.Message ?? x.Message, null);
            }
        }

        private string GetDefaultMoviePath(string fileName = "")
             => Path.Combine("wwwroot/Movies", !string.IsNullOrEmpty(fileName) ? fileName : $"{Guid.NewGuid()}.jpg");

    }
}
