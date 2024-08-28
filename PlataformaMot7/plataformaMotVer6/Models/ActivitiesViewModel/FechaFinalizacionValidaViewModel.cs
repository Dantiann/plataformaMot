using plataformaMotVer6.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace plataformaMotVer6.Models
{
    public class FechaFinalizacionValidaViewModel : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Obtener el objeto que se está validando
            var model = validationContext.ObjectInstance;

            // Obtener las propiedades FechaInicio y FechaFinalizacion usando reflexión
            var fechaInicioProperty = model.GetType().GetProperty("FechaInicio");
            var fechaFinalizacionProperty = model.GetType().GetProperty("FechaFinalizacion");

            if (fechaInicioProperty == null || fechaFinalizacionProperty == null)
            {
                return new ValidationResult("Las propiedades FechaInicio y FechaFinalizacion no existen en el modelo.");
            }

            // Obtener los valores de las propiedades
            var fechaInicioValue = fechaInicioProperty.GetValue(model) as string;
            var fechaFinalizacionValue = fechaFinalizacionProperty.GetValue(model) as string;

            // Declarar e inicializar las variables en una sola línea usando DateTime.TryParse
            if (DateTime.TryParse(fechaInicioValue, out DateTime fechaInicio) &&
                DateTime.TryParse(fechaFinalizacionValue, out DateTime fechaFinalizacion))
            {
                if (fechaFinalizacion < fechaInicio)
                {
                    return new ValidationResult("La fecha de finalización no puede ser anterior a la fecha de inicio.");
                }
            }

            return ValidationResult.Success;
        }
    }
}