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

        public Carrera Carrera { get; set; }


        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(10)]
        public string CodigoMateria { get; set; }
        public string Descripcion { get; set; }
        public int CupoMaximo { get; set; }
        public List<MateriaCursada> cursadas { get; set; }


    }
}
