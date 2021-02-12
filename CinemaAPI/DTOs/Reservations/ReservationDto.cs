using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTOs.Reservations
{
    public class ReservationDto
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public DateTime ReservationTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public long MovieId { get; set; }

        public string MovieName { get; set; }
    }
}
