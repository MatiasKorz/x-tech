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

        public string Id { get; set; }

        //navega
        public Carrera Carrera { get; set; }
        //navega


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(50, MinimumLength = 5)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        public string CodigoMateria { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Range(1, 500, ErrorMessage = ErrorMesseges.Range)]
        public int CupoMaximo { get; set; }
        public List<MateriaCursada> cursadas { get; set; }


    }
}
