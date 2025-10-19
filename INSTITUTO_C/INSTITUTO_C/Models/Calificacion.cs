using INSTITUTO_C.Helpers;
using System.ComponentModel.DataAnnotations;
using System;

namespace INSTITUTO_C.Models
{


    public class Calificacion
    {
        //- Fecha(solo fecha)
        //- Nota(enum)
        //- Profesor
        //- Inscripcion
        //- Alumno

        [Key]
        public int AlumnoId { get; set; }
        [Key]
        public int MateriaCursadald { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public Nota Nota { get; set; }  

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public int ProfesorId { get; set; }

        public Profesor Profesor { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public int MateriaCursadaId { get; set; }

        public Inscripcion Inscripcion { get; set; }

        public Alumno Alumno { get; set; }
    }
}
