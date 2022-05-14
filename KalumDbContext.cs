using Microsoft.EntityFrameworkCore;
using WebApiKalumn.Entities;

namespace WebApiKalumn
{
    public class KalumDbContext : DbContext
    {
        public DbSet<CarreraTecnica>CarreraTecnica { get; set; }
        public  KalumDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new {ct.CarreraId});
        }
    }
}