using AutoMapper;
using CinemaAPI.DTOs;
using CinemaAPI.DTOs.Movies;
using CinemaAPI.DTOs.Reservations;
using CinemaAPI.Models;

namespace CinemaAPI.MappingProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<CreateUserDto, User>();

            CreateMap<CreateMovieDto, Movie>();

            CreateMap<EditMovieDto, Movie>();

            CreateMap<Movie, GetAllMovieDto>();

            CreateMap<Movie, MovieDto>();

            CreateMap<CreateReservationDto, Reservation>();

            CreateMap<GetAllReservationDto, Reservation>();

            CreateMap<Reservation, GetAllReservationDto>()
                .ForMember(o => o.MovieName, x => x.MapFrom(c => c.Movie.Name));

            CreateMap<ReservationDto, Reservation>();

            CreateMap<Reservation, ReservationDto>()
                .ForMember(o => o.MovieName, x => x.MapFrom(c => c.Movie.Name));
        }
    }
}
