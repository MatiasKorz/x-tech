using INSTITUTO_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace INSTITUTO_C.Models
{
    public class Empleado: Persona
    {

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        //elimine el range xq deberia ser autogenerado
        public string Legajo { get; set; }

    }
}
