using INSTITUTO_C.Helpers;
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

        public MateriaCursada materiaCursada { get; set; }

        public Alumno alumno { get; set; } 





    }
}
