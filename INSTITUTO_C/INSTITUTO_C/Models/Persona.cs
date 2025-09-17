using INSTITUTO_C.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(15)]
        [RegularExpression(@"^\d+$", ErrorMessage = ErrorMesseges.SoloNumeros)]
        public int Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        public bool Activo { get; set; } = true;
    }
}
