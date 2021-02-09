using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTOs.Movies
{
    public class GetAllMovieDto
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

        [MaxLength(1000)]
        public string ImageURL { get; set; }
    }
}
