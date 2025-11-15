namespace INSTITUTO_C.Helpers
{
    public static class ErrorMesseges
    {
        public const string Requerido = "El campo {0} es requerido";

        public const string SoloLetras = "El campo {0} solo puede contener letras y espacios.";

        public const string SoloLetrasNumEsp = "El campo {0} solo puede contener letras, numeros y espacios.";

        public const string CaracteresMinMax = "El campo {0} tiene que tener entre {2} y {1} caracteres.";

        public const string Range = "El campo {0} tiene que estar entre {1} y {2}.";

        public const string SoloNumeros = "El campo {0} solo acepta numeros";

        public const string CaracteresExactos = "El campo {0} solo acepta {1} caracteres";

        public const string NotValid = "{0} invalido";

        public const string SoloMayusculasYNumeros = "El campo {0} solo puede contener letras mayúsculas y números.";

        public const string NoMatch = "Las contraseñas no coinciden";

        public const string CarreraNombre = "Ya existe una carrera con ese nombre";

        public const string CodigoEnUso = "El codigo de materia ya esta en uso";

        public const string DNIExistente = "Ya existe una persona con ese DNI";

        public const string EmailExistente = "Ya existe un usuario con ese Email";

        public const string AlumnoInactivo = "No es posible la inscripción, el alumno no está activo.";

        public const string AlumnoEnCursada = "El alumno ya esta inscripto en esa cursada";

        public const string AlumnoNoCarrera = "El alumno no pertenece a esa carrera";

        public const string NoEsElProfe = "Solo el profesor titular de la cursada puede hacer la calificación";

        public const string CursadaConInscripciones = "No se puede eliminar la materia cursada porque tiene inscripciones asociadas.";

        public const string CursadaDuplicada = "No se puede crear 2 veces la misma cursada.";

        public const string CursadaFinalizada = "No se puede calificar porque la cursada ha finalizado.";
    }
}
