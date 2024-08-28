using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace plataformaMotVer6.Models
{
    public class TblUsuarios
    {
        public string TipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string Rol { get; set; }

       
        [DataType(DataType.Password)]
        public string ClaveActual { get; set; }

        [StringLength(15, ErrorMessage = "La contraseña debe tener entre 8 y 15 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}", ErrorMessage = "\"La contraseña debe contener al menos una letra, un número y un carácter especial, además de tener un mínimo de 8 caracteres y un máximo de 15.\"")]
        public string ClaveNueva { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmarClaveNueva { get; set; }
    }
}
