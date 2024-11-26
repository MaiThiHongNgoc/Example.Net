using ComicSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Data
{
    public class ComicSystemDbContext : DbContext
    {
        public ComicSystemDbContext(DbContextOptions<ComicSystemDbContext> options) : base(options) { }

        // DbSet cho từng bảng
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDetail> RentalDetails { get; set; }
    }
}
