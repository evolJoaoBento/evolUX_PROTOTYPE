﻿using Dapper;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Data.Context;
using System.Data;
using evolUX.API.Areas.EvolDP.Repositories.Interfaces;
using Shared.Models.Areas.Finishing;
using evolUX.API.Models;
using System.Data.SqlClient;
using Shared.Models.General;

namespace evolUX.API.Areas.EvolDP.Repositories
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

        public async Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel(int level)
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

        //HANDLE TEXT RESPONSES ON HIGHER LEVELS aggrCompatibility
        public async Task<IEnumerable<AggregateDocCode>> GetAggregateDocCodes(int docCodeID)
        {
            string sql = $@"SELECT	d.DocLayout, 
									d.DocType, 
									e1.ExceptionLevelID,
									e1.ExceptionCode,
									e1.ExceptionDescription,
									e2.ExceptionLevelID,
									e2.ExceptionCode,
									e2.ExceptionDescription,
									e3.ExceptionLevelID,
									e3.ExceptionCode,
									d.[Description] as DocDescription,
									ISNULL(CAST(d.DocCodeID as varchar),'') [Campatible],
									ISNULL(CASE WHEN dac.AggDocCodeID is null then 0 else 1 end, '') [CheckStatus]
							FROM
								RD_DOCCODE d
							LEFT OUTER JOIN

								RD_DOCCODE_AGGREGATION_COMPATIBILITY dac
							ON d.DocCodeID = dac.AggDocCodeID

								AND RefDocCodeID = @DOCCODEID
							LEFT OUTER JOIN
								RD_DOCCODE dc1
							ON dc1.DocCodeID = @DOCCODEID

								AND dc1.DocCodeID<> d.DocCodeID
							 LEFT OUTER JOIN

								RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
							ON e1.ExceptionLevelID = d.ExceptionLevel1ID
							LEFT OUTER JOIN
								RDC_EXCEPTION_LEVEL2 e2 WITH(NOLOCK)
							ON e2.ExceptionLevelID = d.ExceptionLevel2ID
							LEFT OUTER JOIN
								RDC_EXCEPTION_LEVEL3 e3 WITH(NOLOCK)
							ON e3.ExceptionLevelID = d.ExceptionLevel3ID
							ORDER BY dc1.DocCodeID ASC, d.DocLayout,d.DocType,d.ExceptionLevel1ID,d.ExceptionLevel2ID,d.ExceptionLevel3ID";
            var parameters = new DynamicParameters();
            parameters.Add("DOCCODEID", docCodeID, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<AggregateDocCode> docCodeList = await connection.QueryAsync<AggregateDocCode, ExceptionLevel, ExceptionLevel, ExceptionLevel, AggregateDocCode>(sql,
                                        (d, e1, e2, e3) =>
                                        {
                                            AggregateDocCode docCode = d;
                                            docCode.DocExceptionLevel1 = e1;
                                            docCode.DocExceptionLevel2 = e2;
                                            docCode.DocExceptionLevel3 = e3;
                                            return docCode;
                                        }, parameters, splitOn: "ExceptionLevelID");
                return docCodeList;
            }
        }

        //HANDLE TEXT RESPONSES ON HIGHER LEVELS aggrCompatibility
        public async Task<AggregateDocCode> GetAggregateDocCode(int docCodeID)
        {
            string sql = $@"SELECT  d.DocCodeID,
									d.DocLayout
									d.DocType,
									e1.ExceptionLevelID,
									e1.ExceptionCode,
									e1.ExceptionDescription,
									e2.ExceptionLevelID,
									e2.ExceptionCode,
									e2.ExceptionDescription,
									e3.ExceptionLevelID,
									e3.ExceptionCode,
									e3.ExceptionDescription,
									d.[Description] as DocDescription
									dc.AggrCompatibility
							FROM [DMS_evolDP].[dbo].[RD_DOCCODE] d
							INNER JOIN
								[DMS_evolDP].[dbo].[RD_DOCCODE_CONFIG] dc
							ON d.DocCodeID = dc.DocCodeID
							LEFT OUTER JOIN
								[DMS_evolDP].[dbo].[RDC_EXCEPTION_LEVEL1] e1 WITH(NOLOCK)
							ON	e1.ExceptionLevelID = d.ExceptionLevel1ID
							LEFT OUTER JOIN
								[DMS_evolDP].[dbo].[RDC_EXCEPTION_LEVEL2] e2 WITH(NOLOCK)
							ON	e2.ExceptionLevelID = d.ExceptionLevel2ID
							LEFT OUTER JOIN
								[DMS_evolDP].[dbo].[RDC_EXCEPTION_LEVEL3] e3 WITH(NOLOCK)
							ON	e3.ExceptionLevelID = d.ExceptionLevel3ID
							WHERE d.DocCodeID = @DOCCODEID
								AND dc.StartDate = (SELECT MAX(StartDate)
											FROM [DMS_evolDP].[dbo].[RD_DOCCODE_CONFIG]
											WHERE DocCodeID = dc.DocCodeID
												AND StartDate <= CONVERT(varchar,CURRENT_TIMESTAMP,112))";
            var parameters = new DynamicParameters();
            parameters.Add("DOCCODEID", docCodeID, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<AggregateDocCode> docCodeList = await connection.QueryAsync<AggregateDocCode, ExceptionLevel, ExceptionLevel, ExceptionLevel, AggregateDocCode>(sql,
                                        (d, e1, e2, e3) =>
                                        {
                                            AggregateDocCode docCode = d;
                                            docCode.DocExceptionLevel1 = e1;
                                            docCode.DocExceptionLevel2 = e2;
                                            docCode.DocExceptionLevel3 = e3;
                                            return docCode;
                                        }, parameters, splitOn: "ExceptionLevelID");
                return docCodeList.First();
            }
        }

        public async Task ChangeCompatibility(DocCodeCompatabilityViewModel model)
        {
            string itemsChecked = "";
            string finalString = "";
            foreach (AggregateDocCode selection in model.DocCodeList)
            {
                if (model.DocCodeList.Last() == selection)
                {
                    itemsChecked += selection;
                }
                else
                {
                    itemsChecked += selection.DocCodeID + ", ";
                }
            }
            if (string.IsNullOrEmpty(itemsChecked))
            {
                finalString = "is NULL";
            }
            else
            {
                finalString = "in (" + itemsChecked + ")";
            }
            string sql = $@"EXEC RD_SET_DOCCODE_AGGREGATION @ID, @FINALSTRING";
            var parameters = new DynamicParameters();
            parameters.Add("ID", model.DocCode.DocCodeID, DbType.String);
            parameters.Add("FINALSTRING", finalString, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<AggregateDocCode> docCodeList = await connection.QueryAsync<AggregateDocCode, ExceptionLevel, ExceptionLevel, ExceptionLevel, AggregateDocCode>(sql,
                                        (d, e1, e2, e3) =>
                                        {
                                            AggregateDocCode docCode = d;
                                            docCode.DocExceptionLevel1 = e1;
                                            docCode.DocExceptionLevel2 = e2;
                                            docCode.DocExceptionLevel3 = e3;
                                            return docCode;
                                        }, parameters, splitOn: "ExceptionLevelID");
                return;
            }
        }
    }
}
