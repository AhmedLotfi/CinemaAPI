using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTOs.Reservations
{
    public class CreateReservationDto
    {
        public int Quantity { get; set; }

        public double Price { get; set; }

        public DateTime ReservationTime { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }

        public int UserId { get; set; }

        public long MovieId { get; set; }
    }
}
