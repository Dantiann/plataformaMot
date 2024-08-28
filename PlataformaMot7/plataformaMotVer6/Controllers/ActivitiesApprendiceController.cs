using plataformaMotVer6.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;

namespace plataformaMotVer6.Controllers
{
    [Permissions("Aprendiz")]
    public class ActivitiesApprendiceController : Controller
    {
        // Conexión a la base de datos. La conexión se encuentra alojada en Web.config
        static readonly string network = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;


        // GET: Muestra todas las actividades
        public ActionResult Activities()
        {
            Response.AppendHeader("Cache-Control", "no-store");

            ViewBag.Categorias = new SelectList(new List<string> { "Bienestar", "Cultura", "Deportes", "Salud", "Psicosocial" });

            try
            {
                List<TblActividadesViewModel> activitiesList = GetActivities(); // Obtener todas las actividades
                return View(activitiesList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista de actividades. Por favor, inténtelo de nuevo más tarde." + ex.Message;
                return View();
            }
        }

        // Método para obtener todas las actividades desde la base de datos
        private List<TblActividadesViewModel> GetActivities()
        {
            List<TblActividadesViewModel> activitiesList = new List<TblActividadesViewModel>();

            try
            {
                // Lógica para obtener todas las actividades desde la base de datos
                using (SqlConnection connection = new SqlConnection(network))
                {
                    // Código para ejecutar la consulta SQL solo con la categoría proporcionada (spConsultarActividades)
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "spConsultAllActivities";
                        SqlDataReader registros = command.ExecuteReader();

                        while (registros.Read())
                        {
                            TblActividadesViewModel allActivities = new TblActividadesViewModel
                            {
                                IdActividad = registros["idActividad"].ToString(),
                                NombreActividad = registros["nombreActividad"].ToString(),
                                Categoria = registros["categoria"].ToString(),
                                HorasAsignadas = registros["horasAsignadas"].ToString(),
                                Descripcion = registros["descripcion"].ToString(),
                                FechaInicio = registros["fechaInicio"].ToString(),
                                FechaFinalizacion = registros["fechaFinalizacion"].ToString(),
                                Responsable = registros["responsable"].ToString(),
                                Cargo = registros["cargo"].ToString()
                            };
                            activitiesList.Add(allActivities);
                        }
                    }
                }
                return activitiesList;
            }
            catch (Exception ex)
            {
                // Manejar excepción
                // Loggear el error, redirigir a una página de error, etc.
                ViewBag.ErrorMessage = "Error con lista de actividades no existente: " + ex.Message;
                return null; // Devuelve null en caso de error
            }
        }


        // GET: Muestra actividades filtradas por categoria
        [HttpGet]
        public ActionResult ActivitiesByCategory(string category)
        {
            ViewBag.Categorias = new SelectList(new List<string> { "Bienestar", "Cultura", "Deportes", "Salud", "Psicosocial" });

            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction("Activities"); ; // Aquí debería devolver la vista correspondiente a ActivitiesByCategory
            }

            try
            {
                List<TblActividadesViewModel> activitiesList = GetActivitiesByCategory(category); // Obtener actividades filtradas por categoría
                return View("Activities", activitiesList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista filtrada por categoría. Por favor, inténtelo de nuevo más tarde." + ex.Message;
                return View();
            }
        }


        //Método para obtener actividades filtradas por categoría desde la base de datos
        private List<TblActividadesViewModel> GetActivitiesByCategory(string category)
        {
            List<TblActividadesViewModel> activitiesList = new List<TblActividadesViewModel>();

            //Lógica para obtener actividades filtradas por categoría desde la base de datos

            try
            {
                using (SqlConnection connection = new SqlConnection(network))
                {
                    //Código para ejecutar la consulta SQL solo con la categoría proporcionada(spConsultarActividadesPorCategoria)
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "spConsultActivityByCategory";
                        command.Parameters.AddWithValue("@Categoria", category);
                        SqlDataReader registros = command.ExecuteReader();

                        while (registros.Read())
                        {
                            TblActividadesViewModel allActivities = new TblActividadesViewModel
                            {
                                IdActividad = registros["idActividad"].ToString(),
                                NombreActividad = registros["nombreActividad"].ToString(),
                                Categoria = registros["categoria"].ToString(),
                                HorasAsignadas = registros["horasAsignadas"].ToString(),
                                Descripcion = registros["descripcion"].ToString(),
                                FechaInicio = registros["fechaInicio"].ToString(),
                                FechaFinalizacion = registros["fechaFinalizacion"].ToString(),
                                Responsable = registros["responsable"].ToString(),
                                Cargo = registros["cargo"].ToString()
                            };
                            activitiesList.Add(allActivities);
                        }
                    }
                }
                return activitiesList;
            }
            catch (Exception ex)
            {
                // Manejar excepción
                // Loggear el error, redirigir a una página de error, etc.
                ViewBag.ErrorMessage = "Error con categoría no existente: " + ex.Message;
                return null; // Devuelve null en caso de error
            }
        }


        // GET: Muestra actividades filtradas por rango de fechas
        [HttpGet]
        public ActionResult ActivitiesByDateRange(string fechaInicio, string fechaFinalizacion)
        {
            ViewBag.Categorias = new SelectList(new List<string> { "Bienestar", "Cultura", "Deportes", "Salud", "Psicosocial" });

            if (string.IsNullOrEmpty(fechaInicio) || string.IsNullOrEmpty(fechaFinalizacion))
            {
                // No se proporcionaron fechas, redirige a la vista de todas las actividades.
                return RedirectToAction("Activities");
            }

            try
            {
                List<TblActividadesViewModel> activitiesList = GetActivitiesByDateRange(fechaInicio, fechaFinalizacion); // Obtener actividades filtradas por fecha
                return View("Activities", activitiesList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista filtrada por rango de fechas. Por favor, inténtelo de nuevo más tarde." + ex.Message;
                return View();
            }
        }


        // Método para obtener actividades filtradas por rango de fechas desde la base de datos
        private List<TblActividadesViewModel> GetActivitiesByDateRange(string fechaInicio, string fechaFinalizacion)
        {
            List<TblActividadesViewModel> activitiesList = new List<TblActividadesViewModel>();

            // Lógica para obtener actividades filtradas por rango de fechas desde la base de datos
            try
            {
                using (SqlConnection connection = new SqlConnection(network))
                {
                    // Código para ejecutar la consulta SQL solo con el rango de fechas proporcionado (spConsultarActividadesFecha)
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "spConsultActivityByDate";
                        command.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = DateTime.Parse(fechaInicio);
                        command.Parameters.Add("@FechaFinalizacion", SqlDbType.DateTime).Value = DateTime.Parse(fechaFinalizacion);

                        SqlDataReader registros = command.ExecuteReader();

                        while (registros.Read())
                        {
                            TblActividadesViewModel allActivities = new TblActividadesViewModel
                            {
                                IdActividad = registros["idActividad"].ToString(),
                                NombreActividad = registros["nombreActividad"].ToString(),
                                Categoria = registros["categoria"].ToString(),
                                HorasAsignadas = registros["horasAsignadas"].ToString(),
                                Descripcion = registros["descripcion"].ToString(),
                                FechaInicio = registros["fechaInicio"].ToString(),
                                FechaFinalizacion = registros["fechaFinalizacion"].ToString(),
                                Responsable = registros["responsable"].ToString(),
                                Cargo = registros["cargo"].ToString()
                            };
                            activitiesList.Add(allActivities);
                        }
                    }
                }
                return activitiesList;
            }
            catch (Exception ex)
            {
                // Manejar excepción
                // Loggear el error, redirigir a una página de error, etc.
                ViewBag.ErrorMessage = "Error con fechas no existentes: " + ex.Message;
                return null; // Devuelve null en caso de error
            }
        }


        // GET: Muestra actividades filtradas por categoria y fecha
        [HttpGet]
        public ActionResult ActivitiesByCategoryAndDate(string category, string fechaInicio, string fechaFinalizacion)
        {
            Response.AppendHeader("Cache-Control", "no-store");

            ViewBag.Categorias = new SelectList(new List<string> { "Bienestar", "Cultura", "Deportes", "Salud", "Psicosocial" });


            // Redireccionar según los parámetros recibidos

            // Si todos los parámetros son nulos o vacíos, redirige a la acción que muestra todas las actividades.
            if (string.IsNullOrEmpty(category) && string.IsNullOrEmpty(fechaInicio) && string.IsNullOrEmpty(fechaFinalizacion))
            {
                return RedirectToAction("Activities");
            }

            // Si solo la categoría está presente, redirige a la acción que filtra por categoría.
            else if (!string.IsNullOrEmpty(category) && string.IsNullOrEmpty(fechaInicio) && string.IsNullOrEmpty(fechaFinalizacion))
            {
                return RedirectToAction("ActivitiesByCategory", new { Category = category });
            }

            // Si solo las fechas están presentes, redirige a la acción que filtra por fechas.
            else if (string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFinalizacion))
            {
                return RedirectToAction("ActivitiesByDateRange", new { FechaInicio = fechaInicio, FechaFinalizacion = fechaFinalizacion });
            }

            // Si la categoría y las fechas están presentes, redirige a la acción que filtra por ambas.
            else if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFinalizacion))
            {
                try
                {
                    // Aquí se llama a la acción o método que filtra por categoría y fechas.
                    List<TblActividadesViewModel> activitiesList = GetActivitiesByCategoryAndDate(category, fechaInicio, fechaFinalizacion); // Obtener actividades filtradas por fecha
                    return View("Activities", activitiesList);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista filtrada por categoría y rango de fechas. Por favor, inténtelo de nuevo más tarde." + ex.Message;
                    return View();
                }
            }

            // Si ninguna de las condiciones anteriores se cumple, puedes retornar a la página de actividades.
            else
            {
                // Si no se cumple ninguna de las condiciones anteriores, mostrar todas las actividades
                return RedirectToAction("Activities");
            }
        }

        // Método para obtener actividades filtradas por categoría y fecha desde la base de datos
        private List<TblActividadesViewModel> GetActivitiesByCategoryAndDate(string category, string fechaInicio, string fechaFinalizacion)
        {
            List<TblActividadesViewModel> activitiesList = new List<TblActividadesViewModel>();

            //Lógica para obtener actividades filtradas por categoría y rangos de fechas desde la base de datos
            try
            {
                using (SqlConnection connection = new SqlConnection(network))
                {
                    //Código para ejecutar la consulta SQL solo con la categoría proporcionada(spConsultarActividadesPorCategoria)
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "spConsultActivityByCategoryAndDate";
                        command.Parameters.AddWithValue("@Categoria", category);
                        command.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = DateTime.Parse(fechaInicio);
                        command.Parameters.Add("@FechaFinalizacion", SqlDbType.DateTime).Value = DateTime.Parse(fechaFinalizacion);
                        SqlDataReader registros = command.ExecuteReader();

                        while (registros.Read())
                        {
                            TblActividadesViewModel allActivities = new TblActividadesViewModel
                            {
                                IdActividad = registros["idActividad"].ToString(),
                                NombreActividad = registros["nombreActividad"].ToString(),
                                Categoria = registros["categoria"].ToString(),
                                HorasAsignadas = registros["horasAsignadas"].ToString(),
                                Descripcion = registros["descripcion"].ToString(),
                                FechaInicio = registros["fechaInicio"].ToString(),
                                FechaFinalizacion = registros["fechaFinalizacion"].ToString(),
                                Responsable = registros["responsable"].ToString(),
                                Cargo = registros["cargo"].ToString()
                            };
                            activitiesList.Add(allActivities);
                        }
                    }
                }
                return activitiesList;
            }
            catch (Exception ex)
            {
                // Manejar excepción
                // Loggear el error, redirigir a una página de error, etc.
                ViewBag.ErrorMessage = "Error con categoría o rangos de fechas no existentes: " + ex.Message;
                return null; // Devuelve null en caso de error
            }
        }
    }
}

