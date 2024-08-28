using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace plataformaMotVer6.Models.ViewModel
{
    public class RecoverPasswordViewModel
    {
        //[Required(ErrorMessage = "El correo electrónico es requerido")]
        //[EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        //[Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El token de recuperación es requerido")]
        [Remote("ValidateRecoveryToken", "Password", ErrorMessage = "El token de recuperación no es válido")]
        [Display(Name = "Token de recuperación")]
        public string Token_recuperacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [StringLength(15, ErrorMessage = "La contraseña debe tener máximo 15 caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,15}", ErrorMessage = "La contraseña debe tener al menos una letra, un número y un carácter especial.")]
        public string NuevaClave { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [System.Web.Mvc.Compare("NuevaClave", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarClave { get; set; }

    }
}