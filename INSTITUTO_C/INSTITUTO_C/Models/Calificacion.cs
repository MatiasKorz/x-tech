using INSTITUTO_C.Helpers;
using System.ComponentModel.DataAnnotations;
using System;

namespace INSTITUTO_C.Models
{
    public enum Nota 
    {
        Cero = 0,
        Uno = 1,
        Dos = 2,
        Tres = 3,
        Cuatro = 4,
        Cinco = 5,
        Seis = 6,
        Siete = 7,
        Ocho = 8,
        Nueve = 9,
        Diez = 10,
        Ausente,
        Pendiente,
        Baja
    }

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
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public Nota Nota { get; set; }  

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string ProfesorId { get; set; }

        public Profesor Profesor { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public int MateriaCursadaId { get; set; }

        public Inscripcion Inscripcion { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string AlumnoId { get; set; }

        public Alumno Alumno { get; set; }
    }
}
