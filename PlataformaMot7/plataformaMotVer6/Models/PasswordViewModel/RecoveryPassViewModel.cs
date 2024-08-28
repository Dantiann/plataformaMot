using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace plataformaMotVer6.Models.ViewModel
{
    public class RecoveryPassViewModel
    {
        [EmailAddress]
        [Required]
        public string Correo {  get; set; }

        [Required]
        public string Clave { get; set; }

        public string Token_recuperacion { get; set; }

        public string FechaExpiracionToken { get; set; }

    }
}