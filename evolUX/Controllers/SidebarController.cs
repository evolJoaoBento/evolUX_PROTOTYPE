using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Data.SqlClient;
using System.Data;

namespace evolUX.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SidebarController : ControllerBase
    {
        private IConfiguration _configuration;

        public SidebarController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<List<ExpandoObject>> GetSidebarButtons()
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            string connectionString = _configuration.GetConnectionString("IdentityConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM ng_sidebar";

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

                        result.Add(row);
                    }
                }
                connection.Close();
            }
            return result;
        }
    }
}