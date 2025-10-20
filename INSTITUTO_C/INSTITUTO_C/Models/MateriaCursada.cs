using INSTITUTO_C.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INSTITUTO_C.Models
{ 

    public class MateriaCursada
    {
        public int Id { get; set; }



        [Display(Name = "Codigo de Materia")]
        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public int MateriaId { get; set; }

        public Materia Materia { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(1, MinimumLength = 1, ErrorMessage = ErrorMesseges.CaracteresExactos)]
        [RegularExpression("^[A-Z]$", ErrorMessage = ErrorMesseges.SoloLetras)]
        public string CodigoCursada { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Display(Name = "Año")]
        [Range(2020, 2050, ErrorMessage = ErrorMesseges.Range)]
        public int Anio { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Range(1, 2, ErrorMessage = ErrorMesseges.Range)]
        public int Cuatrimestre { get; set; }

        public bool Activo { get; set; }

        //relacional
        [Display(Name = "Profesor")]
        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public int ProfesorId { get; set; }

        public Profesor Profesor { get; set; }

        public List<Inscripcion> Inscripciones { get; set; }

    }
}
