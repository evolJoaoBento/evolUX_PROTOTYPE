using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Data.SqlClient;
using System.Data;

namespace evolUX.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EnvelopeMediaController : Controller
    {
        private IConfiguration _configuration;

        public EnvelopeMediaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [ActionName("get")]
        public async Task<ActionResult<List<ExpandoObject>>> GetEnvelopeMedia()
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"select EnvMediaID as [Id],
	                        EnvMediaName as [Nome da Gama],
	                        [Description] as [Descrição]
                            from RD_ENVELOPE_MEDIA";

                SqlCommand myCommand = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = myCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        dynamic row = new ExpandoObject();
                        row.id = Convert.ToInt32(dataReader["Id"]);
                        row.name = Convert.ToString(dataReader["Nome da Gama"]);
                        row.description = Convert.ToString(dataReader["Descrição"]);

                        result.Add(row);
                    }
                }
                connection.Close();
            }
            return result;
        }
        [HttpGet]
        [ActionName("getOne")]
        public async Task<ActionResult<List<ExpandoObject>>> GetEnvelopeMediaGroups()
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT  
                            CAST(emg.EnvMediaGroupID as varchar) as [Id], 
                            emg.[Description] as [Description],
                            CAST(emg.DefaultEnvMediaID as varchar) as [DefaultEnvMediaId],     
                            em.[Description] as [Omission]
		                    FROM  RD_ENVELOPE_MEDIA_GROUP emg,
			                    RD_ENVELOPE_MEDIA em
		                    WHERE emg.DefaultEnvMediaID = em.EnvMediaID"; ;

                SqlCommand myCommand = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = myCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        dynamic row = new ExpandoObject();
                        row.id = Convert.ToInt32(dataReader["Id"]);
                        row.defaultenvmediaid = Convert.ToString(dataReader["DefaultEnvMediaId"]);
                        row.description = Convert.ToString(dataReader["Description"]);
                        row.omission = Convert.ToString(dataReader["Omission"]);

                        result.Add(row);
                    }
                }
                connection.Close();
            }
            return result;
        }
    }
}
