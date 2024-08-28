using plataformaMotVer6.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Diagnostics;
using plataformaMotVer6.Models;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;
using System.ComponentModel.DataAnnotations;


namespace plataformaMotVer6.Controllers
{
    public class PasswordController : Controller
    {
        static readonly string network = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;

        // GET: RecoverPassword
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            Response.AppendHeader("Cache-Control", "no-store");
            var model = new ForgotPasswordViewModel();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string correo)
        {
            if (string.IsNullOrEmpty(correo))
            {
                ModelState.AddModelError("", "El campo de correo electrónico es obligatorio.");
                return View();

            }

            try
            {
                bool emailExists = ValidateEmail(correo);
                if (!emailExists)
                {
                    ModelState.AddModelError("", "No se encontró ninguna cuenta asociada con este correo electrónico.");
                    return View();
                }

                if (emailExists)
                {
                    string newToken = GenerateRecoveryToken(correo);

                    // Enviar el correo electrónico con el token de recuperación al usuario
                    SendEmail(correo, newToken);

                    TempData["EmailSentMessage"] = "Hemos enviado un correo de recuperación a la dirección de correo electrónico asociada a esta plataforma.";

                    return RedirectToAction("ForgotPassword", "Password");                                       
                }
                return View();
            }

            catch (Exception ex)
            {
                // Manejar la excepción
                ModelState.AddModelError("", "Error al actualizar el token de recuperación: " + ex.Message);
                return View();
            }
        }


        private bool ValidateEmail(string correo)
        {
            bool emailExists = false;

            using (SqlConnection conn = new SqlConnection(network))
            {
                conn.Open();

                // Verificar si el correo electrónico existe en la base de datos
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = conn;
                    com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.AddWithValue("@Correo", correo);
                    com.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = correo;
                    com.Parameters.Add("@EmailExists", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    com.CommandText = "spValidateEmail";
                    com.ExecuteNonQuery();

                    emailExists = (bool)com.Parameters["@EmailExists"].Value;
                }
            }
            return emailExists;
        }


        private const int RecoveryTokenExpirationMinutes = 30;
        private string GenerateRecoveryToken(string correo)
        {
            string token_recuperacion = ConvertSha256(Guid.NewGuid().ToString());
            DateTime fecha_expiracion_token = DateTime.Now.AddMinutes(RecoveryTokenExpirationMinutes);
            string fecha_expiracion_token_string = fecha_expiracion_token.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            UpdateRecoveryToken(correo, token_recuperacion, fecha_expiracion_token_string);
            return token_recuperacion;
        }
        

        private void UpdateRecoveryToken(string correo, string token_recuperacion, string fecha_expiracion_token)
        {
            using (SqlConnection conn = new SqlConnection(network))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = conn;
                        com.CommandType = CommandType.StoredProcedure;
                        //com.Parameters.AddWithValue("@Correo", correo);
                        //com.Parameters.AddWithValue("@Token_recuperacion", token_recuperacion);
                        //com.Parameters.AddWithValue("@Fecha_expiracion_token", fecha_expiracion_token);
                        com.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = correo;
                        com.Parameters.Add("@Token_recuperacion", SqlDbType.NVarChar, 250).Value = token_recuperacion;
                        com.Parameters.Add("@Fecha_expiracion_token", SqlDbType.DateTime).Value = DateTime.Parse(fecha_expiracion_token);
                        
                        com.CommandText = "spUpdateRecoveryToken";
                        com.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Manejar la excepción
                    Console.WriteLine("Error al actualizar el token de recuperación: " + ex.Message);
                }
            }
        }


        private void SendEmail(string correo, string token_recuperacion)
        {

            MailMessage Email = new MailMessage();
            Email.From = new MailAddress("mot_bienestar@outlook.com");
            Email.To.Add(correo);
            Email.Subject = ("Recuperación de contraseña MÖT-BIENESTAR");

            //string link = $"<a href='{Request.Url.Scheme}://{Request.Url.Authority}/Password/RecoverPassword?correo={HttpUtility.UrlEncode(correo)}&token_recuperacion={HttpUtility.UrlEncode(token_recuperacion)}'>Hacer clic aquí para recuperar la contraseña</a>";
            string link = $"<a href='{Url.Action("RecoverPassword", "Password", new { correo = correo, token_recuperacion = token_recuperacion }, Request.Url.Scheme)}'>Hacer clic aquí para recuperar la contraseña</a>";

            Debug.WriteLine("Recuperación de contraseña - Enlace: " + link);
            string body = $"Estimado usuario(a),<br><br>" +
                          $"A continuación encontrará el enlace de seguridad para que pueda efectuar el cambio de su contraseña. " +
                          $"Recuerde que cuenta con 30 minutos para realizar el cambio en el sistema, pasado este tiempo usted deberá realizar " +
                          $"el proceso de recuperación de la contraseña nuevamente.<br><br>" +
                          $"{link}";

            Email.Body = body;
            Email.IsBodyHtml = true; // false respeta los saltos de línea


            SmtpClient ServerEmail = new SmtpClient();
            ServerEmail.Credentials = new NetworkCredential("mot_bienestar@outlook.com", "CDM_BienestarMot2024");
            ServerEmail.Host = "smtp-mail.outlook.com";
            ServerEmail.Port = 587;
            ServerEmail.EnableSsl = true;

            try
            {
                ServerEmail.Send(Email);
                Console.WriteLine("Correo electrónico enviado correctamente a: " + correo);
                Debug.WriteLine("Correo electrónico enviado correctamente a: " + correo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Error al enviar el correo electrónico: " + ex.ToString());
                Debug.WriteLine("Error al enviar el correo electrónico: " + ex.ToString());
            }
        }


        [HttpGet]
        public ActionResult RecoverPassword(string correo, string token_recuperacion)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(token_recuperacion))
            {
                ModelState.AddModelError("", "Los campos correo electrónico y token son obligatorios.");
                return View();
            }

            var user = GetUserByEmailAndToken(correo, token_recuperacion);

            if (user == null)
            {
                ModelState.AddModelError("", "El token de recuperación no es válido o ha expirado.");
                return View();
            }

            var model = new RecoverPasswordViewModel
            {
                Correo = correo,
                Token_recuperacion = token_recuperacion
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecoverPassword(string correo, string token_recuperacion, string nuevaClave, string confirmarClave)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrEmpty(nuevaClave) || string.IsNullOrEmpty(confirmarClave))
            {
                ModelState.AddModelError("", "La nueva contraseña y la confirmación de contraseña son requeridas.");
                return View();
            }

            if (nuevaClave != confirmarClave)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden.");
                return View();
            }


            var user = GetUserByEmailAndToken(correo, token_recuperacion);

            if (user == null)
            {
                ModelState.AddModelError("", "El token de recuperación no es válido o ha expirado.");
                return View();
            }

            var passwordUpdate = UpdatePassword(correo, nuevaClave);

            if (passwordUpdate)
            {
                ViewBag.PasswordChanged = true;
                ViewBag.Message = "Su contraseña ha sido actualizada correctamente.";
                return View();
            }

            ModelState.AddModelError("", "Error al actualizar la contraseña.");
            return View();
        }


        private RecoverPasswordViewModel GetUserByEmailAndToken(string correo, string token_recuperacion)
        {
            RecoverPasswordViewModel user = null;

            using (SqlConnection conn = new SqlConnection(network))
            {
                conn.Open();

                // Verificar si el correo electrónico y el token de recuperación son válidos y no han expirado
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = conn;
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = correo;
                    com.Parameters.Add("@Token_recuperacion", SqlDbType.NVarChar, 250).Value = token_recuperacion;

                    com.CommandText = "spGetUserByEmailAndToken";

                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new RecoverPasswordViewModel
                            {
                                Correo = reader["correo"].ToString(),
                                Token_recuperacion = reader["token_recuperacion"].ToString(),
                            };
                        }
                    }
                }
            }
            return user;
        }



        private bool UpdatePassword(string correo, string nuevaClave)
        {
            bool passwordUpdated = false;

            using (SqlConnection conn = new SqlConnection(network))
            {
                conn.Open();

                // Comando SQL para actualizar la contraseña en la base de datos.
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = conn;
                    com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.AddWithValue("@Correo", usuario.Correo);
                    //com.Parameters.AddWithValue("@NuevaClave", ConvertSha256(nuevaClave));
                    com.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = correo;
                    com.Parameters.Add("@NuevaClave", SqlDbType.NVarChar, 250).Value = ConvertSha256(nuevaClave);

                    com.CommandText = "spUpdateForgottenPassword";
                    com.ExecuteNonQuery();

                    passwordUpdated = true;
                }
            }
            return passwordUpdated;
        }


        // Calcula el hash SHA256 de una cadena de texto utilizando el algoritmo SHA256Managed.
        // Retorna el hash como una cadena de texto en formato hexadecimal.
        // El hash se utiliza para proteger la confidencialidad de la información, ya que no es posible deducir la cadena original a partir del valor de hash.
        public static string ConvertSha256(string text)
        {
            //using System.Text;
            //USAR LA REFERENCIA DE "System.Security.Cryptography"
            //https://emn178.github.io/online-tools/sha256.html 

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(text));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }


        public ActionResult Exit()
        {
            //Limpiar la sesión
            Session["usuario"] = null;

            // Redirigir al usuario a la página de inicio de sesión
            return RedirectToAction("Home", "Home");
        }
    }
}