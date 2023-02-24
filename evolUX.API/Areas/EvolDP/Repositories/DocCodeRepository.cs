using Dapper;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Data.Context;
using System.Data;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using Shared.Models.Areas.Finishing;
using evolUX.API.Models;
using System.Data.SqlClient;
using Shared.Models.General;
using Shared.Models.Areas.Core;

namespace evolUX.API.Areas.evolDP.Repositories
{
    public class DocCodeRepository : IDocCodeRepository
    {
        private readonly DapperContext _context;

        public DocCodeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocCode>> GetDocCodeGroup()
        {
            string sql = @"RD_UX_GET_DOCCODE_GROUP";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<DocCode> docCodeList = await connection.QueryAsync<DocCode>(sql, commandType: CommandType.StoredProcedure);
                return docCodeList;
            }
        }

        public async Task<IEnumerable<DocCode>> GetDocCode(int docCodeID)
        {
            string sql = @"RD_UX_GET_DOCCODE";

            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", docCodeID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<DocCode> docCodeList = await connection.QueryAsync<DocCode, ExceptionLevel, ExceptionLevel, ExceptionLevel, DocCode>(sql,
                                        (d, e1, e2, e3) =>
                                        {
                                            DocCode docCode = d;
                                            docCode.ExceptionLevel1 = e1;
                                            docCode.ExceptionLevel2 = e2;
                                            docCode.ExceptionLevel3 = e3;
                                            return docCode;
                                        }, parameters, commandType: CommandType.StoredProcedure, splitOn: "ExceptionLevelID");
                return docCodeList;
            }
        }

        public async Task<IEnumerable<DocCode>> GetDocCode(string docLayout, string docType, int numRows)
        {
            string sql = @"RD_UX_GET_DOCCODE";

            var parameters = new DynamicParameters();
            parameters.Add("DocLayout", docLayout, DbType.String);
            parameters.Add("DocType", docType, DbType.String);
            if (numRows <= 0) numRows = 2147483647;
            parameters.Add("NumRows", numRows, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<DocCode> docCodeList = await connection.QueryAsync<DocCode, ExceptionLevel, ExceptionLevel, ExceptionLevel, DocCode>(sql,
                                        (d, e1, e2, e3) =>
                                        {
                                            DocCode docCode = d;
                                            docCode.ExceptionLevel1 = e1;
                                            docCode.ExceptionLevel2 = e2;
                                            docCode.ExceptionLevel3 = e3;
                                            return docCode;
                                        }, parameters, commandType: CommandType.StoredProcedure, splitOn: "ExceptionLevelID");
                return docCodeList;
            }
        }

        public async Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(int docCodeID, int? startDate, bool? maxDateFlag)
        {
            string sql = @"RD_UX_GET_DOCCODE_CONFIG";

            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", docCodeID, DbType.Int64);
            if (startDate != null)
                parameters.Add("StartDate", startDate, DbType.Int64);

            if (maxDateFlag != null)
                parameters.Add("MaxDateFlag", maxDateFlag, DbType.Boolean);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<DocCodeConfig> docCodeConfigList = await connection.QueryAsync<DocCodeConfig>(sql, parameters,
                    commandType: CommandType.StoredProcedure);

                return docCodeConfigList;
            }
        }

        public async Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(int docCodeID, DateTime? startDate, bool? maxDateFlag)
        {
            int? startDateInt = null;
            if (startDate != null)
                startDateInt = Int32.Parse(((DateTime)startDate).ToString("yyyyMMdd"));
            return await GetDocCodeConfig(docCodeID, startDateInt, maxDateFlag);
        }

        public async Task<IEnumerable<ExceptionLevel>> GetExceptionLevel(int level)
        {
            string sql = string.Format(@"SELECT	ExceptionLevelID,
									ExceptionCode,
									ExceptionDescription
							FROM	RDC_EXCEPTION_LEVEL{0}
							ORDER BY ExceptionCode", level);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExceptionLevel> docCodeException = await connection.QueryAsync<ExceptionLevel>(sql);
                return docCodeException;
            }
        }

        public async Task<IEnumerable<ExceptionLevel>> SetExceptionLevel(int level, int exceptionID, string exceptionCode, string exceptionDescription)
        {
            string sql = "";
            var parameters = new DynamicParameters();
            parameters.Add("ExceptionCode", exceptionCode, DbType.String);
            parameters.Add("ExceptionDescription", exceptionDescription, DbType.String);

            if (exceptionID == 0)
            {
                sql += string.Format(@"SET NOCOUNT ON
                            INSERT INTO RDC_EXCEPTION_LEVEL{0}(ExceptionLevelID, ExceptionCode, ExceptionDescription)
                            SELECT (SELECT ISNULL(MAX(ExceptionLevelID),0) + 1 FROM RDC_EXCEPTION_LEVEL{0}), @ExceptionCode, @ExceptionDescription
                            WHERE NOT EXISTS (SELECT TOP 1 1 FROM RDC_EXCEPTION_LEVEL{0} WITH(NOLOCK) WHERE ExceptionCode = @ExceptionCode)", level);
            }
            else
            { 
                parameters.Add("ExceptionLevelID", exceptionID, DbType.Int64);
                sql += string.Format(@"SET NOCOUNT ON
                            UPDATE RDC_EXCEPTION_LEVEL{0}
                            SET ExceptionCode = @ExceptionCode, ExceptionDescription = @ExceptionDescription
                            WHERE ExceptionLevelID = @ExceptionLevelID", level);
            }
            sql += string.Format(@"
                            SELECT	ExceptionLevelID,
									ExceptionCode,
									ExceptionDescription
							FROM	RDC_EXCEPTION_LEVEL{0}
							ORDER BY ExceptionCode", level);
            
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExceptionLevel> docCodeException = await connection.QueryAsync<ExceptionLevel>(sql, parameters);
                return docCodeException;
            }
        }

        public async Task<IEnumerable<ExceptionLevel>> DeleteExceptionLevel(int level, int exceptionID)
        {
            string sql = "";
            var parameters = new DynamicParameters();
            parameters.Add("ExceptionLevelID", exceptionID, DbType.Int64);

            sql += string.Format(@"SET NOCOUNT ON
                            DELETE RDC_EXCEPTION_LEVEL{0}
                            WHERE ExceptionLevelID = @ExceptionLevelID", level);

            sql += string.Format(@"
                            SELECT	ExceptionLevelID,
									ExceptionCode,
									ExceptionDescription
							FROM	RDC_EXCEPTION_LEVEL{0}
							ORDER BY ExceptionCode", level);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExceptionLevel> docCodeException = await connection.QueryAsync<ExceptionLevel>(sql, parameters);
                return docCodeException;
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
                    parameters);
                return expCodes;
            }
        }
        
        public async Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups(int? envMediaGroupID)
        {
            string sql = @"RD_UX_GET_ENVELOPE_MEDIA_GROUP";
            var parameters = new DynamicParameters();
            if (envMediaGroupID != null && envMediaGroupID > 0) 
                parameters.Add("EnvMediaGroupID", envMediaGroupID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<EnvelopeMedia> envMedia = await connection.QueryAsync<EnvelopeMedia>(sql,
                    parameters);
                return envMedia;
            }
        }

        public async Task<IEnumerable<int>> GetAggregationList()
        {
            List<int> ilist = new List<int> { 0, 1, 2, 3 };
            return ilist;
        }

        public async Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes(int? expeditionType)
        {
            string sql = @"RD_UX_GET_EXPEDITION_TYPE";
            var parameters = new DynamicParameters();
            if (expeditionType != null && expeditionType > 0)
                parameters.Add("ExpeditionType", expeditionType, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpeditionsType> expeditionCompanies = await connection.QueryAsync<ExpeditionsType>(sql, parameters);
                return expeditionCompanies;
            }
        }

        public async Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID)
        {
            string sql = @"RD_UX_GET_SERVICE_TASK";
            var parameters = new DynamicParameters();
            if (serviceTaskID != null && serviceTaskID > 0)
                parameters.Add("ServiceTaskID", serviceTaskID, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceTask> serviceTasks = await connection.QueryAsync<ServiceTask>(sql, parameters);
                return serviceTasks;
            }
        }
        
        public async Task<GenericOptionList> GetSuporTypeOptionList()
        {
            using (var connection = _context.CreateConnectionEvolDP())
            {
                GenericOptionList result = new GenericOptionList();
                string sql = @"RDC_UX_GET_SUPORT_TYPE_CONFIG";
                var parameters = new DynamicParameters();
                result.List = await connection.QueryAsync<GenericOptionValue>(sql,
                    parameters, commandType: CommandType.StoredProcedure);

                sql = @"SELECT CAST(SuportType as int) [ID]
                        FROM [dbo].[RDC_SUPORT_TYPE]";
                result.ValidList = await connection.QueryAsync<int>(sql);
                return result;
            }
        }

        public async Task<IEnumerable<GenericOptionValue>> GetOptionList(string option)
        {
            string sql = @"RDC_UX_GET_SUPORT_TYPE_CONFIG";
            var parameters = new DynamicParameters();
            parameters.Add("Option", option, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<GenericOptionValue> options = await connection.QueryAsync<GenericOptionValue>(sql, 
                    parameters, commandType: CommandType.StoredProcedure);
                return options;
            }
        }

        public async Task<IEnumerable<DocCode>> SetDocCodeConfig(DocCode docCode)
        {
            string sql = @"RD_NEW_DOCCODE_CONFIG";
            var parameters = new DynamicParameters();
            if (docCode.DocCodeID > 0)
            {
                parameters.Add("DocCodeID", docCode.DocCodeID, DbType.Int64);
            }
            else
            {
                parameters.Add("DocLayout", docCode.DocLayout, DbType.String);
                parameters.Add("DocType", docCode.DocType, DbType.String);
                if (docCode.ExceptionLevel1 != null && docCode.ExceptionLevel1.ExceptionLevelID > 0)
                    parameters.Add("ExceptionLevel1ID", docCode.ExceptionLevel1.ExceptionLevelID, DbType.Int64);

                if (docCode.ExceptionLevel2 != null && docCode.ExceptionLevel2.ExceptionLevelID > 0)
                    parameters.Add("ExceptionLevel2ID", docCode.ExceptionLevel2.ExceptionLevelID, DbType.Int64);

                if (docCode.ExceptionLevel3 != null && docCode.ExceptionLevel3.ExceptionLevelID > 0)
                    parameters.Add("ExceptionLevel3ID", docCode.ExceptionLevel3.ExceptionLevelID, DbType.Int64);
            }

            parameters.Add("Description", docCode.DocDescription, DbType.String);
            parameters.Add("PrintMatchCode", docCode.PrintMatchCode, DbType.String);

            parameters.Add("StartDate", docCode.DocCodeConfigs[0].StartDate, DbType.String);
            parameters.Add("AggrCompatibility", docCode.DocCodeConfigs[0].AggrCompatibility, DbType.Int32);
            parameters.Add("EnvMediaID", docCode.DocCodeConfigs[0].EnvMediaID, DbType.Int64);
            parameters.Add("ExpeditionType", docCode.DocCodeConfigs[0].ExpeditionType, DbType.Int64);
            parameters.Add("ExpCode", docCode.DocCodeConfigs[0].ExpCode, DbType.String);
            parameters.Add("SuportType", docCode.DocCodeConfigs[0].SuportType, DbType.Int32);
            parameters.Add("Priority", docCode.DocCodeConfigs[0].Priority, DbType.Int64);

            if (docCode.DocCodeConfigs[0].ProdMaxSheets > 0)
                parameters.Add("ProdMaxSheets", docCode.DocCodeConfigs[0].ProdMaxSheets, DbType.Int64);

            parameters.Add("MaxProdDate", docCode.DocCodeConfigs[0].MaxProdDate, DbType.String);
            parameters.Add("ArchCaducityDate", docCode.DocCodeConfigs[0].ArchCaducityDate, DbType.String);
            parameters.Add("CaducityDate", docCode.DocCodeConfigs[0].CaducityDate, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<int> docCodeIDResult = await connection.QueryAsync<int>(sql, parameters, commandType: CommandType.StoredProcedure);
                int docCodeID = docCode.DocCodeID;
                if (docCodeIDResult != null && docCodeIDResult.Count() > 0)
                    docCodeID = docCodeIDResult.First();

                IEnumerable<DocCode> docCodeList = await GetDocCode(docCodeID);
                if (docCodeList != null && docCodeList.Count() > 0)
                {
                    docCodeList.First().DocCodeConfigs = (await GetDocCodeConfig(docCodeID, docCode.DocCodeConfigs[0].StartDate, null)).ToList();
                }
                return docCodeList;
            }
        }

        public async Task<IEnumerable<DocCode>> ChangeDocCode(DocCode docCode)
        {
            string sql = @"RD_UPDATE_DOCCODE";
            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", docCode.DocCodeID, DbType.Int64);

            parameters.Add("Description", docCode.DocDescription, DbType.String);
            parameters.Add("PrintMatchCode", docCode.PrintMatchCode, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.QueryAsync<int>(sql, parameters, commandType: CommandType.StoredProcedure);
                IEnumerable<DocCode> docCodeList = await GetDocCode(docCode.DocCodeID);
                if (docCodeList != null && docCodeList.Count() > 0)
                {
                    docCodeList.First().DocCodeConfigs = (await GetDocCodeConfig(docCode.DocCodeID, (int?)null, null)).ToList();
                }
                return docCodeList;
            }
        }

        public async Task<Result> DeleteDocCode(int docCodeID)
        {
            string sql = @"RD_DELETE_DOCCODE";
            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", docCodeID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters,
                   commandType: CommandType.StoredProcedure);
                return results.First();
            }
        }

        public async Task<Result> DeleteDocCodeConfig(int docCodeID, int startDate)
        {
            string sql = @"RD_DELETE_DOCCODE_CONFIG";
            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", docCodeID, DbType.Int64);
            parameters.Add("StartDate", startDate, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters,
                   commandType: CommandType.StoredProcedure);
                return results.First();
            }
        }

        public async Task<IEnumerable<AggregateDocCode>> GetCompatibility(int docCodeID)
        {
            string sql = "RD_UX_GET_DOCCODE_AGGREGATION";
            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", docCodeID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                try
                {
                    IEnumerable<AggregateDocCode> docCodeList = await connection.QueryAsync<AggregateDocCode, ExceptionLevel, ExceptionLevel, ExceptionLevel, AggregateDocCode>(sql,
                                            (d, e1, e2, e3) =>
                                            {
                                                AggregateDocCode docCode = d;
                                                docCode.ExceptionLevel1 = e1;
                                                docCode.ExceptionLevel2 = e2;
                                                docCode.ExceptionLevel3 = e3;
                                                return docCode;
                                            }, parameters, commandType: CommandType.StoredProcedure, splitOn: "ExceptionLevelID");
                    return docCodeList;
                }
                catch (Exception ex) { string lop = ex.Message; return null; }
            }
        }

        public async Task<IEnumerable<AggregateDocCode>> ChangeCompatibility(int docCodeID, DataTable docCodeList)
        {
            string sql = "RD_UX_SET_DOCCODE_AGGREGATION";
            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", docCodeID, DbType.Int64);
            parameters.Add("DocCodeList", docCodeList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                return await GetCompatibility(docCodeID);
            }
        }
        
        public async Task<DocCodeData4ScriptViewModel> DocCodeData4Script(int docCodeID, int startDate)
        {
            string sql = @"RD_UX_GET_DOCCODE_DATA_FOR_SCRIPT";
            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", docCodeID, DbType.Int64);
            parameters.Add("StartDate", startDate, DbType.Int64);

            DocCodeData4ScriptViewModel result = new DocCodeData4ScriptViewModel();
            using (var connection = _context.CreateConnectionEvolDP())
            {
                var reader = connection.QueryMultiple(sql, parameters,
                   commandType: CommandType.StoredProcedure);
                if (reader != null) {
                    result.ExceptionLevelList = reader.Read<ExceptionLevelScript>().ToList();
                    List< DocCodeConfigScript> config = reader.Read<DocCodeConfigScript>().ToList();
                    if (config != null && config.Count > 0)
                    {
                        result.Doc = config.First();
                    }
                    result.AggDocCodeList = reader.Read<DocCodeScript>().ToList();

                    reader.Dispose();
                }
                return result;
            }
        }
    }
}
