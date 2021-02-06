using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace CinemaAPI.Models
{
    public class Movie
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Language { get; set; }

        [Required]
        [MaxLength(50)]
        public string Duration { get; set; }

        [Required]
        [MaxLength(150)]
        public string Genre { get; set; }

        [MaxLength(1000)]
        public string TrailorURL { get; set; }

        [MaxLength(1000)]
        public string ImageURL { get; set; }

        public DateTime PlayingDate { get; set; }

        public DateTime PlayingTime { get; set; }

        public double TicketPrice { get; set; }

        public double Rate { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public string GetDefaultPath(string fileName = "")
            => Path.Combine("wwwroot/Movies", !string.IsNullOrEmpty(fileName) ? fileName : $"{Guid.NewGuid()}.jpg");

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
