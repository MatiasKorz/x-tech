using INSTITUTO_C.Data;
using System.Linq;

namespace INSTITUTO_C.Helpers
{
    public class EmpleadoHelper
    {
        public static string GenerarLegajo(InstitutoContext context)
        {
            int maxLegajo = 0;

            var legajosNumericos = context.Empleados
                .AsEnumerable()
                .Select(e => int.TryParse(e.Legajo, out int parsed) ? parsed : 0)
                .ToList();

            if (legajosNumericos.Any())
            {
                maxLegajo = legajosNumericos.Max();
            }

            return (maxLegajo + 1).ToString();
        }
    }
}