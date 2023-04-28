using Dapper;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Data.Context;
using evolUX.API.Models;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using System.ComponentModel.Design;
using System.Data;
using System.Reflection.Emit;

namespace evolUX.API.Areas.evolDP.Repositories
{
    public class ServiceProvisionRepository : IServiceProvisionRepository
    {
        private readonly DapperContext _context;
        public ServiceProvisionRepository(DapperContext context)
        {
            _context = context;
        }

        ////FOR REFERENCE https://stackoverflow.com/questions/33087629/dapper-dynamic-parameters-with-table-valued-parameters
        public async Task<IEnumerable<ServiceTaskElement>> GetServiceTasks(int? serviceTaskID)
        {
            string sql = @"RD_UX_GET_SERVICE_TASKS";
            var parameters = new DynamicParameters();
            if (serviceTaskID != null && serviceTaskID > 0)
                parameters.Add("ServiceTaskID", serviceTaskID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceTaskElement> serviceTasks = await connection.QueryAsync<ServiceTaskElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return serviceTasks;
            }
        }
        public async Task<IEnumerable<ServiceTypeElement>> GetServiceTaskServiceTypes(int serviceTaskID)
        {
            string sql = @"RD_UX_GET_SERVICE_TASK_SERVICE_TYPES";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceTaskID", serviceTaskID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceTypeElement> serviceTypes = await connection.QueryAsync<ServiceTypeElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return serviceTypes;
            }
        }

        public async Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int? serviceCompanyID)
        {
            string sql = @"RD_UX_GET_SERVICE_COMPANY_RESTRICTIONS";
            var parameters = new DynamicParameters();
            if (serviceCompanyID != null && serviceCompanyID > 0)
                parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceCompanyRestriction> result = await connection.QueryAsync<ServiceCompanyRestriction>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task SetServiceCompanyRestriction(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode)
        {
            string sql = @"RD_UX_SET_SERVICE_COMPANY_RESTRICTION";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("MaterialTypeID", materialTypeID, DbType.Int64);
            parameters.Add("MaterialPosition", materialPosition, DbType.Int64);
            if (fileSheetsCutoffLevel > 0)
                parameters.Add("FileSheetsCutoffLevel", fileSheetsCutoffLevel, DbType.Int64);
            parameters.Add("RestrictionMode", restrictionMode, DbType.Boolean);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                return;
            }
        }
        public async Task<IEnumerable<ServiceCompanyServiceResume>> GetServiceCompanyConfigsResume(int? serviceCompanyID, int? serviceTypeID, int? serviceID, int? costDate)
        {
            string sql = @"RD_UX_GET_SERVICE_COMPANY_SERVICES_RESUME";
            var parameters = new DynamicParameters();
            if (serviceCompanyID != null && serviceCompanyID > 0)
                parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            if (serviceTypeID != null && serviceTypeID > 0)
                parameters.Add("ServiceTypeID", serviceTypeID, DbType.Int64);
            if (serviceID != null && serviceID > 0)
                parameters.Add("ServiceID", serviceID, DbType.Int64);
            if (costDate != null)
                parameters.Add("CostDate", costDate, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceCompanyServiceResume> result = await connection.QueryAsync<ServiceCompanyServiceResume>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
 
        public async Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID)
        {
            string sql = @"RD_UX_GET_SERVICE_COMPANY_SERVICE_COSTS";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            if (serviceTypeID > 0)
                parameters.Add("ServiceTypeID", serviceTypeID, DbType.Int64);
            if (serviceID > 0)
                parameters.Add("ServiceID", serviceID, DbType.Int64);
            if (costDate > 0)
                parameters.Add("CostDate", costDate, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                var configsList = await connection.QueryAsync<ServiceCompanyService>(sql, parameters, commandType: CommandType.StoredProcedure);
                return configsList;
            }
        }
        public async Task SetServiceCompanyConfig(ServiceCompanyService serviceCompanyConfig)
        {
            string sql = @"RD_UX_SET_SERVICE_COMPANY_SERVICE_COST";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyConfig.ServiceCompanyID, DbType.Int64);
            parameters.Add("ServiceID", serviceCompanyConfig.ServiceID, DbType.Int64);
            parameters.Add("CostDate", serviceCompanyConfig.CostDate, DbType.Int64);
            parameters.Add("ServiceCost", serviceCompanyConfig.ServiceCost, DbType.Double);
            parameters.Add("Formula", serviceCompanyConfig.Formula, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                return;
            }
        }
        public async Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID)
        {
            string sql = @"RD_UX_GET_SERVICES";
            var parameters = new DynamicParameters();
            if (serviceTypeID > 0)
                parameters.Add("ServiceTypeID", serviceTypeID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                var servicesList = await connection.QueryAsync<ServiceElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return servicesList;
            }
        }
        public async Task SetService(ServiceElement service)
        {
            string sql = @"RD_UX_SET_SERVICE";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceTypeID", service.ServiceTypeID, DbType.Int64);
            if (service.ServiceID > 0)
                parameters.Add("ServiceID", service.ServiceID, DbType.Int64);
            parameters.Add("ServiceCode", service.ServiceCode, DbType.Int64);
            parameters.Add("ServiceDesc", service.ServiceDesc, DbType.Double);
            parameters.Add("MatchCode", service.MatchCode, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                return;
            }
        }
        public async Task<IEnumerable<ServiceTypeElement>> GetServiceTypes(int? serviceTypeID)
        {
            string sql = @"RD_UX_GET_SERVICE_TYPES";
            var parameters = new DynamicParameters();
            if (serviceTypeID != null && serviceTypeID > 0)
                parameters.Add("ServiceTypeID", serviceTypeID, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceTypeElement> result = await connection.QueryAsync<ServiceTypeElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc)
        {
            string sql = @"RD_UX_SET_SERVICE_TYPE";
            var parameters = new DynamicParameters();
            if (serviceTypeID > 0)
                parameters.Add("ServiceTypeID", serviceTypeID, DbType.Int64);

            parameters.Add("ServiceTypeCode", serviceTypeCode, DbType.String);
            parameters.Add("ServiceTypeDesc", serviceTypeDesc, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {

                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task SetServiceTask(int serviceTaskID, string serviceTaskCode, string serviceTaskDesc, int refServiceTaskID, int complementServiceTaskID, int? externalExpeditionMode, string stationExceededDesc)
        {
            string sql = @"RD_UX_SET_SERVICE_TASK";
            var parameters = new DynamicParameters();
            if (serviceTaskID > 0)
                parameters.Add("ServiceTaskID", serviceTaskID, DbType.Int64);

            parameters.Add("ServiceTaskCode", serviceTaskCode, DbType.String);
            parameters.Add("ServiceTaskDesc", serviceTaskDesc, DbType.String);

            if (refServiceTaskID > 0)
                parameters.Add("RefServiceTaskID", refServiceTaskID, DbType.Int64);
            else
            {
                parameters.Add("ComplementServiceTaskID", complementServiceTaskID, DbType.Int64);
                if (externalExpeditionMode != null)
                    parameters.Add("ExternalExpeditionMode", externalExpeditionMode, DbType.Int16);
                parameters.Add("StationExceededDesc", stationExceededDesc, DbType.String);
            }
            using (var connection = _context.CreateConnectionEvolDP())
            {

                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
        }
 
        public async Task<IEnumerable<ExpCodeElement>> GetExpCodes(int serviceTaskID, int expCompanyID, string expCode, DataTable expCompanyList)
        {
            string sql = @"RD_UX_GET_EXPCODES";
            var parameters = new DynamicParameters();
            if (serviceTaskID > 0)
                parameters.Add("ServiceTaskID", serviceTaskID, DbType.Int64);
            if (expCompanyID > 0)
                parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
            if (!string.IsNullOrEmpty(expCode))
                parameters.Add("ExpCode", expCode, DbType.String);
            if (expCompanyID == 0 && string.IsNullOrEmpty(expCode))
                parameters.Add("ExpCompanyList", expCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpCodeElement> result = await connection.QueryAsync<ExpCodeElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        
        public async Task DeleteServiceType(int serviceTaskID, int serviceTypeID)
        {
            string sql = @"RD_UX_DELETE_SERVICE_TASK_SERVICE_TYPE";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceTaskID", serviceTaskID, DbType.Int64);
            parameters.Add("ServiceTypeID", serviceTypeID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {

                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task AddServiceType(int serviceTaskID, int serviceTypeID)
        {
            string sql = @"RD_UX_SET_SERVICE_TASK_SERVICE_TYPE";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceTaskID", serviceTaskID, DbType.Int64);
            parameters.Add("ServiceTypeID", serviceTypeID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {

                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<ExpCenterElement>> GetExpCenters(string expCode, DataTable serviceCompanyList)
        {
            string sql = @"RD_UX_GET_EXPCODE_EXPCENTER_SELECTIONS";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCode", expCode, DbType.String);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpCenterElement> result = await connection.QueryAsync<ExpCenterElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<IEnumerable<ServiceCompanyExpCodeElement>> GetServiceCompanyExpCodes(int serviceCompanyID, DataTable expCompanyList)
        {
            string sql = @"RD_UX_GET_SERVICE_COMPANY_EXPCODES";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("ExpCompanyList", expCompanyList.AsTableValuedParameter("IDlist"));
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceCompanyExpCodeElement> result = await connection.QueryAsync<ServiceCompanyExpCodeElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        
        public async Task SetExpCenter(string expCode, string expCenterCode, string description1, string description2, string description3, int serviceCompanyID, string expeditionZone)
        {
            string sql = @"RD_UX_SET_EXPEDITION_EXPCENTER_EXPZONE";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCode", expCode, DbType.String);
            parameters.Add("ExpCenterCode", expCenterCode, DbType.String);
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("ExpeditionZone", expeditionZone, DbType.String);
            parameters.Add("Description1", description1, DbType.String);
            parameters.Add("Description2", description2, DbType.String);
            parameters.Add("Description3", description3, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {

                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<ServiceCompanyExpCodeConfig>> GetServiceCompanyExpCodeConfigs(string expCode, int serviceCompanyID, string expCenterCode)
        {
            string sql = @"RD_UX_GET_SERVICE_COMPANY_EXPCODE_CONFIG";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCode", expCode, DbType.String);
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("ExpCenterCode", expCenterCode, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceCompanyExpCodeConfig> result = await connection.QueryAsync<ServiceCompanyExpCodeConfig>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task SetServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel, string fullFillMaterialCode, int docMaxSheets, string barcode)
        {
            string sql = @"RD_UX_SET_SERVICE_COMPANY_EXPCODE_CONFIG";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCode", expCode, DbType.String);
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("ExpCenterCode", expCenterCode, DbType.String);
            if (expLevel >= 0)
                parameters.Add("ExpLevel", expLevel, DbType.Int64);
            parameters.Add("FullFillMaterialCode", fullFillMaterialCode, DbType.String);
            if (docMaxSheets > 0)
                parameters.Add("DocMaxSheets", docMaxSheets, DbType.Int64);
            if (!string.IsNullOrEmpty(barcode))
                parameters.Add("Barcode", barcode, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.QueryAsync<ServiceCompanyExpCodeConfig>(sql, parameters, commandType: CommandType.StoredProcedure);
                return;
            }
        }
        public async Task DeleteServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel)
        {
            string sql = @"RD_UX_DELETE_SERVICE_COMPANY_EXPCODE_CONFIG";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCode", expCode, DbType.String);
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("ExpCenterCode", expCenterCode, DbType.String);
            parameters.Add("ExpLevel", expLevel, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.QueryAsync<ServiceCompanyExpCodeConfig>(sql, parameters, commandType: CommandType.StoredProcedure);
                return;
            }
        }
    }
}
