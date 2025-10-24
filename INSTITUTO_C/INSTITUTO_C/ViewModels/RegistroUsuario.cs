using INSTITUTO_C.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace INSTITUTO_C.ViewModels
{
    public class RegistroUsuario
    {

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
