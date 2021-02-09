using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTOs
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(200)]
        public string Password { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }
    }
}
