using INSTITUTO_C.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INSTITUTO_C.Models
{
    public class Profesor: Empleado
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
        //- Legajo
        //- MateriasCursada(Designado)
        //- Calificaciones(Realizadas)

        public List<MateriaCursada> MateriasCursada { get; set; }

        public List<Calificacion> Calificaciones { get; set; }


    }
}
