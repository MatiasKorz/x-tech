using System.Collections.Generic;

namespace INSTITUTO_C.Models
{
    public class Carrera
    {
        //- Nombre
        //- Materias
        //- Alumnos


        public string Id { get; set; }
        public string Nombre { get; set; }
        public List<Materia> Materias { get; set; }
        public List<Alumno> Alumnos { get; set; }

    }
}
