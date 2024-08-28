using plataformaMotVer6.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace plataformaMotVer6.Controllers
{
    public class SecurityController : Controller
    {
        static readonly string network = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;

        // GET: Security
        public ActionResult Index()
        {
            Response.AppendHeader("Cache-Control", "no-store");
            return View();
        }


        //GET: Login/ChangePassword/Aprendiz
        [Permissions("Aprendiz")]
        public ActionResult ChangepassApprendice()
        {
            Response.AppendHeader("Cache-Control", "no-store");
            return View(new TblUsuarios());
        }


        [HttpPost]
        public ActionResult ChangepassApprendice(TblUsuarios model)
        {
            Response.AppendHeader("Cache-Control", "no-store");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TblUsuarios user = Session["Usuario"] as TblUsuarios;

            if (user == null)
            {
                return RedirectToAction("Home", "Home");
            }

            model.TipoDocumento = user.TipoDocumento;
            model.NumeroDocumento = user.NumeroDocumento;

            try
            {
                // Validar que los valores necesarios (claveActual) esté presente
                if (string.IsNullOrEmpty(model.ClaveActual) || string.IsNullOrEmpty(model.ClaveNueva) || string.IsNullOrEmpty(model.ConfirmarClaveNueva))

                {
                    ModelState.AddModelError("", "Los datos de los campos solicitados son obligatorios.");
                    return View(model);
                }

                string claveActualSha256 = ConvertSha256(model.ClaveActual);

                if (string.IsNullOrEmpty(model.ClaveActual))
                {
                    ModelState.AddModelError("", "Por favor ingresa la contraseña actual.");
                    return View(model);
                }
                 

                if (!CheckCurrentPassword(claveActualSha256, model.TipoDocumento, model.NumeroDocumento))
                {
                    ModelState.AddModelError("", "La contraseña actual es incorrecta. Por favor, inténtalo de nuevo.");
                    return View(model);
                }

                if (model.ClaveActual == model.ClaveNueva)
                {
                    ModelState.AddModelError("", "La nueva contraseña no puede ser igual a la actual. Por favor, inténtalo de nuevo.");
                    return View(model);
                }

                if (model.ClaveNueva != model.ConfirmarClaveNueva)
                {
                    ModelState.AddModelError("", "La nueva contraseña y su confirmación no son idénticas.");
                    return View(model);
                }

                string claveNuevaSha256 = ConvertSha256(model.ClaveNueva);

                try
                {                   
                    UpdatePassword(claveActualSha256, claveNuevaSha256, model.TipoDocumento, model.NumeroDocumento);
                    ViewBag.SuccessMessage = "La contraseña ha sido cambiada exitosamente.";
                    CerrarSesion();
                }
                catch (HttpRequestValidationException)
                {
                    ModelState.AddModelError("", "La contraseña ingresada no es válida.");
                }

                return View(/*"ChangepassApprendice", */model);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar la contraseña: " + ex.Message);
                return View(model);
            }
        }


        //GET: Login/ChangePassword/Bienestar
        [Permissions("Bienestar")]
        public ActionResult ChangepassBienestar()
        {
            Response.AppendHeader("Cache-Control", "no-store");
            return View(new TblUsuarios());
        }


        [HttpPost]
        public ActionResult ChangepassBienestar(TblUsuarios model)
        {
            Response.AppendHeader("Cache-Control", "no-store");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TblUsuarios user = Session["Usuario"] as TblUsuarios;

            if (user == null)
            {
                return RedirectToAction("Home", "Home");
            }

            model.TipoDocumento = user.TipoDocumento;
            model.NumeroDocumento = user.NumeroDocumento;

            try
            {
                // Validar que los valores necesarios (claveActual) esté presente
                if (string.IsNullOrEmpty(model.ClaveActual) || string.IsNullOrEmpty(model.ClaveNueva) || string.IsNullOrEmpty(model.ConfirmarClaveNueva))

                {
                    ModelState.AddModelError("", "Los datos de los campos solicitados son obligatorios.");
                    return View(model);
                }

                string claveActualSha256 = ConvertSha256(model.ClaveActual);

                if (string.IsNullOrEmpty(model.ClaveActual))
                {
                    ModelState.AddModelError("", "Por favor ingresa la contraseña actual.");
                    return View(model);
                }

                //if (!CheckCurrentPassword(claveActualSha256, model.TipoDocumento, model.NumeroDocumento))
                if (!CheckCurrentPassword(claveActualSha256, model.TipoDocumento, model.NumeroDocumento))

                {
                    ModelState.AddModelError("", "La contraseña actual es incorrecta. Por favor, inténtalo de nuevo.");
                    return View(model);
                }

                if (model.ClaveActual == model.ClaveNueva)
                {
                    ModelState.AddModelError("", "La nueva contraseña no puede ser igual a la actual. Por favor, inténtalo de nuevo.");
                    return View(model);
                }

                if (model.ClaveNueva != model.ConfirmarClaveNueva)
                {
                    ModelState.AddModelError("", "La nueva contraseña y su confirmación no son idénticas.");
                    return View(model);
                }

                try
                {
                    string claveNuevaSha256 = ConvertSha256(model.ClaveNueva);
                    UpdatePassword(claveActualSha256, claveNuevaSha256, model.TipoDocumento, model.NumeroDocumento);
                    ViewBag.SuccessMessage = "La contraseña ha sido cambiada exitosamente.";
                    CerrarSesion();
                }
                catch (HttpRequestValidationException)
                {
                    ModelState.AddModelError("", "La contraseña ingresada no es válida.");
                }

                return View("ChangepassBienestar", model);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar la contraseña: " + ex.Message);
                return View(model);
            }
        }


        private bool CheckCurrentPassword(string claveActualSha256, string tipoDocumento, string numeroDocumento)
        {
            bool passwordExists = false;

            using (SqlConnection connection = new SqlConnection(network))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ClaveActual", SqlDbType.NVarChar).Value = claveActualSha256;
                    command.Parameters.Add("@TipoDocumento", SqlDbType.NVarChar).Value = tipoDocumento;
                    command.Parameters.Add("@NumeroDocumento", SqlDbType.NVarChar).Value = numeroDocumento;
                    command.Parameters.Add("@PasswordExists", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    command.CommandText = "spCheckCurrentPassword";
                    command.ExecuteNonQuery();

                    passwordExists = (bool)command.Parameters["@PasswordExists"].Value;
                }
            }

            return passwordExists;
        }


        private void UpdatePassword(string claveActualSha256, string claveNuevaSha256, string tipoDocumento, string numeroDocumento)
        {
            using (SqlConnection connection = new SqlConnection(network))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ClaveActual", SqlDbType.NVarChar).Value = claveActualSha256;
                    command.Parameters.Add("@ClaveNueva", SqlDbType.NVarChar).Value = claveNuevaSha256;
                    command.Parameters.Add("@TipoDocumento", SqlDbType.NVarChar).Value = tipoDocumento;
                    command.Parameters.Add("@NumeroDocumento", SqlDbType.NVarChar).Value = numeroDocumento;

                    command.CommandText = "spUpdatePassword";
                    command.ExecuteNonQuery();
                }
            }
        }


        private void CerrarSesion()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
        }


        public static string ConvertSha256(string text)
        {
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
    }
}

