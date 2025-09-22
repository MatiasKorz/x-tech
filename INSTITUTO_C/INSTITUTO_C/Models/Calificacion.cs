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

        public int id { get; set; }





        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime FechaAlta { get; set; }


        //????
        public enum Nota { }
        //????


        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }

        //????
        public Inscripcion Inscripcion { get; set; }
        //????

        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
    }
}
