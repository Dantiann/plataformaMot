using plataformaMotVer6.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace plataformaMotVer6.Controllers
{
    public class ContactController : Controller
    {
        // Conexión a la base de datos. La conexión se encuentra alojada en Web.config
        static readonly string network = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;


        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }


        [Permissions("Aprendiz")]
        public ActionResult ContactApprendice()
        {
            Response.AppendHeader("Cache-Control", "no-store");

            try
            {
                List<TblBienestar> bienestarContacs = GetContacts(); // Obtener todos los contactos
                return View(bienestarContacs);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista de los contactos de Bienestar. Por favor, inténtelo de nuevo más tarde." + ex.Message; 
                return View();
            }
        }



        [Permissions("Bienestar")]
        public ActionResult ContactBienestar()
        {
            Response.AppendHeader("Cache-Control", "no-store");

            try
            {
                List<TblBienestar> bienestarContacs = GetContacts(); // Obtener todos los contactos
                return View(bienestarContacs);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista de los contactos de Bienestar. Por favor, inténtelo de nuevo más tarde." + ex.Message;
                return View();
            }
        }


        // Método para obtener todas las actividades desde la base de datos
        private List<TblBienestar> GetContacts()
        {
            List<TblBienestar> bienestarContacs = new List<TblBienestar>();

            try
            {
                // Lógica para obtener todas las actividades desde la base de datos
                using (SqlConnection cn = new SqlConnection(network))
                {
                    // Código para ejecutar la consulta SQL solo con la categoría proporcionada (spConsultarActividades)
                    cn.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = cn;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "spConsultContacts";
                        SqlDataReader registros = command.ExecuteReader();

                        while (registros.Read())
                        {
                            TblBienestar allcontacts = new TblBienestar
                            {
                                NombreCompleto = registros["nombreCompleto"].ToString(),
                                Cargo = registros["cargo"].ToString(),
                                Correo = registros["correo"].ToString(),
                            };
                            bienestarContacs.Add(allcontacts);
                        }
                    }
                    cn.Close();
                }
                return bienestarContacs;
            }
            catch (Exception ex)
            {
                // Manejar excepción
                // Loggear el error, redirigir a una página de error, etc.
                ViewBag.ErrorMessage = "Error con lista de contactos no existente: " + ex.Message;
                return null; // Devuelve null en caso de error
            }
        }
    }
}