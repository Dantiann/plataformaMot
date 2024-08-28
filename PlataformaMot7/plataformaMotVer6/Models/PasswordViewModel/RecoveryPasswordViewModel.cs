using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace plataformaMotVer6.Models.ViewModel
{
    
    public class RecoveryPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Token_recuperacion { get; set; }

        public string FechaExpiracionToken { get; set; }

    }
}