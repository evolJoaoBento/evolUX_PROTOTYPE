using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Data.SqlClient;
using System.Data;
using evolUX.Interfaces;

namespace evolUX.Areas.Core.Controllers
{
    [Route("core/sidebar/[action]")]
    [ApiController]
    public class SidebarController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerManager _logger;

        public SidebarController(IWrapperRepository wrapperRepository, ILoggerManager loggerManager)
        {
            _repository = wrapperRepository;
            _logger = loggerManager;
        }

        [HttpGet]
        [ActionName("GetMain")]
        public async Task<ActionResult<List<dynamic>>> GetSidebarButtons()
        {
            try
            {
                var sidebarButtons = await _repository.Sidebar.GetSidebar();

                _logger.LogInfo("Sidebar Get");
                return Ok(sidebarButtons);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetSidebarButtons action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        /*
        [HttpGet]
        [ActionName("GetInner")]
        public async Task<ActionResult<List<ExpandoObject>>> GetInnerButtons(int id
        */
        //public async Task<ActionResult<List<dynamic>>> GetInnerButtons(int id)
        //{
        //    //List<ExpandoObject> result = new List<ExpandoObject>();
        //    //string connectionString = _configuration.GetConnectionString("EvolFlowConnection");
        //    //using (SqlConnection connection = new SqlConnection(connectionString))
        //    //{
        //    //    connection.Open();
        //    //    string sql = "SELECT * FROM ng_innerSidebar WHERE sidebarId = @SIDEBARID AND active = 1";

        //    //    SqlCommand myCommand = new SqlCommand(sql, connection);
        //    //    SqlParameter parameter = new SqlParameter
        //    //    {
        //    //        ParameterName = "@SIDEBARID",
        //    //        Value = id,
        //    //        SqlDbType = SqlDbType.Int,
        //    //        Direction = ParameterDirection.Input
        //    //    };
        //    //    myCommand.Parameters.Add(parameter);

        //    //    using (SqlDataReader dataReader = myCommand.ExecuteReader())
        //    //    {
        //    //        while (dataReader.Read())
        //    //        {
        //    //            dynamic row = new ExpandoObject();
        //    //            row.id = Convert.ToInt32(dataReader["id"]);
        //    //            row.sidebarId = Convert.ToString(dataReader["sidebarId"]);
        //    //            row.text = Convert.ToString(dataReader["text"]);
        //    //            row.translateText = Convert.ToString(dataReader["translateText"]);
        //    //            row.routerLink = Convert.ToString(dataReader["routerLink"]);
        //    //            row.active = Convert.ToString(dataReader["active"]);

        //    //            result.Add(row);
        //    //        }
        //    //    }
        //    //    connection.Close();
        //    //}
        //    //return result;

        //    //try
        //    //{
        //    //    var innerButtons = await _repository.Sidebar.GetInnerButtons();
        //    //    _logger.LogInfo("Inner Buttons Get");
        //    //    return Ok(innerButtons);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    //log error
        //    //    _logger.LogError($"Something went wrong inside GetInnerButtons action: {ex.Message}");
        //    //    return StatusCode(500, "Internal Server Error");
        //    //}
        //}
    }
}