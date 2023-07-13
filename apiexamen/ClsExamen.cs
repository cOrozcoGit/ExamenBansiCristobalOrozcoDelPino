using System;
using System.Net.Http;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Data.SqlClient;

namespace apiexamen
{
    public class Examen
    {
        public int IdExamen { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }
    }

    public class ReturnMessage
    {
        public bool ReturnStatus { get; set; }
        public string? ReturnMsg { get; set; }
    }
    public class ClsExamen
    {
        private readonly HttpClient _httpClient;

        private string _connectionString;

        public ClsExamen(HttpClient httpClient, string connectionString)
        {
            _httpClient = httpClient;
            _connectionString = connectionString;
        }
        public async Task<ReturnMessage?> AgregarExamenAsync(int idExamen, string nombre, string descripcion, string method)
        {
            ReturnMessage returnMessage = new ReturnMessage();
            try
            {
                if (method == null) throw new ArgumentNullException("null method");

                if (method == "WEBAPI")
                {

                    var parameters = new { idExamen, nombre, descripcion };
                    var json = JsonConvert.SerializeObject(parameters);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:7293/Examen/AgregarExamen", content);

                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReturnMessage>(result);
                }
                else if (method == "STOREDPROCEDURES")
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("spAgregar", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Descripcion", descripcion);

                            await connection.OpenAsync();

                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);
                                if (dataTable.Rows.Count > 0)
                                {
                                    returnMessage.ReturnStatus = true;
                                    returnMessage.ReturnMsg = dataTable.Rows[0]["Description"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnMessage.ReturnStatus = false;
                returnMessage.ReturnMsg = ex.Message;
            }
            return returnMessage;

        }
        public async Task<ReturnMessage?> ActualizarExamenAsync(int idExamen, string nombre, string descripcion, string method)
        {
            ReturnMessage returnMessage = new ReturnMessage();
            try
            {
                if (method == null) throw new ArgumentNullException("null method");

                if (method == "WEBAPI")
                {

                    var parameters = new { idExamen, nombre, descripcion };
                    var json = JsonConvert.SerializeObject(parameters);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PutAsync("https://localhost:7293/Examen/ActualizarExamen", content);

                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReturnMessage>(result);
                }
                else if (method == "STOREDPROCEDURES")
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("spActualizar", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@idExamen", idExamen);
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Descripcion", descripcion);

                            await connection.OpenAsync();

                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);
                                if (dataTable.Rows.Count > 0)
                                {
                                    returnMessage.ReturnStatus = true;
                                    returnMessage.ReturnMsg = dataTable.Rows[0]["Description"].ToString();
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                returnMessage.ReturnStatus = false;
                returnMessage.ReturnMsg = ex.Message;
            }
            return returnMessage;
        }

        public async Task<ReturnMessage?> EliminarExamenAsync(int idExamen, string method)
        {
            ReturnMessage returnMessage = new ReturnMessage();
            try
            {
                if (method == null) throw new ArgumentNullException("null method");

                if (method == "WEBAPI")
                {
                    HttpResponseMessage response = await _httpClient.DeleteAsync($"https://localhost:7293/Examen/EliminarExamen?idExamen{idExamen}");

                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReturnMessage>(result);
                }
                else if (method == "STOREDPROCEDURES")
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("spEliminar", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@idExamen", idExamen);

                            await connection.OpenAsync();

                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);
                                if (dataTable.Rows.Count > 0)
                                {
                                    returnMessage.ReturnStatus = true;
                                    returnMessage.ReturnMsg = dataTable.Rows[0]["Description"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnMessage.ReturnStatus = false;
                returnMessage.ReturnMsg = ex.Message;
                
            }
            return returnMessage;
        }

        public async Task<List<Examen>?> ConsultarExamenAsync(string nombre, string descripcion, string method)
        {
            ReturnMessage returnMessage = new ReturnMessage();
            List<Examen> examenes = new List<Examen>();

            if (method == "WEBAPI")
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7293/Examen/ConsultarExamen?nombre={nombre}&descripcion={descripcion}");

                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Examen>>(result);
            }
            else if (method == "STOREDPROCEDURES")
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spConsultar", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Descripcion", descripcion);

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            foreach (DataRow row in dataTable.Rows)
                            {
                                Examen examen = new Examen()
                                {
                                    IdExamen = Convert.ToInt32(row["idExamen"]),
                                    Nombre = row["Nombre"].ToString(),
                                    Descripcion = row["Descripcion"].ToString()
                                };

                                examenes.Add(examen);
                            }

                            return examenes;
                        }
                    }
                }
            }
            return null;
        }
    }
}