using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
