using INSTITUTO_C.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;

namespace INSTITUTO_C.Models
{
    public class Carrera
    {
        //- Nombre
        //- Materias
        //- Alumnos

        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(20, MinimumLength = 5,ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        public string Nombre { get; set; }

        public List<Materia> Materias { get; set; }

        public List<Alumno> Alumnos { get; set; }

    }
}
