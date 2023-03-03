﻿using Dapper;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Data.Context;
using evolUX.API.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.Contracts;
using System.Runtime.Intrinsics.Arm;

namespace evolUX.API.Areas.evolDP.Repositories
{
    public class ExpeditionRepository : IExpeditionRepository
    {
        private readonly DapperContext _context;
        public ExpeditionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExpeditionTypeElement>> GetExpeditionTypes(int? expeditionType)
        {
            string sql = @"RD_UX_GET_EXPEDITION_TYPE";
            var parameters = new DynamicParameters();
            if (expeditionType != null && expeditionType > 0)
                parameters.Add("ExpeditionType", expeditionType, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpeditionTypeElement> expList = await connection.QueryAsync<ExpeditionTypeElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return expList;
            }
        }

        public async Task<IEnumerable<ExpCompanyType>> GetExpCompanyTypes(int? expeditionType, int? expCompanyID, DataTable? expCompanyList)
        {
            string sql = @"RD_UX_GET_EXPCOMPANY_TYPE";
            var parameters = new DynamicParameters();
            if (expeditionType != null && expeditionType > 0)
                parameters.Add("ExpeditionType", expeditionType, DbType.String);
            if (expCompanyID != null)
                parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
            else
            {
                parameters.Add("ExpCompanyList", expCompanyList.AsTableValuedParameter("IDlist"));
            }
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpCompanyType> expList = await connection.QueryAsync<ExpCompanyType>(sql, parameters, commandType: CommandType.StoredProcedure);
                return expList;
            }
        }

        public async Task<Result> SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode)
        {
            string sql = @"RD_UX_SET_EXPCOMPANY_TYPE";
            var parameters = new DynamicParameters();
            parameters.Add("ExpeditionType", expeditionType, DbType.String);
            parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
            parameters.Add("RegistMode", registMode, DbType.Boolean);
            parameters.Add("SeparationMode", separationMode, DbType.Boolean);
            parameters.Add("BarcodeRegistMode", barcodeRegistMode, DbType.Boolean);
            
            using (var connection = _context.CreateConnectionEvolDP())
            {
                
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters,
                   commandType: CommandType.StoredProcedure);
                return results.First();
            }
        }

        public async Task<IEnumerable<ExpeditionZoneElement>> GetExpeditionZones(int? expeditionZone)
        {
            string sql = @"RD_UX_GET_EXPEDITION_ZONE";
            var parameters = new DynamicParameters();
            if (expeditionZone != null && expeditionZone > 0)
                parameters.Add("ExpeditionZone", expeditionZone, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpeditionZoneElement> expList = await connection.QueryAsync<ExpeditionZoneElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return expList;
            }
        }

        public async Task<IEnumerable<ExpCompanyZone>> GetExpCompanyZones(int? expeditionZone, int? expCompanyID, DataTable? expCompanyList)
        {
            string sql = @"RD_UX_GET_EXPCOMPANY_ZONE";
            var parameters = new DynamicParameters();
            if (expeditionZone != null && expeditionZone > 0)
                parameters.Add("ExpeditionZone", expeditionZone, DbType.String);
            if (expCompanyID != null)
                parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
            else
            {
                parameters.Add("ExpCompanyList", expCompanyList.AsTableValuedParameter("IDlist"));
            }
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpCompanyZone> expList = await connection.QueryAsync<ExpCompanyZone>(sql, parameters, commandType: CommandType.StoredProcedure);
                return expList;
            }
        }

        public async Task<IEnumerable<ExpCompanyServiceTask>> GetExpCompanyServiceTask(string expCode)
        {
            string sql = @"RD_UX_GET_EXPCOMPANY_SERVICE_TASK";
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(expCode))
                parameters.Add("ExpCode", expCode, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpCompanyServiceTask> expCodes = await connection.QueryAsync<ExpCompanyServiceTask>(sql,
                    parameters, commandType: CommandType.StoredProcedure);
                return expCodes;
            }
        }

        public async Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID)
        {
            string sql = @"RD_UX_GET_EXPCOMPANY_EXPEDITION_IDS";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpeditionRegistElement> expList = await connection.QueryAsync<ExpeditionRegistElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return expList;
            }
        }

        public async Task<int> SetExpeditionRegistID(ExpeditionRegistElement expRegist)
        {
            string sql = @"RD_UX_SET_EXPCOMPANY_EXPEDITION_IDS";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCompanyID", expRegist.ExpCompanyID, DbType.Int64);
            parameters.Add("StartExpeditionID", expRegist.StartExpeditionID, DbType.Int64);
            parameters.Add("EndExpeditionID", expRegist.EndExpeditionID, DbType.Int64);
            parameters.Add("CompanyRegistCode", expRegist.CompanyRegistCode, DbType.Int64);
            parameters.Add("RegistCodePrefix", expRegist.RegistCodePrefix, DbType.String);
            parameters.Add("RegistCodeSuffix", expRegist.RegistCodeSuffix, DbType.String);
            if (expRegist.LastExpeditionID <= 0)
                expRegist.LastExpeditionID = expRegist.StartExpeditionID - 1;
            parameters.Add("LastExpeditionID", expRegist.LastExpeditionID, DbType.Int64);
            parameters.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                try
                {
                    await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                int StartExpeditionID = parameters.Get<int>("@ReturnID");
                return StartExpeditionID;
            }

        }

        public async Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID)
        {
            string sql = @"RD_UX_GET_EXPCOMPANY_CONTRACTS";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpContractElement> expList = await connection.QueryAsync<ExpContractElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return expList;
            }
        }

        public async Task<int> SetExpContract(ExpContractElement expContract)
        {
            string sql = @"RD_UX_SET_EXPCOMPANY_CONTRACTS";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCompanyID", expContract.ExpCompanyID, DbType.Int64);
            if (expContract.ContractID > 0)
                parameters.Add("ContractID", expContract.ContractID, DbType.Int64);
            parameters.Add("ContractNr", expContract.ContractNr, DbType.Int64);
            parameters.Add("ClientNr", expContract.ClientNr, DbType.Int64);
            parameters.Add("ClientName", expContract.ClientNr, DbType.String);
            parameters.Add("ClientNIF", expContract.ClientNr, DbType.String);
            parameters.Add("ClientAddress", expContract.ClientNr, DbType.String);
            parameters.Add("ClientPostalCode", expContract.ClientNr, DbType.String);
            parameters.Add("ClientPostalCodeDescription", expContract.ClientNr, DbType.String);
            if (!string.IsNullOrEmpty(expContract.CompanyExpeditionCode))
                parameters.Add("CompanyExpeditionCode", expContract.CompanyExpeditionCode, DbType.String);
            if (expContract.PurchaseOrderNr > 0)
                parameters.Add("PurchaseOrderNr", expContract.PurchaseOrderNr, DbType.Decimal);

            parameters.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                int ContractID = parameters.Get<int>("@ReturnID");
                return ContractID;
            }
        }

        public async Task<IEnumerable<ExpCompanyConfig>> GetExpCompanyConfigs(int expCompanyID, int startDate, int expeditionType, int expeditionZone)
        {
            string sql = @"RD_UX_GET_EXPCOMPANY_CONFIGS";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
            if (expeditionType > 0)
                parameters.Add("ExpeditionType", expeditionType, DbType.Int64);
            if (expeditionZone > 0)
                parameters.Add("ExpeditionZone", expeditionZone, DbType.Int64);
            if (startDate > 0)
                parameters.Add("StartDate", startDate, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                var expeditionCompanyConfigsList = await connection.QueryAsync<ExpCompanyConfig>(sql, parameters, commandType: CommandType.StoredProcedure);
                return expeditionCompanyConfigsList;
            }
        }
        public async Task SetExpCompanyConfig(ExpCompanyConfig expCompanyConfig)
        {
            string sql = @"RD_UX_SET_EXPCOMPANY_CONFIG";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCompanyID", expCompanyConfig.ExpCompanyID, DbType.Int64);
            parameters.Add("ExpeditionType", expCompanyConfig.ExpeditionType, DbType.Int64);
            parameters.Add("ExpeditionZone", expCompanyConfig.ExpeditionZone, DbType.Int64);
            parameters.Add("ExpCompanyLevel", expCompanyConfig.ExpCompanyLevel, DbType.Int64);
            parameters.Add("StartDate", expCompanyConfig.StartDate, DbType.Int64);
            parameters.Add("UnitCost", expCompanyConfig.UnitCost, DbType.Double);
            if (expCompanyConfig.MaxWeight > 0)
                parameters.Add("MaxWeight", expCompanyConfig.MaxWeight, DbType.String);
            parameters.Add("ExpColumnA", expCompanyConfig.ExpColumnA, DbType.String);
            parameters.Add("ExpColumnB", expCompanyConfig.ExpColumnB, DbType.String);
            parameters.Add("ExpColumnE", expCompanyConfig.ExpColumnE, DbType.String);
            if (!string.IsNullOrEmpty(expCompanyConfig.ExpColumnI))
                parameters.Add("ExpColumnI", expCompanyConfig.ExpColumnI, DbType.String);
            if (!string.IsNullOrEmpty(expCompanyConfig.ExpColumnF))
                parameters.Add("ExpColumnF", expCompanyConfig.ExpColumnF, DbType.String);

            using(var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                return;
            }
        }
        public async Task<IEnumerable<ExpCompanyConfigResume>> GetExpCompanyConfigsResume(int expCompanyID)
        {
            string sql = @"RD_UX_GET_EXPCOMPANY_CONFIGS_RESUME";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCompanyID", expCompanyID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                var expeditionCompanyConfigsList = await connection.QueryAsync<ExpCompanyConfigResume>(sql, parameters, commandType: CommandType.StoredProcedure);
                return expeditionCompanyConfigsList;
            }
        }
    }
}
