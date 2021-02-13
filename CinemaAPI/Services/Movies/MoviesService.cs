using AutoMapper;
using CinemaAPI.Data;
using CinemaAPI.DTOs.Movies;
using CinemaAPI.Models;
using CinemaAPI.Utilites;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CinemaAPI.Services.Movies
{
    public class MoviesService : IMoviesService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly CinemaDbContext _cinemaDbContext;
        private readonly IMapper _mapper;

        public MoviesService(CinemaDbContext cinemaDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _cinemaDbContext = cinemaDbContext;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                var data = await _cinemaDbContext.Movies.ToListAsync();

                var mappedData = _mapper.Map<IReadOnlyList<GetAllMovieDto>>(data);

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
                            _mapper.Map<IReadOnlyList<GetAllMovieDto>>(await data.OrderByDescending(movie => movie.Rate).ToListAsync()));
                    case "asc":
                        return APIResponse.GetAPIResponse(
                            (int)HttpStatusCode.OK,
                            string.Empty,
                            _mapper.Map<IReadOnlyList<GetAllMovieDto>>(await data.OrderByDescending(movie => movie.Rate).ToListAsync()));

                    default:
                        return APIResponse.GetAPIResponse(
                                (int)HttpStatusCode.OK,
                                string.Empty,
                                _mapper.Map<IReadOnlyList<GetAllMovieDto>>(data));
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
                    string path = GetDefaultMoviePath();

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await createMovieDto.Image.CopyToAsync(fileStream);

                        createMovieDto.ImageURL = Path.GetRelativePath(_webHostEnvironment.WebRootPath, path);
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
                    string path = GetDefaultMoviePath();

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await editMovieDto.Image.CopyToAsync(fileStream);

                        editMovieDto.ImageURL = Path.GetRelativePath(_webHostEnvironment.WebRootPath, path);
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
        {
            return Path.Combine($"{_webHostEnvironment.WebRootPath}/Movies", !string.IsNullOrEmpty(fileName) ? fileName : $"{Guid.NewGuid()}.jpg");
        }
    }
}
