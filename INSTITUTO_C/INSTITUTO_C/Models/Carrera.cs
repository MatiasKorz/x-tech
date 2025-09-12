using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INSTITUTO_C.Models
{
    public class Carrera
    {
        //- Nombre
        //- Materias
        //- Alumnos


        public string Id { get; set; }





        [Required]
        [StringLength(20, MinimumLength = 5,ErrorMessage = "Tiene que tener entre {2} y {1} caracteres.")]
        public string Nombre { get; set; }


        public List<Materia> Materias { get; set; }
        public List<Alumno> Alumnos { get; set; }

    }
}
