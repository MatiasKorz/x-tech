using INSTITUTO_C.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INSTITUTO_C.Models
{
    public class Inscripcion
    {
        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Key]
        public int MateriaCursadaId { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Key]
        public int AlumnoId { get; set; }
        //asi estaba en los videos cortos

        public MateriaCursada MateriaCursada { get; set; }

        public Alumno Alumno { get; set; }

        public Calificacion Calificacion { get; set; }

    }
}
