using AutoMapper;
using CinemaAPI.Data;
using CinemaAPI.DTOs.Reservations;
using CinemaAPI.Models;
using CinemaAPI.Utilites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CinemaAPI.Services.Reservations
{
    public class ReservationService : IReservationService
    {
        private readonly CinemaDbContext _cinemaDbContext;
        private readonly IMapper _mapper;

        public ReservationService(CinemaDbContext cinemaDbContext, IMapper mapper)
        {
            _cinemaDbContext = cinemaDbContext;
            _mapper = mapper;
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                var data = await _cinemaDbContext.Reservation
                                 .Include(res => res.Movie)
                                 .Include(us => us.User)
                                 .ToListAsync();

                var mappedData = _mapper.Map<GetAllReservationDto>(data);

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
                var data = _cinemaDbContext.Reservation
                            .Include(res => res.Movie)
                            .Include(us => us.User)
                            .Where(reserv => string.IsNullOrEmpty(search) || reserv.Movie.Name.Contains(search));

                data = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                switch (sort)
                {
                    case "desc":
                        return APIResponse.GetAPIResponse(
                            (int)HttpStatusCode.OK,
                            string.Empty,
                            _mapper.Map<GetAllReservationDto>(await data.OrderByDescending(reserv => reserv.ReservationTime).ToListAsync()));
                    case "asc":
                        return APIResponse.GetAPIResponse(
                            (int)HttpStatusCode.OK,
                            string.Empty,
                            _mapper.Map<GetAllReservationDto>(await data.OrderByDescending(reserv => reserv.ReservationTime).ToListAsync()));

                    default:
                        return APIResponse.GetAPIResponse(
                                (int)HttpStatusCode.OK,
                                string.Empty,
                                _mapper.Map<GetAllReservationDto>(data));
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
                var currentReservation = await _cinemaDbContext.Reservation.FindAsync(id);

                var mappedData = _mapper.Map<ReservationDto>(currentReservation);

                return APIResponse.GetAPIResponse((int)HttpStatusCode.OK, string.Empty, mappedData);
            }
            catch (Exception x)
            {
                return APIResponse.GetAPIResponse((int)HttpStatusCode.BadRequest, x.InnerException?.Message ?? x.Message, null);
            }
        }

        public async Task<APIResponse> Post(CreateReservationDto createReservationDto)
        {
            try
            {
                var reserv = _mapper.Map<Reservation>(createReservationDto);

                _cinemaDbContext.Reservation.Add(reserv);
                await _cinemaDbContext.SaveChangesAsync();

                return APIResponse.GetAPIResponse((int)HttpStatusCode.Created, "Reservation Created Successfully", reserv);
            }
            catch (Exception x)
            {
                return APIResponse.GetAPIResponse((int)HttpStatusCode.BadRequest, x.InnerException?.Message ?? x.Message, null);
            }
        }
    }
}
