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
        [Display(Name = "Alumno")]
        public int AlumnoId { get; set; }
        [Key]
        [Display(Name = "Cursada")]
        public int MateriaCursadaId { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public Nota Nota { get; set; }  

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Display(Name = "Profesor")]
        public int ProfesorId { get; set; }

        public Profesor Profesor { get; set; }

    

        public Inscripcion Inscripcion { get; set; }

        public Alumno Alumno { get; set; }
    }
}
