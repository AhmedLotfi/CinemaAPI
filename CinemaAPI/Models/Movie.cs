using Microsoft.AspNetCore.Http;
using System;
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

        [MaxLength(100)]
        public string Language { get; set; }

        [Column(TypeName = "decimal(18,1)")]
        public double Rate { get; set; }

        [MaxLength(1000)]
        public string ImageURL { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public string GetDefaultPath(string fileName = "")
            => Path.Combine("wwwroot/Movies", !string.IsNullOrEmpty(fileName) ? fileName : $"{Guid.NewGuid()}.jpg");
    }
}
