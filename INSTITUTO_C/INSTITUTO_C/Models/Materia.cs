using System.Collections.Generic;

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
        public string Nombre { get; set; }
        public string CodigoMateria { get; set; }
        public string Descripcion { get; set; }
        public int CupoMaximo { get; set; }
        public List<MateriaCursada> cursadas { get; set; }


    }
}
