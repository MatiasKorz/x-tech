using System;
using System.Collections.Generic;

namespace INSTITUTO_C.Models
{
    public class Persona
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

        public int id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime FechaAlta { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string DNI { get; set; }

        public int Telefono { get; set; }

        public string Direccion { get; set; }

        public bool Activo { get; set; }
    }
}
