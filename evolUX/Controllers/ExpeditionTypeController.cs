using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using evolUX.Models;

namespace evolUX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpeditionTypeController : ControllerBase
    {
        private IConfiguration _configuration;

        public ExpeditionTypeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<ExpeditionTypeController>
        [HttpGet]
        public ActionResult<List<ExpeditionType>> ExpeditionType()
        {
            List<ExpeditionType> expeditionTypeList = new List<ExpeditionType>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM RD_EXPEDITION_TYPE";
                SqlCommand myCommand = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = myCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ExpeditionType expeditionType = new ExpeditionType();
                        expeditionType.ExpeditionTypeId = Convert.ToInt32(dataReader["ExpeditionType"]);
                        expeditionType.Priority = Convert.ToInt32(dataReader["Priority"]);
                        expeditionType.Description = Convert.ToString(dataReader["Description"]);

                        expeditionTypeList.Add(expeditionType);
                    }
                }
                connection.Close();
            }
            var list = expeditionTypeList;
            return expeditionTypeList;
        }
    }
}
