using AutoMapper;
using CinemaAPI.DTOs;
using CinemaAPI.Models;

namespace CinemaAPI.MappingProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<CreateUserDto, User>();
        }
    }
}
