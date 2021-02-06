using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public DateTime ReservationTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public long MovieId { get; set; }

        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; }
    }
}
