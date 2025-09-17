using INSTITUTO_C.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INSTITUTO_C.Models
{
    public class Profesor
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

        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string UserName { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMesseges.NotValid)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s']+$", ErrorMessage = ErrorMesseges.SoloLetras)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s']+$", ErrorMessage = ErrorMesseges.SoloLetras)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [RegularExpression(@"^\d+$", ErrorMessage = ErrorMesseges.SoloNumeros)]
        [StringLength(10, MinimumLength = 8, ErrorMessage = ErrorMesseges.CaracteresExactos)]
        public string DNI { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [RegularExpression(@"^[\d\s\-\(\)\+]+$", ErrorMessage = ErrorMesseges.SoloNumeros)]
        public int Telefono { get; set; }

        [StringLength(50)]
        public string Direccion { get; set; }

        public bool Activo { get; set; } = true;

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(20, MinimumLength = 3, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        public string Legajo { get; set; }

        public List<MateriaCursada> MateriasCursada { get; set; }

        public List<Calificacion> Calificaciones { get; set; }


    }
}
