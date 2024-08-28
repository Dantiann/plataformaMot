using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace plataformaMotVer6.Models.ViewModel
{
    public class RecoveryViewModel
    {
        [EmailAddress]
        [Required]
        public string Correo {  get; set; }
    }
}