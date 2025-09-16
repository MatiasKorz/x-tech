using INSTITUTO_C.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INSTITUTO_C.Models
{
    public class Alumno
    {
        //- UserName
        //- Email
        //- FechaAlta
        //- Nombre
        //- Apellido
        //- DNI
        //- Telefono
        //- Direccion
        //- Activo
        //- NumeroMatricula
        //- Carrera
        //- Inscripciones
        //- Calificaciones


        [Key,ForeignKey("Carrera")]
        public string Id { get; set; }


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string UserName { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMesseges.NotValid)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s' ]+$", ErrorMessage = ErrorMesseges.SoloLetras)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = ErrorMesseges.CaracteresMinMax)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s' ]+$", ErrorMessage = ErrorMesseges.SoloLetras)]
        public string Apellido { get; set; }



        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [RegularExpression(@"^\d+$", ErrorMessage = ErrorMesseges.SoloNumeros)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = ErrorMesseges.CaracteresExactos)] // Exactamente 8 caracteres
        public string DNI { get; set; }


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [RegularExpression(@"^\d+$", ErrorMessage = ErrorMesseges.SoloNumeros)]
        public string Telefono { get; set; }


        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        public string Direccion { get; set; }

        //[Required(ErrorMessage = ErrorMesseges.Requerido)] dice chatgpt que no va
        public bool Activo { get; set; }

        [Required(ErrorMessage = ErrorMesseges.Requerido)]
        [Range(1, 500, ErrorMessage = ErrorMesseges.Range)]
        public int NumeroMatricula { get; set; }


        //navegacional
        public Carrera Carrera { get; set; }

        
        //navegacional

        public List<Inscripcion> Inscripciones { get; set; }
        public List<Calificacion> Calificaciones { get; set; }
        
}
}
