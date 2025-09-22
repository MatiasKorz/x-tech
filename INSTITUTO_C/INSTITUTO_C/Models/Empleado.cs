using INSTITUTO_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace INSTITUTO_C.Models
{
    public class Empleado: Persona
    {

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(20, MinimumLength = 3, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        public string Legajo { get; set; }

    }
}
