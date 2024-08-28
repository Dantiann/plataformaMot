using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;



namespace plataformaMotVer6.Models
{
    public class TblActividadesViewModel : DeleteActivitiesViewModel
    {
        [Required(ErrorMessage = "El nombre de la actividad es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre de la actividad no debe exceder los 100 caracteres.")]
        public string NombreActividad { get; set; }

        [Required(ErrorMessage = "La categoría es requerida.")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "Las horas asignadas son requeridas.")]
        [Range(1, 40, ErrorMessage = "Las horas asignadas deben estar entre 1 y 40.")]
        public string HorasAsignadas { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        [StringLength(500, ErrorMessage = "La descripción no debe exceder los 500 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de finalización es requerida.")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [FechaFinalizacionValidaViewModel(ErrorMessage = "La fecha de finalización no puede ser anterior a la fecha de inicio.")]
        public string FechaFinalizacion { get; set; }

        [Required(ErrorMessage = "El tipo de documento es requerido.")]
         public string TipoDocumentoBienestar { get; set; }

        [Required(ErrorMessage = "El número de documento es requerido.")]
        public string NumeroDocumentoBienestar { get; set; }

        public string Responsable { get; set; }

        public string Cargo { get; set; }

        public string Boton { get; set; }

     }
}

