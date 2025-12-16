using INSTITUTO_C.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INSTITUTO_C.Models
{
    public class Materia
    {
        //- Carrera
        //- Nombre(Programación en nuevas tecnologías 1)
        //- CodigoMateria(PNT1)
        //- Descripcion 
        //- CupoMaximo
        //- Cursadas

        public int Id { get; set; }

        public Carrera Carrera { get; set; }


        [Display(Name = "Carrera")]
        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public int CarreraId { get; set; }


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(50, MinimumLength = 5)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(10, MinimumLength = 2, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = ErrorMesseges.SoloMayusculasYNumeros)]
        [Display(Name = "Codigo de Materia")]
        public string CodigoMateria { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Range(1, 500, ErrorMessage = ErrorMesseges.Range)]
        [Display(Name = "Cupo maximo")]
        public int CupoMaximo { get; set; }

        public List<MateriaCursada> cursadas { get; set; }


    }
}
