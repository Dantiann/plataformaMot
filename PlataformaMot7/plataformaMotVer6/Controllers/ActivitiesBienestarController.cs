using plataformaMotVer6.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.EnterpriseServices;
using System.Runtime.Remoting.Messaging;
using Microsoft.Ajax.Utilities;


namespace plataformaMotVer6.Controllers
{
    [Permissions("Bienestar")]
    public class ActivitiesBienestarController : Controller
    {
        static readonly string network = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;


        // MÉTODO "Index". Actúa como punto el punto de entrada principal para el controlador.
        // Maneja la solicitud HTTP para la URL correspondiente al controlador y devuelve la vista principal para la gestión de Actividades de Bienestar
        public ActionResult Index()
        {
            // Limpiar el caché de la respuesta para asegurar la actualización de los datos
            Response.AppendHeader("Cache-Control", "no-store");

            return View();
        }

        // ============================================================
        // 1. MÉTODOS PARA OBTENER LA TABLA DE ACTIVIDADES
        // ============================================================


        /* 1.1.----------------- MÉTODOS DE ACCIÓN ----------------- */


        // 1.1.1. MÉTODO "Activities". GET: Muestra todas las actividades.
        public ActionResult Activities()
        {
            // Limpiar el caché de la respuesta para asegurar la actualización de los datos
            Response.AppendHeader("Cache-Control", "no-store");

            // Invocar método para  cargar la lista de categorias
            InitializeCategoria();

            // Invocar método para  cargar la lista de tipo de documento
            InitializeTipoDocumento();

            try
            {
                TblActividadesViewModel model = new TblActividadesViewModel();   // Crear un nuevo modelo de actividad
                List<TblActividadesViewModel> activitiesList = GetActivities(); // Obtener todas las actividades
                return View(activitiesList);
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar un mensaje de error
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista de actividades. Por favor, inténtelo de nuevo más tarde." + ex.Message;
                return View();
            }
        }


        /* 1.2.--------- MÉTODOS PRIVADOS PARA OBTENER DATOS --------- */


        // 1.2.1. MÉTODO "GetActivities".  Método para obtener todas las actividades desde la base de datos.
        private List<TblActividadesViewModel> GetActivities()
        {
            // Inicializar una lista "ctivitiesList" para almacenar objetos de tipo TblActividadesViewModel
            List<TblActividadesViewModel> activitiesList = new List<TblActividadesViewModel>();

            // Lógica para obtener todas las actividades desde la base de datos
            try
            {
                // Conexión a la base de datos para obtener todas las actividades
                using (SqlConnection connection = new SqlConnection(network))
                {
                    connection.Open();

                    // Configurar el comando "SqlCommand" para ejecutar el procedimiento almacenado "spConsultAllActivities"
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "spConsultAllActivities";


                        // Leer los resultados utilizando "SqlDataReader" del comando ejecutado "ExecuteReader()" y utilizar el método
                        // CreateActivityFromReader para crear los objetos TblActividades
                        using (SqlDataReader registers = command.ExecuteReader())
                        {
                            while (registers.Read())
                            {
                                TblActividadesViewModel allActivities = CreateActivityFromReader(registers);
                                activitiesList.Add(allActivities);
                            };
                        }
                    }
                }
                return activitiesList;
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar un mensaje de error
                ViewBag.ErrorMessage = "Error con lista de actividades no existente: " + ex.Message;
                return new List<TblActividadesViewModel>(); // Devuelve new List<TblActividades>() en caso de error
            }
        }


        // ============================================================
        // 2. MÉTODOS PARA FILTRAR ACTIVIDADES POR CATEGORIA Y/O FECHA
        // ============================================================


        /* 2.1.----------------- MÉTODOS DE ACCIÓN ----------------- */


        // 2.1.1. MÉTODO "ActivitiesByCategory". GET: Muestra actividades filtradas por categoria.
        [HttpGet]
        public ActionResult ActivitiesByCategory(FilterActivitiesViewModel model) // (string categoria)
        {

            // Limpiar el caché de la respuesta para asegurar la actualización de los datos
            Response.AppendHeader("Cache-Control", "no-store");

            // Ejecutar el método InitializeCategoria (cargar la lista de categorías).
            InitializeCategoria();

            if (string.IsNullOrEmpty(model.Categoria))
            {
                // Validación: Si no se proporciona una categoría, redirigir a la vista de todas las actividades
                return RedirectToAction("Activities"); ; // Aquí debería devolver la vista correspondiente a ActivitiesBycategoria
            }

            try
            {
                // Obtener actividades filtradas por categoría
                List<TblActividadesViewModel> activitiesList = GetActivitiesByCategory(model.Categoria); // Obtener actividades filtradas por categoría. Argumento(categoria)
                return View("Activities", activitiesList);
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar un mensaje de error
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista filtrada por categoría. Por favor, inténtelo de nuevo más tarde." + ex.Message;
                return View();
            }
        }


        // 2.1.2. MÉTODO "ActivitiesByDateRange". GET: Muestra actividades filtradas por rango de fechas.
        [HttpGet]
        public ActionResult ActivitiesByDateRange(FilterActivitiesViewModel model) // (string fechaInicio, string fechaFinalizacion)
        {
            // Limpiar el caché de la respuesta para asegurar la actualización de los datos
            Response.AppendHeader("Cache-Control", "no-store");

            // Ejecutar el método InitializeCategoria (cargar la lista de categorías).
            InitializeCategoria();

            if (string.IsNullOrEmpty(model.FechaInicio) || string.IsNullOrEmpty(model.FechaFinalizacion))
            {
                // Validación: Si no se proporcionan fechas, redirigir a la vista de todas las actividades
                return RedirectToAction("Activities");
            }

            try
            {
                // Validación: Verificar si las fechas proporcionadas son válidas antes de convertirlas a tipos de datos DateTime
                DateTime fechaInicio, fechaFinalizacion;
                if (!DateTime.TryParse(model.FechaInicio, out fechaInicio) || !DateTime.TryParse(model.FechaFinalizacion, out fechaFinalizacion))
                {
                    ViewBag.ErrorMessage = "Las fechas proporcionadas son inválidas.";
                    return View();
                }

                // Obtener actividades filtradas por rango de fechas
                List<TblActividadesViewModel> activitiesList = GetActivitiesByDateRange(model.FechaInicio, model.FechaFinalizacion); // Obtener actividades filtradas por fecha. Argumentos (fechaInicio, fechaFinalizacion)
                return View("Activities", activitiesList);
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar un mensaje de error
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista filtrada por rango de fechas. Por favor, inténtelo de nuevo más tarde." + ex.Message;
                return View();
            }
        }


        // 2.1.3. MÉTODO "ActivitiesByCategoryAndDate". GET: Muestra actividades filtradas por categoria y fecha.
        [HttpGet]
        public ActionResult ActivitiesByCategoryAndDate(FilterActivitiesViewModel model) //(string categoria, string fechaInicio, string fechaFinalizacion)
        {
            // Limpiar el caché de la respuesta para asegurar la actualización de los datos
            Response.AppendHeader("Cache-Control", "no-store");

            // Ejecutar el método InitializeCategoria (cargar la lista de categorías).
            InitializeCategoria();

            // Redireccionar según los parámetros recibidos

            // Si todos los parámetros son nulos o vacíos, redirige a la acción que muestra todas las actividades.
            if (string.IsNullOrEmpty(model.Categoria) && string.IsNullOrEmpty(model.FechaInicio) && string.IsNullOrEmpty(model.FechaFinalizacion))
            {
                return RedirectToAction("Activities");
            }

            // Si solo la categoría está presente, redirige a la acción que filtra por categoría.
            else if (!string.IsNullOrEmpty(model.Categoria) && string.IsNullOrEmpty(model.FechaInicio) && string.IsNullOrEmpty(model.FechaFinalizacion))
            {
                return RedirectToAction("ActivitiesByCategory", new { categoria = model.Categoria });
            }

            // Si solo las fechas están presentes, redirige a la acción que filtra por fechas.
            else if (string.IsNullOrEmpty(model.Categoria) && !string.IsNullOrEmpty(model.FechaInicio) && !string.IsNullOrEmpty(model.FechaFinalizacion))
            {
                return RedirectToAction("ActivitiesByDateRange", new { fechaInicio = model.FechaInicio, fechaFinalizacion = model.FechaFinalizacion });
            }

            // Si la categoría y las fechas están presentes, redirige a la acción que filtra por ambas.
            else if (!string.IsNullOrEmpty(model.Categoria) && !string.IsNullOrEmpty(model.FechaInicio) && !string.IsNullOrEmpty(model.FechaFinalizacion))
            {
                try
                {
                    // Validación: Verificar si las fechas proporcionadas son válidas antes de convertirlas a tipos de datos DateTime
                    DateTime fechaInicio, fechaFinalizacion;
                    if ((!string.IsNullOrEmpty(model.FechaInicio) && !DateTime.TryParse(model.FechaInicio, out fechaInicio)) ||
                        (!string.IsNullOrEmpty(model.FechaFinalizacion) && !DateTime.TryParse(model.FechaFinalizacion, out fechaFinalizacion)))
                    {
                        ViewBag.ErrorMessage = "Las fechas proporcionadas son inválidas.";
                        return View();
                    }

                    // Aquí se llama a la acción o método que filtra por categoría y fechas. Si la categoría y al menos una de las fechas están presentes, obtener actividades filtradas por ambas
                    List<TblActividadesViewModel> activitiesList = GetActivitiesByCategoryAndDate(model.Categoria, model.FechaInicio, model.FechaFinalizacion); // Obtener actividades filtradas por fecha. Argumentos (categoria, fechaInicio, fechaFinalizacion)
                    return View("Activities", activitiesList);
                }
                catch (Exception ex)
                {
                    // Manejar errores y mostrar un mensaje de error
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


        /* 2.2.----------------- MÉTODOS PRIVADOS PARA OBTENER DATOS ----------------- */


        // 2.2.1. MÉTODO "GetActivitiesByCategory". Método para obtener actividades filtradas por categoría desde la base de datos
        private List<TblActividadesViewModel> GetActivitiesByCategory(string categoria)
        {
            // Inicializar una lista "ctivitiesList" para almacenar objetos de tipo TblActividades
            List<TblActividadesViewModel> activitiesList = new List<TblActividadesViewModel>();

            //Lógica para obtener actividades filtradas por categoría desde la base de datos
            try
            {
                // Conexión a la base de datos para obtener actividades filtradas por categoría
                using (SqlConnection connection = new SqlConnection(network))
                {
                    connection.Open();

                    // Configurar el comando "SqlCommand" para ejecutar el procedimiento almacenado "spConsultActivityByCategory"
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "spConsultActivityByCategory";
                        command.Parameters.AddWithValue("@Categoria", categoria);

                        // Leer los resultados utilizando "SqlDataReader" del comando ejecutado "ExecuteReader()" y utilizar el método
                        // CreateActivityFromReader para crear los objetos TblActividades
                        using (SqlDataReader registers = command.ExecuteReader())
                        {
                            while (registers.Read())
                            {
                                TblActividadesViewModel allActivities = CreateActivityFromReader(registers);
                                activitiesList.Add(allActivities);
                            };
                        }
                    }
                }
                return activitiesList;
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar un mensaje de error
                ViewBag.ErrorMessage = "Error al obtener las actividades: " + ex.Message;
                return new List<TblActividadesViewModel>(); // Devuelve new List<TblActividades>() en caso de error
            }
        }


        // 2.2.2. MÉTODO "GetActivitiesByDateRange". Método para obtener actividades filtradas por rango de fechas desde la base de datos
        private List<TblActividadesViewModel> GetActivitiesByDateRange(string fechaInicio, string fechaFinalizacion)
        {
            // Inicializar una lista "ctivitiesList" para almacenar objetos de tipo TblActividades           
            List<TblActividadesViewModel> activitiesList = new List<TblActividadesViewModel>();

            // Lógica para obtener actividades filtradas por rango de fechas desde la base de datos
            try
            {
                // Conexión a la base de datos para obtener actividades filtradas por categoría
                using (SqlConnection connection = new SqlConnection(network))
                {
                    connection.Open();

                    // Configurar el comando "SqlCommand" para ejecutar el procedimiento almacenado "spConsultActivityByDate"
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "spConsultActivityByDate";
                        command.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = DateTime.Parse(fechaInicio).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        command.Parameters.Add("@FechaFinalizacion", SqlDbType.DateTime).Value = DateTime.Parse(fechaFinalizacion).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);


                        // Leer los resultados utilizando "SqlDataReader" del comando ejecutado "ExecuteReader()" y utilizar el método
                        // CreateActivityFromReader para crear los objetos TblActividades
                        using (SqlDataReader registers = command.ExecuteReader())
                        {
                            while (registers.Read())
                            {
                                TblActividadesViewModel allActivities = CreateActivityFromReader(registers);
                                activitiesList.Add(allActivities);
                            };
                        }
                    }
                }
                return activitiesList;
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar un mensaje de error
                ViewBag.ErrorMessage = "Error al obtener las actividades: " + ex.Message;
                return new List<TblActividadesViewModel>(); // Devuelve new List<TblActividades>() en caso de error
            }
        }


        // 2.2.3. MÉTODO "GetActivitiesByCategoryAndDate". Método para obtener actividades filtradas por categoría y fecha desde la base de datos
        private List<TblActividadesViewModel> GetActivitiesByCategoryAndDate(string categoria, string fechaInicio, string fechaFinalizacion)
        {
            // Inicializar una lista "ctivitiesList" para almacenar objetos de tipo TblActividades
            List<TblActividadesViewModel> activitiesList = new List<TblActividadesViewModel>();

            //Lógica para obtener actividades filtradas por categoría y rangos de fechas desde la base de datos
            try
            {
                // Conexión a la base de datos para obtener actividades filtradas por categoría
                using (SqlConnection connection = new SqlConnection(network))
                {
                    connection.Open();

                    // Configurar el comando "SqlCommand" para ejecutar el procedimiento almacenado "spConsultActivityByCategoryAndDate"
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "spConsultActivityByCategoryAndDate";
                        command.Parameters.AddWithValue("@Categoria", categoria);
                        command.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = DateTime.Parse(fechaInicio).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        command.Parameters.Add("@FechaFinalizacion", SqlDbType.DateTime).Value = DateTime.Parse(fechaFinalizacion).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        // Leer los resultados utilizando "SqlDataReader" del comando ejecutado "ExecuteReader()" y utilizar el método
                        // CreateActivityFromReader para crear los objetos TblActividades
                        using (SqlDataReader registers = command.ExecuteReader())
                        {
                            while (registers.Read())
                            {
                                TblActividadesViewModel allActivities = CreateActivityFromReader(registers);
                                activitiesList.Add(allActivities);
                            };
                        }
                    }
                }
                return activitiesList;
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar un mensaje de error
                ViewBag.ErrorMessage = "Error al obtener las actividades: " + ex.Message;
                return new List<TblActividadesViewModel>(); // Devuelve new List<TblActividades>() en caso de error
            }
        }


        // ============================================================
        // 3. MÉTODOS PARA EL CRUD DE ACTIVIDADES
        // ============================================================


        /* 3.1.----------------- MÉTODOS DE ACCIÓN ------------------ */


        [HttpPost]
        public ActionResult Activities(TblActividadesViewModel model)
        {
            Response.AppendHeader("Cache-Control", "no-store");

            List<TblActividadesViewModel> activitiesList = new List<TblActividadesViewModel>();

            if (model.Boton is null)
            {
                activitiesList = GetActivities();
                return View(activitiesList);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Por favor, corrija los errores en el formulario.";

                InitializeCategoria();
                InitializeTipoDocumento();

                return View("Activities", GetActivities());
            }

            try
            {
                // Convertir fechas al formato correcto antes de enviarlas a la base de datos
                model.FechaInicio = Convert.ToDateTime(model.FechaInicio).ToString("yyyy-MM-dd");
                model.FechaFinalizacion = Convert.ToDateTime(model.FechaFinalizacion).ToString("yyyy-MM-dd");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Por favor, ingrese las fechas en el formato correcto.";
                InitializeCategoria();
                InitializeTipoDocumento();
                return View("Activities", GetActivities());
            }

            if (!CheckUser(model.TipoDocumentoBienestar, model.NumeroDocumentoBienestar))
            {
                // El usuario no existe en tblBienestar, mostrar un mensaje de error
                ViewBag.ErrorMessage = "No existe un usuario con este tipo y número de documento en Bienestar.";
                InitializeCategoria();
                InitializeTipoDocumento();
                return View("Activities", GetActivities());
            }

            if (model.Boton == "Agregar")
            {
                using (SqlConnection conn = new SqlConnection(network))
                {
                    conn.Open();

                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = conn;
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@NombreActividad", model.NombreActividad);
                        com.Parameters.AddWithValue("@Categoria", model.Categoria);
                        com.Parameters.AddWithValue("@Descripcion", model.Descripcion);
                        com.Parameters.AddWithValue("@HorasAsignadas", model.HorasAsignadas);
                        com.Parameters.AddWithValue("@FechaInicio", model.FechaInicio);
                        com.Parameters.AddWithValue("@FechaFinalizacion", model.FechaFinalizacion);
                        com.Parameters.AddWithValue("@TipoDocumentoBienestar", model.TipoDocumentoBienestar);
                        com.Parameters.AddWithValue("@NumeroDocumentoBienestar", model.NumeroDocumentoBienestar);
                        com.CommandText = "spInsertActivity";
                        com.ExecuteNonQuery();

                        activitiesList = GetActivities();

                        InitializeCategoria();
                        InitializeTipoDocumento();

                        ViewBag.SuccessMessage = "La actividad se agregó correctamente.";

                        return RedirectToAction("Activities");

                    }
                }
            }
            else if (model.Boton == "Actualizar")
            {
                using (SqlConnection conn = new SqlConnection(network))
                {
                    conn.Open();

                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = conn;
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.Add("@IdActividad", SqlDbType.NVarChar).Value = model.IdActividad;
                        com.Parameters.Add("@NombreActividad", SqlDbType.NVarChar).Value = model.NombreActividad;
                        com.Parameters.Add("@Categoria", SqlDbType.NVarChar).Value = model.Categoria;
                        com.Parameters.Add("@HorasAsignadas", SqlDbType.NVarChar).Value = model.HorasAsignadas;
                        com.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = model.Descripcion;
                        com.Parameters.Add("@FechaInicio", SqlDbType.NVarChar).Value = model.FechaInicio;
                        com.Parameters.Add("@FechaFinalizacion", SqlDbType.NVarChar).Value = model.FechaFinalizacion;
                        com.Parameters.Add("@TipoDocumentoBienestar", SqlDbType.NVarChar).Value = model.TipoDocumentoBienestar;
                        com.Parameters.Add("@NumeroDocumentoBienestar", SqlDbType.NVarChar).Value = model.NumeroDocumentoBienestar;
                        com.CommandText = "spUpdateActivity";
                        com.ExecuteNonQuery();

                        activitiesList = GetActivities();

                        InitializeCategoria();
                        InitializeTipoDocumento();

                        ViewBag.SuccessMessage = "La actividad se actualizó correctamente.";

                        return View("Activities", activitiesList);
                    }
                }
            }


            return View("Activities", activitiesList);
        }


        [HttpPost]
        public ActionResult DeleteActivity(DeleteActivitiesViewModel model)
        {
            Response.AppendHeader("Cache-Control", "no-store");

            if (!string.IsNullOrEmpty(model.IdActividad) && ModelState.IsValidField("IdActividad"))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(network))
                    {
                        conn.Open();
                        using (SqlCommand com = new SqlCommand())
                        {
                            com.Connection = conn;
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.Add("@IdActividad", SqlDbType.Int).Value = Convert.ToInt32(model.IdActividad);

                            com.CommandText = "spDeleteActivity";
                            com.ExecuteNonQuery();

                            return Json(new { success = true, message = "La actividad se eliminó correctamente." });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            // Retorno en caso de que no se cumplan las condiciones anteriores
            return Json(new { success = false, message = "El ID de la actividad no es válido." });
        }


        [HttpGet]
        public ActionResult GetActivityData(string idActividad)

        {
            TblActividadesViewModel activity = GetActivityById(idActividad);
            return Json(activity, JsonRequestBehavior.AllowGet);
        }


       
        /* 3.2.----------------- MÉTODOS PRIVADOS PARA OBTENER DATOS ----------------- */


        [HttpGet]
        private bool CheckUser(string tipoDocumentoBienestar, string numeroDocumentoBienestar)
        {
            bool userExists = false;

            using (SqlConnection conn = new SqlConnection(network))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM tblBienestar WHERE tipoDocumentoBienestar = @TipoDocumentoBienestar AND numeroDocumentoBienestar = @NumeroDocumentoBienestar";

                using (SqlCommand com = new SqlCommand(query, conn))
                {
                    com.Connection = conn;
                    //com.CommandType = CommandType.StoredProcedure;

                    com.Parameters.Add("@TipoDocumentoBienestar", SqlDbType.NVarChar).Value = tipoDocumentoBienestar;
                    com.Parameters.Add("@NumeroDocumentoBienestar", SqlDbType.NVarChar).Value = numeroDocumentoBienestar;
                    //com.CommandText = "spVerificarDocumentoBienestar";

                    int count = (int)com.ExecuteScalar();

                    userExists = count > 0;
                }
            }

            return userExists;
        }


        private TblActividadesViewModel GetActivityById(string idActividad)
        {
            TblActividadesViewModel activity = null;

            using (SqlConnection conn = new SqlConnection(network))
            {
                conn.Open();

                string query = "SELECT * FROM TblActividades WHERE IdActividad = @IdActividad";

                using (SqlCommand com = new SqlCommand(query, conn))
                {
                    //com.Connection = conn;
                    //com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@IdActividad", SqlDbType.Int).Value = Convert.ToInt32(idActividad);
                    //com.CommandText = "spGetActivityById";

                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            activity = new TblActividadesViewModel
                            {
                                IdActividad = reader["IdActividad"].ToString(),
                                NombreActividad = reader["NombreActividad"].ToString(),
                                Categoria = reader["Categoria"].ToString(),
                                HorasAsignadas = reader["HorasAsignadas"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                FechaInicio = reader["FechaInicio"].ToString(),
                                FechaFinalizacion = reader["FechaFinalizacion"].ToString(),
                                TipoDocumentoBienestar = reader["TipoDocumentoBienestar"].ToString(),
                                NumeroDocumentoBienestar = reader["NumeroDocumentoBienestar"].ToString()
                            };
                        }
                    }
                }
            }

            return activity;
        }


        // ============================================================
        // 4. MÉTODOS AUXILIARES O DE UTILIDAD
        // ============================================================


        // 4.1. MÉTODO "CreateActivityFromReader". Método privado para crear un objeto de actividad a partir del lector de datos. Es utilizado por otros métodos para evitar duplicación de código
        private TblActividadesViewModel CreateActivityFromReader(SqlDataReader registers)
        {
            return new TblActividadesViewModel
            {
                IdActividad = registers["idActividad"].ToString(),
                NombreActividad = registers["nombreActividad"].ToString(),
                Categoria = registers["categoria"].ToString(),
                HorasAsignadas = registers["horasAsignadas"].ToString(),
                Descripcion = registers["descripcion"].ToString(),
                FechaInicio = registers["fechaInicio"].ToString(),
                FechaFinalizacion = registers["fechaFinalizacion"].ToString(),
                Responsable = registers["responsable"].ToString(),
                Cargo = registers["cargo"].ToString()
            };
        }


        // ============================================================
        // 5. MÉTODOS PRIVADOS PARA INALIZACIÓN DE DATOS
        // ============================================================

        // 5.1. MÉTODO InitializeTipoDocumento. Método para inicializar la lista de tipo de documento y asignarla al ViewBag
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


        // 5.2. MÉTODO InitializeTipoDocumento. Método para inicializar la lista de categorías y asignarla al ViewBag
        private void InitializeCategoria()
        {
            ViewBag.Categorias = new SelectList(new List<string> { "Bienestar", "Cultura", "Deportes", "Salud", "Psicosocial" });
        }



    }
}