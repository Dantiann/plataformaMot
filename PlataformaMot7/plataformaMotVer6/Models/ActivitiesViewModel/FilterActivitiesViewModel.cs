using plataformaMotVer6.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace plataformaMotVer6.Models
{
    public class FilterActivitiesViewModel
    {
        public string Categoria { get; set; }
              
       
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string FechaInicio { get; set; }

       
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [FechaFinalizacionValidaViewModel(ErrorMessage = "La fecha de finalización no puede ser anterior a la fecha de inicio.")]
        public string FechaFinalizacion { get; set; }
    }
}

