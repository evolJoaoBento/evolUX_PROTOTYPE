using Dapper;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Data.Context;
using System.Data;
using evolUX.API.Areas.EvolDP.Repositories.Interfaces;
using Shared.Models.Areas.Finishing;
using evolUX.API.Models;

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

        public async Task<IEnumerable<DocCode>> GetDocCode(string docLayout, string docType, int numRows)
        {
            string sql = @"RD_UX_GET_DOCCODE";

            var parameters = new DynamicParameters();
            parameters.Add("DocLayout", docLayout, DbType.String);
            parameters.Add("DocType", docType, DbType.String);
            if (numRows <= 0) numRows = -1;
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

        public async Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(int docCodeID, DateTime? startDate, bool? maxDateFlag)
        {
            string sql = @"RD_UX_GET_DOCCODE_CONFIG";

            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", docCodeID, DbType.Int64);
            if (startDate != null)
            {
                int startDateInt = Int32.Parse(((DateTime)startDate).ToString("yyyyMMdd"));
                parameters.Add("StartDate", startDateInt, DbType.Int64);
            }
            if (maxDateFlag != null)
            {
                parameters.Add("MaxDateFlag", maxDateFlag, DbType.Boolean);
            }
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<DocCodeConfig> docCodeConfigList = await connection.QueryAsync<DocCodeConfig>(sql, parameters,
                    commandType: CommandType.StoredProcedure);

                return docCodeConfigList;
            }
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

        public async Task PostDocCodeConfig(DocCode docCode)
        {
            string sql = @"EXEC RDC_GET_SUPORTTYPE_BY_CONFIG 
								@DOCFINISHING ,
								@DOCARCHIVE ,
								'@DOCELECTRONICFORMAT',
								@DOCEMAILHIDE,
								'@DOCEMAIL',
								@DOCELECTRONICFORMATHIDE";
            var parameters = new DynamicParameters();
            parameters.Add("DOCFINISHING", docCode.DocCodeConfigs[0].Finishing, DbType.String);
            parameters.Add("DOCARCHIVE", docCode.DocCodeConfigs[0].Archive, DbType.String);
            parameters.Add("DOCEMAIL", docCode.DocCodeConfigs[0].Email, DbType.String);
            parameters.Add("DOCEMAILHIDE", docCode.DocCodeConfigs[0].EmailHide, DbType.String);
            parameters.Add("DOCELECTRONICFORMATHIDE", docCode.DocCodeConfigs[0].ElectronicHide, DbType.String);
            parameters.Add("DOCELECTRONICFORMAT", docCode.DocCodeConfigs[0].Electronic, DbType.String);
            string sql2 = @"EXEC RD_NEW_DOCCODE_CONFIG 
								'@DOCLAYOUT',
								'@DOCSUBTYPE',
								@DOCSTARTDATE, 
								@DOCAGGREGATION,
								@DOCENVMEDIAID, 
								@DOCEXPTYPEID, 
								@DOCEXPCOMPANYID, 
								@DOCSERVICETASK,
								@DOCSUPPORTTYPE,
								@DOCPRIORITY, 
								0, 
								'@DOCCADUCITYDATE', 
								'@DOCMAXPRODDATE', 
								@DOCMAXSHEETS,
								@DOCEXCEPTIONLEVEL1, 
								@DOCEXCEPTIONLEVEL2, 
								@DOCEXCEPTIONLEVEL3,
								'@DOCDESCRIPTION',
								'@DOCARCHCADUCITY'";
            var parameters2 = new DynamicParameters();
            parameters2.Add("DOCLAYOUT", docCode.DocLayout, DbType.String);
            parameters2.Add("DOCSUBTYPE", docCode.DocType, DbType.String);
            parameters2.Add("DOCSTARTDATE", docCode.DocCodeConfigs[0].StartDate, DbType.String);
            parameters2.Add("DOCAGGREGATION", docCode.DocCodeConfigs[0].AggrCompatibility, DbType.String);
            parameters2.Add("DOCENVMEDIAID", docCode.DocCodeConfigs[0].EnvMedia, DbType.String);
            parameters2.Add("DOCEXPTYPEID", docCode.DocCodeConfigs[0].ExpeditionType, DbType.String);
            parameters2.Add("DOCEXPCOMPANYID", docCode.DocCodeConfigs[0].ExpCompanyName, DbType.String);
            parameters2.Add("DOCSERVICETASK", docCode.DocCodeConfigs[0].ServiceTaskDesc, DbType.String);

            parameters2.Add("DOCCADUCITYDATE", docCode.DocCodeConfigs[0].CaducityDate, DbType.String);
            parameters2.Add("DOCMAXPRODDATE", docCode.DocCodeConfigs[0].MaxProdDate, DbType.String);
            parameters2.Add("DOCMAXSHEETS", docCode.DocCodeConfigs[0].ProdMaxSheets, DbType.String);
            parameters2.Add("DOCEXCEPTIONLEVEL1", docCode.ExceptionLevel1.ExceptionLevelID, DbType.String);
            parameters2.Add("DOCEXCEPTIONLEVEL2", docCode.ExceptionLevel2.ExceptionLevelID, DbType.String);
            parameters2.Add("DOCEXCEPTIONLEVEL3", docCode.ExceptionLevel3.ExceptionLevelID, DbType.String);
            parameters2.Add("DOCDESCRIPTION", docCode.DocDescription, DbType.String);
            parameters2.Add("DOCARCHCADUCITY", docCode.DocCodeConfigs[0].ArchCaducityDate, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                string supportType = await connection.QueryFirstOrDefaultAsync<string>(sql, parameters);
                parameters2.Add("DOCSUPPORTTYPE", supportType, DbType.String);
                IEnumerable<GenericOptionList> treatmentTypes = await connection.QueryAsync<GenericOptionList>(sql2, parameters2);
                return;
            }
        }

        public async Task<IEnumerable<string>> DeleteDocCode(int docCodeID)
        {
            string sql = @"SET NOCOUNT ON  
								IF (EXISTS(SELECT TOP 1 * FROM RT_DOCUMENT WHERE DocCodeID = @ID ))   
								BEGIN    
									SELECT 'Existem Documentos registados com este Tipo de Documento!<BR>Não foi possível apagar Tipo de Documento!' RESULTADO   
								END   
							ELSE	
								BEGIN    
									IF (EXISTS(SELECT TOP 1 * FROM RT_DOCUMENT_SET WHERE DocCodeID = @ID ))   
										BEGIN     
											SELECT 'Existem Conjuntos de Documentos registados com este Tipo de Documento!<BR>Não foi possível apagar Tipo de Documento!' RESULTADO    
										END    
									ELSE    
										BEGIN     
											DELETE RD_DOCCODE_CONFIG    
												WHERE DocCodeID = @ID        
											DELETE RD_DOCCODE_AGGREGATION_COMPATIBILITY     
												WHERE RefDocCodeID = @ID       
													OR AggDocCodeID = @ID        
											DELETE RD_DOCCODE     
												WHERE DocCodeID = @ID        
												SELECT 'Tipo de Documento Apagado com Sucesso!' RESULTADO    
										END   
								END   
				SET NOCOUNT OFF";
            var parameters = new DynamicParameters();
            parameters.Add("ID", docCodeID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<string> results = await connection.QueryAsync<string>(sql, parameters);
                return results;
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
