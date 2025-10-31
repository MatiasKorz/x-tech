using INSTITUTO_C.Data;
using System.Linq;


namespace INSTITUTO_C.Helpers
{
    public class AlumnoHelper
    {

        public static string GenerarNumMatricula(InstitutoContext context)
        {
            int maxMatricula = 0;

            var matriculasNumericas = context.Alumnos
                .AsEnumerable()
                .Select(a => int.TryParse(a.NumeroMatricula, out int parsed) ? parsed : 0)
                .ToList();

            if (matriculasNumericas.Any())
            {
                maxMatricula = matriculasNumericas.Max();
            }

            return (maxMatricula + 1).ToString();
        }
    }
}
