using Dapper;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Data.Context;
using evolUX.API.Models;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using System.ComponentModel.Design;
using System.Data;

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
        public async Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID)
        {
            string sql = @"RD_UX_GET_SERVICE_TASKS";
            var parameters = new DynamicParameters();
            if (serviceTaskID != null && serviceTaskID > 0)
                parameters.Add("ServiceTaskID", serviceTaskID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceTask> serviceTasks = await connection.QueryAsync<ServiceTask>(sql, parameters, commandType: CommandType.StoredProcedure);
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
        //public async Task<IEnumerable<ExpCompanyZone>> GetExpCompanyZones(int? expeditionZone, int? expCompanyID, DataTable? expCompanyList)
        //{
        //    string sql = @"RD_UX_GET_EXPCOMPANY_ZONE";
        //    var parameters = new DynamicParameters();
        //    if (expeditionZone != null && expeditionZone > 0)
        //        parameters.Add("ExpeditionZone", expeditionZone, DbType.String);
        //    if (expCompanyID != null)
        //        parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
        //    else
        //    {
        //        parameters.Add("ExpCompanyList", expCompanyList.AsTableValuedParameter("IDlist"));
        //    }
        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        IEnumerable<ExpCompanyZone> expList = await connection.QueryAsync<ExpCompanyZone>(sql, parameters, commandType: CommandType.StoredProcedure);
        //        return expList;
        //    }
        //}

        //public async Task<IEnumerable<ExpCompanyServiceTask>> GetExpCompanyServiceTask(string expCode)
        //{
        //    string sql = @"RD_UX_GET_EXPCOMPANY_SERVICE_TASK";
        //    var parameters = new DynamicParameters();
        //    if (!string.IsNullOrEmpty(expCode))
        //        parameters.Add("ExpCode", expCode, DbType.Int64);
        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        IEnumerable<ExpCompanyServiceTask> expCodes = await connection.QueryAsync<ExpCompanyServiceTask>(sql,
        //            parameters, commandType: CommandType.StoredProcedure);
        //        return expCodes;
        //    }
        //}

        //public async Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID)
        //{
        //    string sql = @"RD_UX_GET_EXPCOMPANY_EXPEDITION_IDS";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        IEnumerable<ExpeditionRegistElement> expList = await connection.QueryAsync<ExpeditionRegistElement>(sql, parameters, commandType: CommandType.StoredProcedure);
        //        return expList;
        //    }
        //}

        //public async Task<int> SetExpeditionRegistID(ExpeditionRegistElement expRegist)
        //{
        //    string sql = @"RD_UX_SET_EXPCOMPANY_EXPEDITION_IDS";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("ExpCompanyID", expRegist.ExpCompanyID, DbType.Int64);
        //    parameters.Add("StartExpeditionID", expRegist.StartExpeditionID, DbType.Int64);
        //    parameters.Add("EndExpeditionID", expRegist.EndExpeditionID, DbType.Int64);
        //    parameters.Add("CompanyRegistCode", expRegist.CompanyRegistCode, DbType.Int64);
        //    parameters.Add("RegistCodePrefix", expRegist.RegistCodePrefix, DbType.String);
        //    parameters.Add("RegistCodeSuffix", expRegist.RegistCodeSuffix, DbType.String);
        //    if (expRegist.LastExpeditionID <= 0)
        //        expRegist.LastExpeditionID = expRegist.StartExpeditionID - 1;
        //    parameters.Add("LastExpeditionID", expRegist.LastExpeditionID, DbType.Int64);
        //    parameters.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        try
        //        {
        //            await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        int StartExpeditionID = parameters.Get<int>("@ReturnID");
        //        return StartExpeditionID;
        //    }

        //}

        //public async Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID)
        //{
        //    string sql = @"RD_UX_GET_EXPCOMPANY_CONTRACTS";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        IEnumerable<ExpContractElement> expList = await connection.QueryAsync<ExpContractElement>(sql, parameters, commandType: CommandType.StoredProcedure);
        //        return expList;
        //    }
        //}

        //public async Task<int> SetExpContract(ExpContractElement expContract)
        //{
        //    string sql = @"RD_UX_SET_EXPCOMPANY_CONTRACTS";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("ExpCompanyID", expContract.ExpCompanyID, DbType.Int64);
        //    if (expContract.ContractID > 0)
        //        parameters.Add("ContractID", expContract.ContractID, DbType.Int64);
        //    parameters.Add("ContractNr", expContract.ContractNr, DbType.Int64);
        //    parameters.Add("ClientNr", expContract.ClientNr, DbType.Int64);
        //    parameters.Add("ClientName", expContract.ClientNr, DbType.String);
        //    parameters.Add("ClientNIF", expContract.ClientNr, DbType.String);
        //    parameters.Add("ClientAddress", expContract.ClientNr, DbType.String);
        //    parameters.Add("ClientPostalCode", expContract.ClientNr, DbType.String);
        //    parameters.Add("ClientPostalCodeDescription", expContract.ClientNr, DbType.String);
        //    if (!string.IsNullOrEmpty(expContract.CompanyExpeditionCode))
        //        parameters.Add("CompanyExpeditionCode", expContract.CompanyExpeditionCode, DbType.String);
        //    if (expContract.PurchaseOrderNr > 0)
        //        parameters.Add("PurchaseOrderNr", expContract.PurchaseOrderNr, DbType.Decimal);

        //    parameters.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
        //        int ContractID = parameters.Get<int>("@ReturnID");
        //        return ContractID;
        //    }
        //}

        //public async Task NewExpCompanyConfig(int expCompanyID, int startDate)
        //{
        //    string sql = @"RD_UX_NEW_EXPCOMPANY_CONFIGS";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
        //    parameters.Add("StartDate", startDate, DbType.Int64);

        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
        //        return;
        //    }
        //}
        //public async Task<IEnumerable<ExpCompanyConfigResume>> GetExpCompanyConfigsResume(int expCompanyID)
        //{
        //    string sql = @"RD_UX_GET_EXPCOMPANY_CONFIGS_RESUME";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        var expeditionCompanyConfigsList = await connection.QueryAsync<ExpCompanyConfigResume>(sql, parameters, commandType: CommandType.StoredProcedure);
        //        return expeditionCompanyConfigsList;
        //    }
        //}
    }
}
