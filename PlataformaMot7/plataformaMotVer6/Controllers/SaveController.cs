using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using plataformaMotVer6.Models;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace plataformaMotVer6.Controllers
{
    [Permissions("Bienestar")]
    public class SaveController : Controller
    {
        static readonly string network = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;

        // GET: SaveApprentices
        public ActionResult SaveApprentices()
        {
            Response.AppendHeader("Cache-Control", "no-store");
            return View();
        }

        // POST: SaveFile
        [HttpPost]
        public ActionResult SaveFile(TblFichas newFile)
        {
            if (!string.IsNullOrEmpty(newFile.FichaNumero) && !string.IsNullOrEmpty(newFile.NombrePrograma) && !string.IsNullOrEmpty(newFile.FechaInicio) && !string.IsNullOrEmpty(newFile.FechaFinalizacion) && !string.IsNullOrEmpty(newFile.Jornada))
            {
                try
                {
                    using (SqlConnection conection = new SqlConnection(network))
                    {

                        SqlCommand command = new SqlCommand("spInsertFile", conection);
                        command.Parameters.AddWithValue("fichaNumero", newFile.FichaNumero);
                        command.Parameters.AddWithValue("nombrePrograma", newFile.NombrePrograma);
                        command.Parameters.AddWithValue("fechaInicio", newFile.FechaInicio);
                        command.Parameters.AddWithValue("fechaFinalizacion", newFile.FechaFinalizacion);
                        command.Parameters.AddWithValue("jornada", newFile.Jornada);
                        command.CommandType = CommandType.StoredProcedure;

                        conection.Open();
                        command.ExecuteNonQuery();
                        TempData["MensajeFicha"] = "\"La ficha se ha creado satisfactoriamente.\"";
                        return RedirectToAction("SaveApprentices");
                    }
                }
                catch (Exception ex)
                {
                    TempData["MensajeFicha"] = "\"La ficha que está intentando crear ya existe.\"" + ex ;
                    return RedirectToAction("SaveApprentices");
                }

            }
            TempData["MensajeFicha"] = "\"Todos los campos son obligatorios.  Por favor, inténtalo de nuevo.\"";
            return RedirectToAction("SaveApprentices");
        }


        // POST: SaveApprentices
        [HttpPost]
        public ActionResult SaveApprentices(HttpPostedFileBase fileIn)
        {
            if (fileIn != null && fileIn.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileIn.FileName);
                if (extension == ".csv")
                {
                    try
                    {
                        using (var reader = new StreamReader(fileIn.InputStream, Encoding.UTF8))
                        {
                            string linea;
                            while ((linea = reader.ReadLine()) != null)
                            {
                                string[] valores = linea.Split(';');
                                string[] nombrescomp = valores[2].Split(' ');

                                if (nombrescomp.Length > 0 && nombrescomp[0] != " ")
                                {
                                    string clave = nombrescomp[0] + "123";
                                    clave = GetSha256(clave);

                                    using (SqlConnection conection = new SqlConnection(network))
                                    {
                                        SqlCommand com = new SqlCommand("spInsertApprentice", conection);
                                        com.Parameters.AddWithValue("TipoDocumento", valores[0]);
                                        com.Parameters.AddWithValue("NumeroDocumento", valores[1]);
                                        com.Parameters.AddWithValue("Nombres", valores[2]);
                                        com.Parameters.AddWithValue("Apellidos", valores[3]);
                                        com.Parameters.AddWithValue("Correo", valores[4]);
                                        com.Parameters.AddWithValue("Clave", clave);
                                        com.Parameters.AddWithValue("CopiaFichaNumero", valores[5]);
                                        com.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                                        com.Parameters.Add("Mensaje", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
                                        com.CommandType = CommandType.StoredProcedure;

                                        conection.Open();
                                        com.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    ViewData["Mensaje"] = "\"Hubo inconvenientes al cargar algunos datos\"";
                                }
                            }
                        }
                        ViewData["Exito"] = "\"Los datos de los aprendices fueron cargados con éxito en la base de datos\"";

                    }
                    catch (Exception ex)
                    {
                        // Maneja cualquier excepción que pueda ocurrir durante la lectura del archivo
                        // Aquí podrías implementar tu propia lógica de manejo de errores
                        ViewData["Mensaje"] = "\"Ocurrió un error en la lectura del archivo a cargar en la base de datos\"" + ex;
                    }
                }
                else
                {
                    ViewData["Mensaje"] = "\"Por favor verifique que la extensión del archivo a cargar en la base de datos sea .csv\"";
                }
            }
            else { ViewData["Mensaje"] = "\"Por favor seleccione un archivo .csv para cargar a la base de datos\""; }
            return View();
        }

        public static string GetSha256(string text)
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

    }
}
