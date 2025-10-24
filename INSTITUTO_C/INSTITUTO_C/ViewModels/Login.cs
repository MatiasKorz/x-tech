using INSTITUTO_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace INSTITUTO_C.ViewModels
{
    public class Login
    {


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMesseges.NotValid)]
        public string Email { get; set; }


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }



        public bool Recordarme { get; set; }




    }
}
