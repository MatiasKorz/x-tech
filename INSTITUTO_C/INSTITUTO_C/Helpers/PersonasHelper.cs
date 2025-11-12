using INSTITUTO_C.Data;
using System;
using System.Linq;

namespace INSTITUTO_C.Helpers
{
    public class PersonasHelper
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

        public static bool PersonaDNIExists(InstitutoContext context, string dni)
        {
            if (string.IsNullOrEmpty(dni))
                return false;

            return context.Personas.Any(p => p.DNI == dni);
        }

        public static bool PersonaEmailExists(InstitutoContext context, string email)
        {
            return context.Personas.Any(p => p.Email == email);
        }
    }
}
