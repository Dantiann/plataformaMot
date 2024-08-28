using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using plataformaMotVer6.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.Remoting.Messaging;
using System.Web.Management;
using System.Web.UI.WebControls;

// Working 3/04/2024

namespace plataformaMotVer6.Controllers
{
    public class HomeController : Controller
    {
        static readonly string network = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;

        public ActionResult Home()
        {
            Response.AppendHeader("Cache-Control", "no-store");
            return View();
        }

        

        public ActionResult Login()
        {
            Response.AppendHeader("Cache-Control", "no-store");

            // Invocar método para cargar la lista de tipos de documento 
            InitializeTipoDocumento();

            return View();
        }


        public ActionResult Index()
        {
            Response.AppendHeader("Cache-Control", "no-store");
            return View();
        }


        public ActionResult About()
        {
            Response.AppendHeader("Cache-Control", "no-store");
            ViewBag.Message = "Your application description page.";
            return View();
        }


        [HttpPost]
        public ActionResult Login(TblUsuarios loginUser)
        {
            if (!string.IsNullOrEmpty(loginUser.TipoDocumento) && !string.IsNullOrEmpty(loginUser.NumeroDocumento) && !string.IsNullOrEmpty(loginUser.Clave))
            {
                loginUser.Clave = ConvertSha256(loginUser.Clave);

                using (SqlConnection conection = new SqlConnection(network))
                {

                    SqlCommand command = new SqlCommand("spValidateUser", conection);
                    command.Parameters.AddWithValue("TipoDocumento", loginUser.TipoDocumento);
                    command.Parameters.AddWithValue("NumeroDocumento", loginUser.NumeroDocumento);
                    command.Parameters.AddWithValue("Clave", loginUser.Clave);
                    command.CommandType = CommandType.StoredProcedure;

                    conection.Open();

                    using (SqlDataReader dateRead = command.ExecuteReader())
                    {
                        while (dateRead.Read())
                        {
                            if (!string.IsNullOrEmpty(dateRead[0].ToString()))
                            {
                                loginUser.TipoDocumento = dateRead["tipoDocumento"].ToString();
                                loginUser.NumeroDocumento = dateRead["numeroDocumento"].ToString();
                                loginUser.Nombres = dateRead["nombres"].ToString();
                                loginUser.Apellidos = dateRead["apellidos"].ToString();
                                loginUser.Correo = dateRead["correo"].ToString();
                                loginUser.Clave = dateRead["clave"].ToString();
                                loginUser.Rol = dateRead["rol"].ToString();

                                conection.Close();

                                Session["usuario"] = loginUser;

                                if (loginUser.Rol == "Aprendiz")
                                {
                                    return RedirectToAction("Activities", "ActivitiesApprendice");
                                }
                                else if (loginUser.Rol == "Bienestar")
                                {
                                    return RedirectToAction("Activities", "ActivitiesBienestar");
                                }
                                else if (loginUser.Rol == "Administrador")
                                {
                                    return RedirectToAction("Index", "ActivitiesBienestar");
                                }
                            }
                        }
                    }
                }

                // Invocar método para volver a cargar la lista de tipos de documento en caso de campos incorrectos
                InitializeTipoDocumento();

                ViewData["Mensaje"] = "\"Datos incorrectos. Por favor, inténtalo de nuevo.\"";
            }
            else
            {
                // Invocar método para volver a cargar la lista de tipos de documento en caso de campos vacíos
                InitializeTipoDocumento();

                ViewData["Mensaje"] = "\"Todos los campos son obligatorios.  Por favor, inténtalo de nuevo.\"";
                }
                return View();
        }


        private void InitializeTipoDocumento()
        {
            ViewBag.TipoDocumento = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "CC", Text = "Cédula de Ciudadanía"},
                new SelectListItem { Value = "CE", Text = "Cédula de Extranjería"},
                new SelectListItem { Value = "TI", Text = "Tarjeta de Identidad"},
                new SelectListItem { Value = "PEP", Text = "Permiso Especial de Permanencia"},
                new SelectListItem { Value = "PPT", Text = "Permiso de Protección Temporal"}
            }, "Value", "Text");
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