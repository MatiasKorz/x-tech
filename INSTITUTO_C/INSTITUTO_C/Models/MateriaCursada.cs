using INSTITUTO_C.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INSTITUTO_C.Models
{
    public enum Cuatrimestre
    {
        Primero = 1, // 1C
        Segundo = 2  // 2C
    }

    public class MateriaCursada
    {
        public int Id { get; set; }


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public int MateriaId { get; set; }
        public Materia Materia { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(1, MinimumLength = 1, ErrorMessage = ErrorMesseges.CaracteresExactos)]
        [RegularExpression("^[A-Z]$", ErrorMessage = ErrorMesseges.SoloLetras)]
        public string CodigoCursada { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public int Anio { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public Cuatrimestre Cuatrimestre { get; set; }

        public bool Activo { get; set; }

        public Profesor Profesor { get; set; }

        public List<Inscripcion> Inscripciones { get; set; }

    }
}
