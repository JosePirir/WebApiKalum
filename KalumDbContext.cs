using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum
{
    public class KalumDbContext : DbContext
    {
        public DbSet<Jornada> Jornada { get; set; }
        public DbSet<CarreraTecnica> CarreraTecnica { get; set; }
        public DbSet<Alumno> Alumno { get; set; }
        public DbSet<Aspirante> Aspirante { get; set; }
        public DbSet<ExamenAdmision> ExamenAdmision { get; set; }
        public DbSet<Inscripcion> Inscripcion { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<CuentaXCobrar> CuentaXCobrar { get; set; }
        public DbSet<ResultadoExamenAdmision> ResultadoExamenAdmision { get; set; }
        public DbSet<InscripcionPago> InscripcionPago { get; set; }
        public DbSet<InversionCarreraTecnica> InversionCarreraTecnica { get; set; }
        public  KalumDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Jornada>().ToTable("Jornada").HasKey(j => new{j.JornadaId});
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new {ct.CarreraId});
            modelBuilder.Entity<ExamenAdmision>().ToTable("ExamenAdmision").HasKey(ea=> new {ea.ExamenId});
            modelBuilder.Entity<Aspirante>().ToTable("Aspirante").HasKey(a => new {a.NoExpediente});
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion").HasKey(i => new {i.InscripcionId});
            modelBuilder.Entity<Alumno>().ToTable("Alumno").HasKey(a2 => new {a2.Carne});
            modelBuilder.Entity<Cargo>().ToTable("Cargo").HasKey(c => new {c.CargoId});
            modelBuilder.Entity<CuentaXCobrar>().ToTable("CuentaXCobrar").HasKey(cxc => new{cxc.Cargo, cxc.Anio, cxc.Carne});
            modelBuilder.Entity<ResultadoExamenAdmision>().ToTable("ResultadoExamenAdmision").HasKey(rea => new{rea.NoExpediente, rea.Anio});
            modelBuilder.Entity<InscripcionPago>().ToTable("InscripcionPago").HasKey(ip => new{ip.BoletaPago, ip.NoExpediente, ip.Anio});
            modelBuilder.Entity<InversionCarreraTecnica>().ToTable("InversionCarreraTecnica").HasKey(ict => new{ict.InversionId});
            
            modelBuilder.Entity<Aspirante>()
                .HasOne<CarreraTecnica>(a=>a.CarreraTecnica)
                .WithMany(ct => ct.Aspirantes)
                .HasForeignKey(a => a.CarreraId);

            modelBuilder.Entity<Aspirante>()
                .HasOne<Jornada>(a=>a.Jornada)/*amarra a jornada con aspirante*/
                .WithMany(j => j.Aspirantes)/*union de una jornada a muchos aspirantes*/
                .HasForeignKey(a => a.JornadaId);/*la manera de unir una tabla con las llaves foraneas*/
                
            modelBuilder.Entity<Aspirante>()
                .HasOne<ExamenAdmision>(a=>a.ExamenAdmision)
                .WithMany(ea => ea.Aspirantes)
                .HasForeignKey(a => a.ExamenId);
    
            modelBuilder.Entity<Inscripcion>()
                .HasOne<CarreraTecnica>(i => i.CarreraTecnica)
                .WithMany(ct => ct.Inscripciones)
                .HasForeignKey(i => i.CarreraId);
            modelBuilder.Entity<Inscripcion>()
                .HasOne<Jornada>(i => i.Jornada)
                .WithMany(j => j.Inscripciones)
                .HasForeignKey(i => i.JornadaId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<Alumno>(i => i.Alumnos)
                .WithMany(a2 => a2.Inscripcion)
                .HasForeignKey(i=>i.Carne);

            modelBuilder.Entity<CuentaXCobrar>()
                .HasOne<Alumno>(cxc => cxc.Alumnos)
                .WithMany(a2 => a2.CuentaXCobrar)
                .HasForeignKey(cxc => cxc.Carne);

            modelBuilder.Entity<CuentaXCobrar>()
                .HasOne<Cargo>(cxc => cxc.Cargos)
                .WithMany(c => c.CuentaXCobrar)
                .HasForeignKey(cxc => cxc.CargoId);

            modelBuilder.Entity<ResultadoExamenAdmision>()
                .HasOne<Aspirante>(rea => rea.Aspirantes)
                .WithMany(a => a.ResultadoExamenAdmision)
                .HasForeignKey(rea => rea.NoExpediente);

            modelBuilder.Entity<InscripcionPago>()
                .HasOne<Aspirante>(ip=>ip.Aspirantes)
                .WithMany(a => a.InscripcionPago)
                .HasForeignKey(ip => ip.NoExpediente);

            modelBuilder.Entity<InversionCarreraTecnica>()
                .HasOne<CarreraTecnica>(ict => ict.CarreraTecnica)
                .WithMany(ct => ct.InversionCarreraTecnica)
                .HasForeignKey(ict => ict.CarreraId);
        }
    }
}