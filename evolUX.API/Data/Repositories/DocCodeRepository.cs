using Dapper;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Data.Repositories
{
    public class DocCodeRepository : IDocCodeRepository
    {
        private readonly DapperContext _context;

        public DocCodeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> GetDocCode()
        {
            var docCodeList = new List<dynamic>();
            string sql = @"SELECT	 
d.DocLayout,
d.DocType,
d1.Description 
FROM RD_DOCCODE d with(nolock)
LEFT OUTER JOIN 
RD_DOCCODE d1 with(nolock) ON d1.DocLayout = d.DocLayout 
AND d1.DocType = d.DocType 
AND d1.ExceptionLevel1ID is NULL 
AND d1.ExceptionLevel2ID is NULL 
AND d1.ExceptionLevel3ID is NULL 
GROUP BY d.DocLayout, d.DocType, d1.Description";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                docCodeList = (List<dynamic>)await connection.QueryAsync(sql);
                return docCodeList;
            }
        }

        public async Task<List<dynamic>> GetDocCodeLevel1(dynamic data)
        {
            var docCodeList = new List<dynamic>();
            string sql = @"SELECT	
    d.DocCodeID [DocCodeId],
	d.DocLayout [Layout do Documento],
	d.DocType [Subtipo do Documento],
	e1.ExceptionLevelID [EXCEPTIONLEVEL1ID],
	e1.ExceptionCode [EXCEPTIONLEVEL1CODE],
	e1.ExceptionDescription [EXCEPTIONLEVEL1DISCRIPTION],
	e2.ExceptionLevelID [EXCEPTIONLEVEL2ID],
	e2.ExceptionCode [EXCEPTIONLEVEL2CODE],
	e2.ExceptionDescription [EXCEPTIONLEVEL2DISCRIPTION],
	e3.ExceptionLevelID [EXCEPTIONLEVEL3ID],
	e3.ExceptionCode [EXCEPTIONLEVEL3CODE],
	e3.ExceptionDescription [EXCEPTIONLEVEL3DISCRIPTION],
	[Description] [Descrição]
FROM 	RD_DOCCODE d WITH(NOLOCK)
LEFT OUTER JOIN
	RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
ON	e1.ExceptionLevelID = d.ExceptionLevel1ID
LEFT OUTER JOIN
	RDC_EXCEPTION_LEVEL2 e2 WITH(NOLOCK)
ON	e2.ExceptionLevelID = d.ExceptionLevel2ID
LEFT OUTER JOIN
	RDC_EXCEPTION_LEVEL3 e3 WITH(NOLOCK)
ON	e3.ExceptionLevelID = d.ExceptionLevel3ID
WHERE d.DocLayout = @DOCLAYOUT
	AND d.DocType = @DOCTYPE";

            var parameters = new DynamicParameters();
            parameters.Add("DOCLAYOUT", data.doclayout, DbType.String);
            parameters.Add("DOCTYPE", data.docsubtype, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                docCodeList = (List<dynamic>)await connection.QueryAsync(sql, parameters);
                return docCodeList;
            }
        }

        public async Task<List<dynamic>> GetDocCodeLevel2(dynamic data)
        {
            var docCodeList = new List<dynamic>();
            string sql = @"SET NOCOUNT ON
IF (NOT EXISTS(SELECT TOP 1 * FROM RD_DOCCODE_CONFIG WITH(NOLOCK)
	WHERE DocCodeID = @DOCCODEID))
BEGIN
	SELECT '<DOCCODEID>' + cast(d.DocCodeID as varchar) + '</DOCCODEID><DATA>0</DATA>'  as l_queryItem,
		'Ver Detalhe' as l_queryItemStr,
        d.DocCodeId [DocCodeId],
		d.DocLayout [Layout do Documento],
		d.DocType [Subtipo do Documento],
		e1.ExceptionDescription [@PARAMETERS/evolDP/EXCEPTIONLEVEL1ID ],
		e2.ExceptionDescription [@PARAMETERS/evolDP/EXCEPTIONLEVEL2ID ],
		e3.ExceptionDescription [@PARAMETERS/evolDP/EXCEPTIONLEVEL3ID ],
		'Não existem configurações para o Tipo de Documento' [Resultado],
		[Description] [Descrição]
	FROM 	RD_DOCCODE d WITH(NOLOCK)
	LEFT OUTER JOIN
		RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
	ON	e1.ExceptionLevelID = d.ExceptionLevel1ID
	LEFT OUTER JOIN
		RDC_EXCEPTION_LEVEL1 e2 WITH(NOLOCK)
	ON	e2.ExceptionLevelID = d.ExceptionLevel2ID
	LEFT OUTER JOIN
		RDC_EXCEPTION_LEVEL1 e3 WITH(NOLOCK)
	ON	e3.ExceptionLevelID = d.ExceptionLevel3ID
	WHERE d.DocCodeID = @DOCCODEID
END
ELSE
BEGIN
	SELECT '<DOCCODEID>' + cast(d.DocCodeID as varchar) + '</DOCCODEID><DATA>' + cast(dcc.StartDate as varchar) + '</DATA>'  as l_queryItem,
		'Ver Detalhe da configuração para Data = ' + CAST(dcc.StartDate as varchar) as l_queryItemStr,
        d.DocCodeId [DocCodeId],		
        d.DocLayout [Layout do Documento],
		d.DocType [Subtipo do Documento],
		e1.ExceptionDescription [@PARAMETERS/evolDP/EXCEPTIONLEVEL1ID ],
		e2.ExceptionDescription [@PARAMETERS/evolDP/EXCEPTIONLEVEL2ID ],
		e3.ExceptionDescription [@PARAMETERS/evolDP/EXCEPTIONLEVEL3ID ],
		dcc.StartDate [Data Efectiva],
		d.[Description] [Descrição]
	FROM 	RD_DOCCODE d with(nolock)
	INNER JOIN
		RD_DOCCODE_CONFIG dcc with(nolock)
	ON 	d.DocCodeID = dcc.DocCodeID
	LEFT OUTER JOIN
		RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
	ON	e1.ExceptionLevelID = d.ExceptionLevel1ID
	LEFT OUTER JOIN
		RDC_EXCEPTION_LEVEL2 e2 WITH(NOLOCK)
	ON	e2.ExceptionLevelID = d.ExceptionLevel2ID
	LEFT OUTER JOIN
		RDC_EXCEPTION_LEVEL3 e3 WITH(NOLOCK)
	ON	e3.ExceptionLevelID = d.ExceptionLevel3ID
	WHERE	d.DocCodeID=@DOCCODEID
	ORDER BY dcc.StartDate DESC
END
SET NOCOUNT OFF";

            var parameters = new DynamicParameters();
            parameters.Add("DOCCODEID", data.doccodeid, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                docCodeList = (List<dynamic>)await connection.QueryAsync(sql, parameters);
                return docCodeList;
            }
        }

		public async Task<dynamic> GetDocCodeConfig(dynamic data)
		{
			string sql = "RD_GET_DOCCODE_CONFIG";
			var parameters = new DynamicParameters();
			parameters.Add("DocCodeID", data.doccodeid, DbType.String);
			parameters.Add("DocStartDate", data.docstartdate, DbType.Int64);
			using (var connection = _context.CreateConnectionEvolDP())
			{
				var docCodeConfig = (dynamic)await connection.QueryAsync(sql, parameters, commandType: CommandType.StoredProcedure);
				return docCodeConfig;
			}

		}
		
		public async Task<dynamic> GetDocCodeExceptionOptions(dynamic data)
		{
			string sql = "RD_GET_DOCCODE_CONFIG";
			var parameters = new DynamicParameters();
			parameters.Add("DocCodeID", data.doccodeid, DbType.String);
			parameters.Add("DocStartDate", data.docstartdate, DbType.Int64);
			using (var connection = _context.CreateConnectionEvolDP())
			{
				var docCodeConfig = (dynamic)await connection.QueryAsync(sql, parameters, commandType: CommandType.StoredProcedure);
				return docCodeConfig;
			}

		}
	}
}
