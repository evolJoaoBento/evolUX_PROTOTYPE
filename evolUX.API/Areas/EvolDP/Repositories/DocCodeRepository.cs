using Dapper;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Data.Context;
using System.Data;
using evolUX.API.Areas.EvolDP.Repositories.Interfaces;

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

        public async Task<IEnumerable<DocCode>> GetDocCode(string docLayout, string docType)
        {
            string sql = @"RD_UX_GET_DOCCODE";

            var parameters = new DynamicParameters();
            parameters.Add("DocLayout", docLayout, DbType.String);
            parameters.Add("DocType", docType, DbType.String);
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
                                        }, parameters, splitOn: "ExceptionLevelID");
                return docCodeList;
            }
        }
        // HANDLE DOC MESSAGE IN UPPER LEVELS
        // DATA VALIDATION SHOULD BE DONE ON THE SERVICE LAYER
        public async Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(string ID)
        {
            string sql = @"	SET NOCOUNT ON
							IF (NOT EXISTS(SELECT TOP 1 * 
									FROM RD_DOCCODE_CONFIG WITH(NOLOCK)
									WHERE DocCodeID = @DOCCODEID))
							BEGIN
								SELECT	d.DocCodeId as DocCodeID,
										d.DocLayout as DocLayout,
										d.DocType as DocType,
										'NO CONFIG' as DocMessage,
										[Description] as DocDescription
										e1.ExceptionDescription,
										e2.ExceptionDescription,
										e3.ExceptionDescription,
								FROM 	RD_DOCCODE d WITH(NOLOCK)
								LEFT OUTER JOIN
										RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
								ON		e1.ExceptionLevelID = d.ExceptionLevel1ID
								LEFT OUTER JOIN
										RDC_EXCEPTION_LEVEL1 e2 WITH(NOLOCK)
								ON		e2.ExceptionLevelID = d.ExceptionLevel2ID
								LEFT OUTER JOIN
									RDC_EXCEPTION_LEVEL1 e3 WITH(NOLOCK)
								ON	e3.ExceptionLevelID = d.ExceptionLevel3ID
								WHERE d.DocCodeID = @DOCCODEID
							END
							ELSE
							BEGIN
								SELECT	d.DocCodeId as DocCodeID,		
										d.DocLayout as DocLayout,
										d.DocType as DocType,
										dcc.StartDate,
										d.[Description] as DocDescription
										e1.ExceptionDescription,
										e2.ExceptionDescription,
										e3.ExceptionDescription,
								FROM 	RD_DOCCODE d with(nolock)
								INNER JOIN
										RD_DOCCODE_CONFIG dcc with(nolock)
								ON 		d.DocCodeID = dcc.DocCodeID
								LEFT OUTER JOIN
										RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
								ON		e1.ExceptionLevelID = d.ExceptionLevel1ID
								LEFT OUTER JOIN
										RDC_EXCEPTION_LEVEL2 e2 WITH(NOLOCK)
								ON		e2.ExceptionLevelID = d.ExceptionLevel2ID
								LEFT OUTER JOIN
										RDC_EXCEPTION_LEVEL3 e3 WITH(NOLOCK)
								ON		e3.ExceptionLevelID = d.ExceptionLevel3ID
								WHERE	d.DocCodeID=@DOCCODEID
								ORDER BY dcc.StartDate DESC
							END
							SET NOCOUNT OFF";

            var parameters = new DynamicParameters();
            parameters.Add("DOCCODEID", ID, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<DocCodeConfig> docCodeConfigList = await connection.QueryAsync<DocCodeConfig, ExceptionLevel, ExceptionLevel, ExceptionLevel, DocCodeConfig>(sql,
                    (d, e1, e2, e3) =>
                    {
                        DocCodeConfig docCodeConfig = d;
                        docCodeConfig.DocExceptionLevel1 = e1;
                        docCodeConfig.DocExceptionLevel2 = e2;
                        docCodeConfig.DocExceptionLevel3 = e3;
                        return docCodeConfig;
                    }, parameters, splitOn: "ExceptionDescription");
                return docCodeConfigList;
            }
        }

        //HANDLE FIELD NAMES IN UPPER LEVELS finishing, exceptions, archive, email, email hide, electronic, electronic hide
        //HANDLE TEXT RESPONSES IN UPPER LEVELS result, print match code, aggrcompatibility, fnishing, archive, email, emailhide, eletronic, eletronic hide
        //RD_GET_DOCCODE_CONFIG
        public async Task<DocCodeConfig> GetDocCodeConfig(string ID, int startdate)
        {
            string sql = @"	SELECT
										d.DocCodeID,
										d.DocLayout,
										d.DocType,
										e1.ExceptionDescription,
										e2.ExceptionDescription,
										e3.ExceptionDescription, 
										d.[Description] as DocDescription,
										d.[PrintMatchCode] as PrintMatchCode,
										dcc.StartDate,
										dcc.EnvMedia,
										dcc.AggrCompatibility,
										dcc.Priority,
										dcc.ProdMaxSheets,
										dcc.CompanyName,
										dcc.expDesc as ExpeditionType,
										dcc.treatType as TreatmentType,
										dcc.production as Finishing, 
										dcc.CaducityDate,
										dcc.MaxProdDate,
										dcc.Archive,
										dcc.ArchCaducityDate,
										dcc.email as Email, 
										dcc.emailHide as EmailHide,
										dcc.electFormat as Electronic,
										dcc.electFormatHide as ElectronicHide
							FROM RD_DOCCODE d WITH(NOLOCK)
							LEFT OUTER JOIN
								RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
							ON e1.ExceptionLevelID = d.ExceptionLevel1ID
							LEFT OUTER JOIN
								RDC_EXCEPTION_LEVEL2 e2 WITH(NOLOCK)
							ON e2.ExceptionLevelID = d.ExceptionLevel2ID
							LEFT OUTER JOIN
								RDC_EXCEPTION_LEVEL3 e3 WITH(NOLOCK)
							ON e3.ExceptionLevelID = d.ExceptionLevel3ID
							LEFT OUTER JOIN
										(SELECT		dc.DocCodeID,
													dc.StartDate,
													dc.AggrCompatibility as AggrCompatibility,
													em.[Description] as EnvMedia,
													et.[Description] as expDesc, --[Tipo de expediçao],
													st.[Description] as treatType, --[Tipo de tratamento],
													cst.Finishing as finishing,
													cst.Archive as Archive,
													cst.EMail as email,
													cst.EMailHide as emailHide,
													cst.Electronic  as electFormat,
													cst.ElectronicHide as electFormatHide,
													c.CompanyName,
													dc.Priority,
													dc.CaducityDate,
													dc.MaxProdDate,
													dc.ArchCaducityDate,
													dc.ProdMaxSheets
										FROM	RD_DOCCODE_CONFIG dc  WITH(NOLOCK),
												RD_EXPCODE ec  WITH(NOLOCK),
												RD_EXPCOMPANY_SERVICE_TASK est  WITH(NOLOCK),
												RD_SERVICE_TASK st  WITH(NOLOCK),
												RD_COMPANY c  WITH(NOLOCK),
												RD_EXPEDITION_TYPE et WITH(NOLOCK),
												RDC_SUPORT_TYPE cst WITH(NOLOCK),
												RD_ENVELOPE_MEDIA_GROUP em WITH(NOLOCK)
										WHERE	dc.ExpCode = ec.ExpCode
												AND
												dc.ExpeditionType = et.ExpeditionType
												AND
												dc.SuportType = cst.SuportType
												AND
												ec.ExpCode = est.ExpCode
												AND
												est.ServiceTaskID = st.ServiceTaskID
												AND
												est.ExpCompanyID = c.CompanyID
												AND
												em.EnvMediaGroupID = dc.EnvMediaID
												AND(@DocStartDate is NULL OR dc.StartDate = @DocStartDate)) dcc
										ON d.DocCodeID = dcc.DocCodeID
										WHERE d.DocCodeID = @DocCodeID
									";
            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", ID, DbType.String);
            parameters.Add("DocStartDate", startdate, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<DocCodeConfig> docCodeConfigList = await connection.QueryAsync<DocCodeConfig, ExceptionLevel, ExceptionLevel, ExceptionLevel, DocCodeConfig>(sql,
                    (d, e1, e2, e3) =>
                    {
                        DocCodeConfig docCodeConfig = d;
                        docCodeConfig.DocExceptionLevel1 = e1;
                        docCodeConfig.DocExceptionLevel2 = e2;
                        docCodeConfig.DocExceptionLevel3 = e3;
                        return docCodeConfig;
                    }, parameters, splitOn: "ExceptionDescription");
                return docCodeConfigList.AsList().FirstOrDefault();
            }

        }


        public async Task<DocCodeConfig> GetDocCodeConfigOptions(string ID)
        {
            string sql = $@"SELECT	[DocCodeID]
									,[StartDate]
							FROM	RD_DOCCODE_CONFIG
							where	DocCodeID =@DOCCODEID";

            var parameters = new DynamicParameters();
            parameters.Add("DocCodeID", ID, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                DocCodeConfig docCodeConfig = await connection.QueryFirstOrDefaultAsync<DocCodeConfig>(sql,
                    parameters);
                return await GetDocCodeConfig(ID, int.Parse(docCodeConfig.StartDate));
            }
        }

        //HANDLE TEXT RESPONSES IN UPPER LEVELS
        public async Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel1()
        {
            string sql = $@"SELECT	ExceptionLevelID,
									ExceptionCode,
									ExceptionDescription
							FROM	RDC_EXCEPTION_LEVEL1
							ORDER BY Caption";

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExceptionLevel> docCodeException = await connection.QueryAsync<ExceptionLevel>(sql);
                return docCodeException;
            }
        }

        public async Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel2()
        {
            string sql = $@"SELECT	ExceptionLevelID,
									ExceptionCode,
									ExceptionDescription
							FROM	RDC_EXCEPTION_LEVEL2
							ORDER BY Caption";

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExceptionLevel> docCodeException = await connection.QueryAsync<ExceptionLevel>(sql);
                return docCodeException;
            }
        }

        public async Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel3()
        {
            string sql = $@"SELECT	ExceptionLevelID,
									ExceptionCode,
									ExceptionDescription
							FROM	RDC_EXCEPTION_LEVEL3
							ORDER BY Caption";

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExceptionLevel> docCodeException = await connection.QueryAsync<ExceptionLevel>(sql);
                return docCodeException;
            }
        }

        public async Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups(string envMediaGroupID)
        {
            string sql = $@"SELECT		e1.Description,
									e1.EnvMediaGroupID
						FROM		[DMS_evolDP].[dbo].[RD_ENVELOPE_MEDIA_GROUP] e1
						LEFT OUTER JOIN   
									[DMS_evolDP].[dbo].[RD_ENVELOPE_MEDIA_GROUP] e2
						ON			e1.EnvMediaGroupID<>e2.EnvMediaGroupID
							AND		e2.EnvMediaGroupID = @envMediaGroupID  
						ORDER BY	e2.EnvMediaGroupID";

            var parameters = new DynamicParameters();
            parameters.Add("envMediaGroupID", envMediaGroupID, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<EnvelopeMedia> envMedia = await connection.QueryAsync<EnvelopeMedia>(sql,
                    parameters);
                return envMedia;
            }
        }

        public async Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups()
        {
            string sql = $@"SELECT		e1.Description,
									e1.EnvMediaGroupID
						FROM		RD_ENVELOPE_MEDIA_GROUP e1";

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<EnvelopeMedia> envMedia = await connection.QueryAsync<EnvelopeMedia>(sql);
                return envMedia;
            }
        }

        //HANDLE TEXT RESPONSES IN UPPER LEVELS
        public async Task<IEnumerable<int>> GetAggregationList(string aggrCompatibility)
        {
            int aggrcomp = int.Parse(aggrCompatibility);
            List<int> ilist = new List<int> { 0, 1, 2, 3 };
            foreach (int i in ilist)
            {
                if (i != aggrcomp)
                    ilist.Remove(i);
            }
            IEnumerable<int> enumerable = ilist as IEnumerable<int>;
            return enumerable;
        }

        public async Task<IEnumerable<Company>> GetExpeditionCompanies(string expCompanyID)
        {
            string sql = $@"SELECT	c1.CompanyName,
									c1.CompanyID as  Value
							FROM	RD_COMPANY c1
							INNER JOIN
								(SELECT DISTINCT	(ExpCompanyID) ExpCompanyID
								FROM 				RD_EXPCOMPANY_SERVICE_TASK) est
							ON est.ExpCompanyID = c1.CompanyID
							LEFT OUTER JOIN
								RD_COMPANY c2
							ON c1.CompanyID <> c2.CompanyID
							AND c2.CompanyID = @expCompanyID
							ORDER BY c2.CompanyID";
            var parameters = new DynamicParameters();
            parameters.Add("expCompanyID", expCompanyID, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Company> expeditionCompanies = await connection.QueryAsync<Company>(sql, parameters);
                return expeditionCompanies;
            }
        }
        public async Task<IEnumerable<Company>> GetExpeditionCompanies()
        {
            string sql = $@"SELECT	c.CompanyName,
									c.CompanyID
							FROM	RD_COMPANY c
							WHERE 	CompanyID in
									(	SELECT		distinct(ExpCompanyID) 
										FROM 		RD_EXPCOMPANY_SERVICE_TASK)
							ORDER BY c.CompanyID DESC";

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Company> expeditionCompanies = await connection.QueryAsync<Company>(sql);
                return expeditionCompanies;
            }
        }

        public async Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes(string expeditionType)
        {
            string sql = $@"SELECT 		e1.Description,	
										e1.ExpeditionType
							FROM 		RD_EXPEDITION_TYPE e1
							LEFT OUTER JOIN
							RD_EXPEDITION_TYPE e2
							ON			e1.ExpeditionType<>e2.ExpeditionType
							AND			e2.ExpeditionType = '@expeditionType'
							ORDER BY	e2.ExpeditionType";
            var parameters = new DynamicParameters();
            parameters.Add("expeditionType", expeditionType, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpeditionsType> expeditionCompanies = await connection.QueryAsync<ExpeditionsType>(sql, parameters);
                return expeditionCompanies;
            }
        }
        public async Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes()
        {
            string sql = $@"SELECT	Description as caption,	
									ExpeditionType
							FROM 	RD_EXPEDITION_TYPE";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ExpeditionsType> expeditionCompanies = await connection.QueryAsync<ExpeditionsType>(sql);
                return expeditionCompanies;
            }
        }

        public async Task<IEnumerable<ServiceTask>> GetTreatmentTypes(string treatmentType)
        {
            string sql = $@"SELECT 	e1.Description,
									e1.ServiceTaskID as value
							FROM 	RD_SERVICE_TASK e1
							LEFT OUTER JOIN
									RD_SERVICE_TASK e2
							ON e1.ServiceTaskID<>e2.ServiceTaskID
							AND e2.ServiceTaskID = '@treatmentType'
							ORDER BY e2.ServiceTaskID";
            var parameters = new DynamicParameters();
            parameters.Add("treatmentType", treatmentType, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceTask> treatmentTypes = await connection.QueryAsync<ServiceTask>(sql, parameters);
                return treatmentTypes;
            }
        }
        public async Task<IEnumerable<ServiceTask>> GetTreatmentTypes()
        {
            string sql = $@"SELECT 	Description,
									ServiceTaskID
							FROM 	RD_SERVICE_TASK";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceTask> treatmentTypes = await connection.QueryAsync<ServiceTask>(sql);
                return treatmentTypes;
            }
        }
        //HANDLE TEXT RESPONSES IN UPPER LEVELS
        public async Task<IEnumerable<int>> GetFinishingList(string finishing)
        {
            int finish = int.Parse(finishing);
            List<int> ilist = new List<int> { 0, 1, 2, 3 };
            foreach (int i in ilist)
            {
                if (i != finish)
                    ilist.Remove(i);
            }
            IEnumerable<int> enumerable = ilist as IEnumerable<int>;
            return enumerable;
        }
        //HANDLE TEXT RESPONSES IN UPPER LEVELS
        public async Task<IEnumerable<int>> GetArchiveList(string archive)
        {
            int arch = int.Parse(archive);
            List<int> ilist = new List<int> { 0, 1, 2, 3 };
            foreach (int i in ilist)
            {
                if (i != arch)
                    ilist.Remove(i);
            }
            IEnumerable<int> enumerable = ilist as IEnumerable<int>;
            return enumerable;
        }

        public async Task<IEnumerable<Email>> GetEmailList(string email)
        {
            string sql = $@"SELECT 	s1.SuportStream as EmailDescription,
									s1.SuportTypevalue as EmailID
							FROM 	(SELECT SuportStream, SuportTypevalue, SuportTypeDescription
							FROM RDC_SUPORT_TYPE_REFERENCE
							UNION SELECT 'Não Enviar' SuportStream, 0 SuportTypevalue, 'Email' SuportTypeDescription) s1
							LEFT OUTER JOIN
									(SELECT SuportStream, SuportTypevalue
									FROM	RDC_SUPORT_TYPE_REFERENCE
									UNION	SELECT 'Não Enviar' SuportStream, 0 SuportTypevalue) s2
							ON s1.SuportTypevalue<>s2.SuportTypevalue
								AND s2.SuportStream = '@email'
							WHERE s1.SuportTypeDescription = 'Email'
							ORDER BY s2.SuportTypevalue";
            var parameters = new DynamicParameters();
            parameters.Add("email", email, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Email> treatmentTypes = await connection.QueryAsync<Email>(sql, parameters);
                return treatmentTypes;
            }
        }
        public async Task<IEnumerable<Email>> GetEmailList()
        {
            string sql = $@"SELECT  SuportStream as caption,
									SuportTypevalue as value  
							FROM  
									RDC_SUPORT_TYPE_REFERENCE	
							WHERE SuportTypeDescription='Email'";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Email> treatmentTypes = await connection.QueryAsync<Email>(sql);
                return treatmentTypes;
            }
        }
        //HANDLE TEXT RESPONSES IN UPPER LEVELS
        public async Task<IEnumerable<int>> GetEmailHideList(string emailHide)
        {
            int emailH = int.Parse(emailHide);
            List<int> ilist = new List<int> { 0, 1, 2, 3 };
            foreach (int i in ilist)
            {
                if (i != emailH)
                    ilist.Remove(i);
            }
            IEnumerable<int> enumerable = ilist as IEnumerable<int>;
            return enumerable;
        }

        public async Task<IEnumerable<Electronic>> GetElectronicList(string electronic)
        {
            string sql = $@"SELECT 	s1.SuportStream,
									s1.SuportTypevalue
							FROM 	(SELECT SuportStream, SuportTypevalue, SuportTypeDescription
									FROM [DMS_evolDP].[dbo].[RDC_SUPORT_TYPE_REFERENCE]
									UNION SELECT 'Não Enviar' SuportStream, 0 SuportTypevalue, 'Electronic' SuportTypeDescription) s1
							LEFT OUTER JOIN
									(SELECT SuportStream, SuportTypevalue
									FROM [DMS_evolDP].[dbo].[RDC_SUPORT_TYPE_REFERENCE]
									UNION SELECT 'Não Enviar' SuportStream, 0 SuportTypevalue) s2
							ON s1.SuportTypevalue<>s2.SuportTypevalue
								AND s2.SuportStream = '@electronic'
							WHERE s1.SuportTypeDescription = 'Electronic'
							ORDER BY s2.SuportTypevalue";
            var parameters = new DynamicParameters();
            parameters.Add("electronic", electronic, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Electronic> treatmentTypes = await connection.QueryAsync<Electronic>(sql, parameters);
                return treatmentTypes;
            }
        }

        public async Task<IEnumerable<Electronic>> GetElectronicList()
        {
            string sql = $@"SELECT  SuportStream, 
									SuportTypevalue  
							FROM	RDC_SUPORT_TYPE_REFERENCE  
							WHERE	SuportTypeDescription='Electronic'";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Electronic> treatmentTypes = await connection.QueryAsync<Electronic>(sql);
                return treatmentTypes;
            }
        }
        //HANDLE TEXT RESPONSES IN UPPER LEVELS
        public async Task<IEnumerable<int>> GetElectronicHideList(string electronicHide)
        {
            int electronicH = int.Parse(electronicHide);
            List<int> ilist = new List<int> { 0, 1, 2, 3 };
            foreach (int i in ilist)
            {
                if (i != electronicH)
                    ilist.Remove(i);
            }
            IEnumerable<int> enumerable = ilist as IEnumerable<int>;
            return enumerable;
        }

        public async Task PostDocCodeConfig(DocCodeConfig model)
        {
            string sql = @"EXEC RDC_GET_SUPORTTYPE_BY_CONFIG 
								@DOCFINISHING ,
								@DOCARCHIVE ,
								'@DOCELECTRONICFORMAT',
								@DOCEMAILHIDE,
								'@DOCEMAIL',
								@DOCELECTRONICFORMATHIDE";
            var parameters = new DynamicParameters();
            parameters.Add("DOCFINISHING", model.Finishing, DbType.String);
            parameters.Add("DOCARCHIVE", model.Archive, DbType.String);
            parameters.Add("DOCEMAIL", model.Email, DbType.String);
            parameters.Add("DOCEMAILHIDE", model.EmailHide, DbType.String);
            parameters.Add("DOCELECTRONICFORMATHIDE", model.ElectronicHide, DbType.String);
            parameters.Add("DOCELECTRONICFORMAT", model.Electronic, DbType.String);
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
            parameters2.Add("DOCLAYOUT", model.DocLayout, DbType.String);
            parameters2.Add("DOCSUBTYPE", model.DocType, DbType.String);
            parameters2.Add("DOCSTARTDATE", model.StartDate, DbType.String);
            parameters2.Add("DOCAGGREGATION", model.AggrCompatibility, DbType.String);
            parameters2.Add("DOCENVMEDIAID", model.EnvMedia, DbType.String);
            parameters2.Add("DOCEXPTYPEID", model.ExpeditionType, DbType.String);
            parameters2.Add("DOCEXPCOMPANYID", model.CompanyName, DbType.String);
            parameters2.Add("DOCSERVICETASK", model.TreatmentType, DbType.String);

            parameters2.Add("DOCCADUCITYDATE", model.CaducityDate, DbType.String);
            parameters2.Add("DOCMAXPRODDATE", model.MaxProdDate, DbType.String);
            parameters2.Add("DOCMAXSHEETS", model.ProdMaxSheets, DbType.String);
            parameters2.Add("DOCEXCEPTIONLEVEL1", model.DocExceptionLevel1.ExceptionLevelID, DbType.String);
            parameters2.Add("DOCEXCEPTIONLEVEL2", model.DocExceptionLevel2.ExceptionLevelID, DbType.String);
            parameters2.Add("DOCEXCEPTIONLEVEL3", model.DocExceptionLevel3.ExceptionLevelID, DbType.String);
            parameters2.Add("DOCDESCRIPTION", model.DocDescription, DbType.String);
            parameters2.Add("DOCARCHCADUCITY", model.ArchCaducityDate, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                string supportType = await connection.QueryFirstOrDefaultAsync<string>(sql, parameters);
                parameters2.Add("DOCSUPPORTTYPE", supportType, DbType.String);
                IEnumerable<Electronic> treatmentTypes = await connection.QueryAsync<Electronic>(sql2, parameters2);
                return;
            }
        }

        public async Task<IEnumerable<string>> DeleteDocCode(string ID)
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
            parameters.Add("ID", ID, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<string> results = await connection.QueryAsync<string>(sql, parameters);
                return results;
            }
        }


        //HANDLE TEXT RESPONSES ON HIGHER LEVELS aggrCompatibility
        public async Task<IEnumerable<AggregateDocCode>> GetAggregateDocCodes(string ID)
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
            parameters.Add("DOCCODEID", ID, DbType.String);

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
        public async Task<AggregateDocCode> GetAggregateDocCode(string ID)
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
            parameters.Add("DOCCODEID", ID, DbType.String);

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
