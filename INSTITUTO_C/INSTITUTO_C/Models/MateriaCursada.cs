using System;
using System.Collections.Generic;

namespace INSTITUTO_C.Models
{
    public class MateriaCursada
    {
        public int Id { get; set; }
        public Materia Materia { get; set; }
        public char CodigoCursada { get; set; }
        public int Anio { get; set; }
        public int Cuatrimestre { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public Profesor Profesor { get; set; }
        public List<Inscripcion> Inscripciones { get; set; }
    }
}
