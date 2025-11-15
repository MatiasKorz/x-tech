using INSTITUTO_C.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INSTITUTO_C.Models
{
    public class Persona: IdentityUser<int>
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

       // public int Id { get; set; }

        //[Required(ErrorMessage = ErrorMesseges.Requerido)]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Solo letras, números y guión bajo")]
        public override string UserName { 
             get { return base.UserName; }
             set { base.UserName = value; }
        }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMesseges.NotValid)]
        public override string Email {
            get { return base.Email; }
            set { base.Email = value; }
        }

        //[Required(ErrorMessage = ErrorMesseges.Requerido)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [ScaffoldColumn(false)] // Esto le dice a ASP.NET MVC que no genere un campo en los formularios
        public DateTime FechaAlta { get; private set; } = DateTime.Now;


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
        [StringLength(10, MinimumLength = 7, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        public string DNI { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        [RegularExpression(@"^\d+$", ErrorMessage = ErrorMesseges.SoloNumeros)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "La dirección solo puede contener letras, números y espacios")]
        public string Direccion { get; set; }

        public bool Activo { get; set; } = true;
    }
}
