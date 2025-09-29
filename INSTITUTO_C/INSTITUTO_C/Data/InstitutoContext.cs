using INSTITUTO_C.Models;
using Microsoft.EntityFrameworkCore;

namespace INSTITUTO_C.Data
{
    public class InstitutoContext: DbContext
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
        }


        public DbSet<Persona> Personas { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
          
        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<Profesor> Profesores { get; set; }

        public DbSet<Carrera> Carreras { get; set; }

        public DbSet<Calificacion> Calificaciones { get; set; }

        public DbSet<Materia> Materias { get; set; }


        public DbSet<MateriaCursada> MateriasCursadas { get; set; }
        public DbSet<INSTITUTO_C.Models.Inscripcion> Inscripcion { get; set; }

    }
}
