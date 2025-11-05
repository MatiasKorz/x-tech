using INSTITUTO_C.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace INSTITUTO_C.ViewModels
{
    public class RegistroUsuario
    {




        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Display(Name = "Carrera")]
        public string CarreraNombre { get; set; }


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string DNI { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string Direccion { get; set; }


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMesseges.NotValid)]
        public string Email { get; set; }


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }



        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password",ErrorMessage = ErrorMesseges.NoMatch)]
        public string ConfirmacionPassword { get; set; }

    }
}
