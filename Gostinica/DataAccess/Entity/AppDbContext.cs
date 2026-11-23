using Microsoft.EntityFrameworkCore;

namespace Gostinica.DataAccess.Entity
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Room> Room { get; set; }
        public DbSet<Guest> Guest { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<HotelService> HotelService { get; set; }
        public DbSet<Booking> Booking { get; set; }
    }
}
