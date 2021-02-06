using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(150)]
        public string Role { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
