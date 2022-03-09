using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Data.SqlClient;
using System.Data;

namespace evolUX.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SidebarController : ControllerBase
    {
        private IConfiguration _configuration;

        public SidebarController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [ActionName("GetMain")]
        public async Task<ActionResult<List<ExpandoObject>>> GetSidebarButtons()
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            string connectionString = _configuration.GetConnectionString("IdentityConnection");
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM ng_sidebar WHERE active = 1";

                SqlCommand myCommand = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = myCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        dynamic row = new ExpandoObject();
                        row.id = Convert.ToInt32(dataReader["id"]);
                        row.title = Convert.ToString(dataReader["title"]);
                        row.translateTitle = Convert.ToString(dataReader["translateTitle"]);
                        row.routerPrefix = Convert.ToString(dataReader["routerPrefix"]);
                        row.theme = Convert.ToString(dataReader["theme"]);
                        row.selectedTheme = Convert.ToString(dataReader["selectedTheme"]);
                        row.linkTheme = Convert.ToString(dataReader["linkTheme"]);
                        row.links = GetInnerButtons(row.id);

                        result.Add(row);
                    }
                }
                connection.Close();
            }
            return result;
        }

        /*
        [HttpGet]
        [ActionName("GetInner")]
        public async Task<ActionResult<List<ExpandoObject>>> GetInnerButtons(int id
        */
        public List<ExpandoObject> GetInnerButtons(int id)
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            string connectionString = _configuration.GetConnectionString("IdentityConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM ng_innerSidebar WHERE sidebarId = @SIDEBARID AND active = 1";

                SqlCommand myCommand = new SqlCommand(sql, connection);
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@SIDEBARID",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                myCommand.Parameters.Add(parameter);

                using (SqlDataReader dataReader = myCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        dynamic row = new ExpandoObject();
                        row.id = Convert.ToInt32(dataReader["id"]);
                        row.sidebarId = Convert.ToString(dataReader["sidebarId"]);
                        row.text = Convert.ToString(dataReader["text"]);
                        row.translateText = Convert.ToString(dataReader["translateText"]);
                        row.routerLink = Convert.ToString(dataReader["routerLink"]);
                        row.active = Convert.ToString(dataReader["active"]);

                        result.Add(row);
                    }
                }
                connection.Close();
            }
            return result;
        }
    }
}