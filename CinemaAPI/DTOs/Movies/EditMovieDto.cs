using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTOs.Movies
{
    public class EditMovieDto
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

        public IFormFile Image { get; set; }
    }
}
