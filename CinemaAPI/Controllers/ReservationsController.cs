using CinemaAPI.BaseControllers;
using CinemaAPI.DTOs.Reservations;
using CinemaAPI.Models;
using CinemaAPI.Services.Reservations;
using CinemaAPI.Utilites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    public class ReservationsController : AuthBaseController
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: api/<ReservationsController>
        [HttpGet]
        [Authorize(Roles = AppRoles.AdminRole)]
        public async Task<APIResponse> Get()
        {
            return await _reservationService.GetAll();
        }

        [HttpGet]
        [Route(nameof(GetPagged))]
        [Authorize(Roles = AppRoles.AdminRole)]
        public async Task<APIResponse> GetPagged(string search, string sort = "desc", int pageNumber = 1, int pageSize = 5)
        {
            return await _reservationService.GetAll(search, sort, pageNumber, pageSize);
        }

        // GET api/<ReservationsController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = AppRoles.AdminRole)]
        public async Task<APIResponse> Get(long id)
        {
            return await _reservationService.GetById(id);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.AdminRole)]
        public async Task<APIResponse> Post([FromForm] CreateReservationDto createReservationDto)
        {
            return await _reservationService.Post(createReservationDto);
        }
    }
}
