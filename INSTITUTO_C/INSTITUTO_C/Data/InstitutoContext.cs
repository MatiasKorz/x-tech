using INSTITUTO_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace INSTITUTO_C.Data
{
    public class InstitutoContext: IdentityDbContext<IdentityUser<int>, IdentityRole<int>,int>
    {
        public InstitutoContext(DbContextOptions options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Inscripcion>().HasKey(i => new { i.AlumnoId, i.MateriaCursadaId });

            modelBuilder.Entity<Inscripcion>().HasOne(i => i.Alumno).WithMany(a => a.Inscripciones).HasForeignKey(i=> i.AlumnoId);
            modelBuilder.Entity<Inscripcion>().HasOne(i => i.MateriaCursada).WithMany(mc => mc.Inscripciones).HasForeignKey(i => i.MateriaCursadaId);


            // Clave compuesta para Calificación (PK y FK)
            modelBuilder.Entity<Calificacion>().HasKey(c => new { c.AlumnoId, c.MateriaCursadaId });
            modelBuilder.Entity<Calificacion>().HasOne(c => c.Inscripcion).WithOne(i => i.Calificacion).HasForeignKey<Calificacion>(c => new { c.AlumnoId, c.MateriaCursadaId });



            //fecha alta
            modelBuilder.Entity<Persona>()
           .Property(p => p.FechaAlta)
           .ValueGeneratedOnAdd();



            modelBuilder.Entity<Empleado>()
           .HasIndex(e => e.Legajo)
           .IsUnique();

            modelBuilder.Entity<Alumno>()
          .HasIndex(a => a.NumeroMatricula)
          .IsUnique();


            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");

        }


        public DbSet<Persona> Personas { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
          
        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<Profesor> Profesores { get; set; }

        public DbSet<Carrera> Carreras { get; set; }

        public DbSet<Calificacion> Calificaciones { get; set; }

        public DbSet<Materia> Materias { get; set; }


        public DbSet<MateriaCursada> MateriasCursadas { get; set; }
        public DbSet<INSTITUTO_C.Models.Inscripcion> Inscripciones { get; set; }

    }
}
