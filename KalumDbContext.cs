using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum
{
    public class KalumDbContext : DbContext
    {
        public DbSet<Jornada> Jornada { get; set; }
        public DbSet<CarreraTecnica> CarreraTecnica { get; set; }
        public DbSet<Aspirante> Aspirante { get; set; }
        public  KalumDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Jornada>().ToTable("Jornada").HasKey(j => new{j.JornadaId});
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new {ct.CarreraId});
            modelBuilder.Entity<Aspirante>().ToTable("Aspirante").HasKey(a => new {a.NoExpediente});
            modelBuilder.Entity<Aspirante>()
                .HasOne<CarreraTecnica>(c=>c.CarreraTecnica)
                .WithMany(ct => ct.Aspirantes)
                .HasForeignKey(c => c.CarreraId);
            modelBuilder.Entity<Aspirante>()
                .HasOne<Jornada>(j=>j.Jornada)
                .WithMany(j => j.Aspirantes)
                .HasForeignKey(j => j.JornadaId);
    
        }
    }
}