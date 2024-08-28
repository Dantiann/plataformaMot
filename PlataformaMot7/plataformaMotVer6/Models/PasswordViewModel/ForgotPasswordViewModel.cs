using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace plataformaMotVer6.Models.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "El campo de correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Por favor, ingresa un correo electrónico válido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required]
        public string Token_recuperacion { get; set; }

        public string Fecha_expiracion_token { get; set; }

    }
}