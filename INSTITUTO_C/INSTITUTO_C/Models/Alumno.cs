using INSTITUTO_C.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INSTITUTO_C.Models
{
    public class Alumno: Persona
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


        


        
        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Range(1, 500, ErrorMessage = ErrorMesseges.Range)]
        public int NumeroMatricula { get; set; }


        //navegacional
        public Carrera Carrera { get; set; }

        public string CarreraId {  get; set; }
        
        //navegacional

        public List<Inscripcion> Inscripciones { get; set; }
        public List<Calificacion> Calificaciones { get; set; }
        
}
}
