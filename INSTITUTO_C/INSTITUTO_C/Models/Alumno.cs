using System;
using System.Collections.Generic;

namespace INSTITUTO_C.Models
{
    public class Alumno
    {
        //- UserName
        //- Email
        //- FechaAlta
        //- Nombre
        //- Apellido
        //- DNI
        //- Telefono
        //- Direccion
        //- Activo
        //- NumeroMatricula
        //- Carrera
        //- Inscripciones
        //- Calificaciones

        public string Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public DateOnly FechaAlta { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }

        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; }

        public int NumeroMatricula { get; set; }

        public Carrera Carrera { get; set; }

        public List<Inscripcion> Inscripciones { get; set; }
        public List<Calificacion> Calificaciones { get; set; }
        
}
}
