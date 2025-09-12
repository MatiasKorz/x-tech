using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateOnly FechaAlta { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s' ]+$", ErrorMessage = "El {0} solo puede contener letras y espacios. ")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s' ]+$", ErrorMessage = "El {0} solo puede contener letras y espacios. ")]
        public string Apellido { get; set; }

        [Required]
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
