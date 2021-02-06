using CinemaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Data
{
    public class CinemaDbContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }

        public CinemaDbContext(DbContextOptions<CinemaDbContext> dbContextOptions) : base(dbContextOptions) { }
    }
}
