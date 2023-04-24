﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using System.Data.SqlClient;
using evolUX.API.Areas.evolDP.Services;
using Shared.Models.Areas.evolDP;
using evolUX.API.Models;

namespace evolUX.API.Areas.evolDP.Controllers
{
    [Route("api/evoldp/materials/[action]")]
    [ApiController]
    public class MaterialsController : Controller
    {
        private readonly ILoggerService _logger;
        private readonly IMaterialsService _materials;

        public MaterialsController(ILoggerService logger, IMaterialsService materials)
        {
            _logger = logger;
            _materials = materials;
        }
        
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetFulfillMaterialCodes")]
        public async Task<ActionResult<IEnumerable<FulfillMaterialCode>>> GetFulfillMaterialCodes([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("FullFillMaterialCode", out obj);
                string fullFillMaterialCode = Convert.ToString(obj).ToString();

                var result = await _materials.GetFulfillMaterialCodes(fullFillMaterialCode);
                _logger.LogInfo("GetFulfillMaterialCodes Get");
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetFulfillMaterialCodes action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetMaterialTypes")]
        public async Task<ActionResult<IEnumerable<MaterialType>>> GetMaterialTypes([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                bool groupCodes = false;
                string materialTypeCode = "";
                dictionary.TryGetValue("GroupCodes", out obj);
                if (obj != null)
                    groupCodes = bool.Parse(Convert.ToString(obj).ToString());
                dictionary.TryGetValue("MaterialTypeCode", out obj);
                if (obj!= null)
                    materialTypeCode = Convert.ToString(obj).ToString();
                var result = await _materials.GetMaterialTypes(groupCodes, materialTypeCode);
                _logger.LogInfo("GetMaterialTypes Get");
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetMaterialTypes action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetMaterialGroups")]
        public async Task<ActionResult<IEnumerable<MaterialElement>>> GetMaterialGroups([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int groupID = 0;
                dictionary.TryGetValue("GroupID", out obj);
                if (obj != null)
                    groupID = Convert.ToInt32(obj.ToString());

                string groupCode = "";
                dictionary.TryGetValue("GroupCode", out obj);
                if (obj != null)
                    groupCode = Convert.ToString(obj).ToString();

                string materialTypeCode = "";
                dictionary.TryGetValue("MaterialTypeCode", out obj);
                if (obj != null)
                    materialTypeCode = Convert.ToString(obj).ToString();

                int materialTypeID = 0;
                dictionary.TryGetValue("MaterialTypeID", out obj);
                if (obj != null)
                    materialTypeID = Convert.ToInt32(obj.ToString());

                var result = await _materials.GetMaterialGroups(groupID,groupCode,materialTypeID,materialTypeCode);
                _logger.LogInfo("GetMaterialGroups Get");
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetMaterialGroups action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetMaterials")]
        public async Task<ActionResult<IEnumerable<MaterialElement>>> GetMaterials([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int materialID = 0;
                dictionary.TryGetValue("MaterialID", out obj);
                if (obj != null)
                    materialID = Convert.ToInt32(obj.ToString());

                string materialRef = "";
                dictionary.TryGetValue("MaterialRef", out obj);
                if (obj != null)
                    materialRef = Convert.ToString(obj).ToString();

                string materialCode = "";
                dictionary.TryGetValue("MaterialCode", out obj);
                if (obj != null)
                    materialCode = Convert.ToString(obj).ToString();

                int groupID = 0;
                dictionary.TryGetValue("GroupID", out obj);
                if (obj != null)
                    groupID = Convert.ToInt32(obj.ToString());

                string materialTypeCode = "";
                dictionary.TryGetValue("MaterialTypeCode", out obj);
                if (obj != null)
                    materialTypeCode = Convert.ToString(obj).ToString();

                int materialTypeID = 0;
                dictionary.TryGetValue("MaterialTypeID", out obj);
                if (obj != null)
                    materialTypeID = Convert.ToInt32(obj.ToString());

                var result = await _materials.GetMaterials(materialID, materialRef, materialCode, groupID, materialTypeID, materialTypeCode);
                _logger.LogInfo("GetMaterials Get");
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetMaterials action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        [ActionName("GetEnvelopeMedia")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMedia()
        {
            try
            {
                //var envelopeMediaList = await _repository.EnvelopeMedia.GetEnvelopeMedia();
                var envelopeMediaList = await _materials.GetEnvelopeMedia(null);
                _logger.LogInfo("Envelope Media Get");
                return Ok(envelopeMediaList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetEnvelopeMedia action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        [ActionName("GetEnvelopeMediaGroups")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMediaGroups()
        {
            try
            {
                var envelopeMediaGroupList = await _materials.GetEnvelopeMediaGroups(null);
                _logger.LogInfo("Return envelope media group list from database");
                return Ok(envelopeMediaGroupList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetEnvelopeMediaGroups action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
    }
}
