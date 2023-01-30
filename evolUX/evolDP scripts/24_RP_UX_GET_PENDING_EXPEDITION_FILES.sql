IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_GET_PENDING_EXPEDITION_FILES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_GET_PENDING_EXPEDITION_FILES] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_GET_PENDING_EXPEDITION_FILES]
	@BusinessID int = NULL,
	@ServiceCompanyList IDList READONLY
AS
BEGIN
	IF (@BusinessID = 0)
		SET @BusinessID = NULL

	SELECT	b.CompanyID,
			b.BusinessID,
			b.BusinessCode,
			b.[Description] [BusinessDescription],
			r.RunDate,
			r.RunSequence,     
			f.RunID,
			f.FileID,
			f.[FileName],
			est.ExpCompanyID,
			c.CompanyCode [ExpCompanyCode],
			ec.ClientName,
			ec.ContractNr,
			ec.ClientNr,
			ec.ContractID
	FROM
		VW_FULLFILLED_FILE ff
	INNER JOIN
		RT_FILE_REGIST f WITH(NOLOCK)
	ON	ff.RunID = f.RunID
		AND ff.FileID = f.FileID
	INNER JOIN
		RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	ON	pd.ProdDetailID = f.ProdDetailID
	INNER JOIN
		@CompanyList cl
	ON cl.ID = pd.ServiceCompanyID
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	ON	est.ExpCode = pd.ExpCode
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK)
	ON	c.CompanyID = est.ExpCompanyID
	INNER JOIN
		RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
	ON	stst.ServiceTaskID = est.ServiceTaskID
	INNER JOIN
		RD_ENVMEDIA_EXPCOMPANY_CONTRACT eec WITH(NOLOCK)
	ON	eec.EnvMediaID = pd.EnvMediaID
		AND eec.ExpCompanyID = est.ExpCompanyID
	INNER JOIN
		RD_EXPCOMPANY_CONTRACT ec WITH(NOLOCK)
	ON	ec.ExpCompanyID = eec.ExpCompanyID
		AND ec.ContractID = eec.ContractID
	INNER JOIN
		RT_RUN r WITH(NOLOCK)
	ON	r.RunID = f.RunID
	INNER JOIN
		RD_BUSINESS b WITH(NOLOCK)
	ON	b.BusinessID = r.BusinessID
	WHERE f.ErrorID = 0
		AND (@BusinessID is NULL OR b.BusinessID = @BusinessID)
		AND stst.ServiceTypeID = (SELECT ServiceTypeID
								FROM RD_SERVICE_TYPE
								WHERE ServiceTypeCode = 'EXPEDITION')
	ORDER BY est.ExpCompanyID, b.CompanyID, b.BusinessID, eec.ContractID, f.RunID, f.FileID
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_COMPANY_CONFIG]') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].[RD_COMPANY_CONFIG]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_COMPANY_CONFIG]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RD_COMPANY_CONFIG](
	[CompanyID] [int] NOT NULL,
	[RelationCompanyID] [int] NOT NULL,
	[RelationType] [varchar](10) NOT NULL,
 CONSTRAINT [PK_RD_COMPANY_CONFIG] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[RelationCompanyID] ASC,
	[RelationType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET NOCOUNT ON
INSERT INTO [dbo].[RD_COMPANY_CONFIG]([CompanyID], [RelationCompanyID], [RelationType])
SELECT DISTINCT 1, c.[ServiceCompanyID], 'SERVICE'
FROM [dbo].[RD_SERVICE_COMPANY_RESTRICTION] c
WHERE NOT EXISTS (SELECT TOP 1 1
				FROM [dbo].[RD_COMPANY_CONFIG]
				WHERE [CompanyID] = 1
					AND [RelationCompanyID] = c.[ServiceCompanyID]
					AND [RelationType] = 'SERVICE')

INSERT INTO [dbo].[RD_COMPANY_CONFIG]([CompanyID], [RelationCompanyID], [RelationType])
SELECT DISTINCT 1, c.[ExpCompanyID], 'EXPEDITION'
FROM [dbo].[RD_EXPCOMPANY_TYPE] c
WHERE NOT EXISTS (SELECT TOP 1 1
				FROM [dbo].[RD_COMPANY_CONFIG]
				WHERE [CompanyID] = 1
					AND [RelationCompanyID] = c.[ExpCompanyID]
					AND [RelationType] = 'EXPEDITION')

INSERT INTO [dbo].[RD_COMPANY_CONFIG]([CompanyID], [RelationCompanyID], [RelationType])
SELECT DISTINCT c1.[RelationCompanyID], c2.[RelationCompanyID], c2.[RelationType]
FROM [dbo].[RD_COMPANY_CONFIG] c1
INNER JOIN
	[dbo].[RD_COMPANY_CONFIG] c2
ON c1.[CompanyID] = c2.[CompanyID]
	AND c1.[RelationType] = 'SERVICE'
	AND c2.[RelationType] = 'EXPEDITION'
WHERE c1.[CompanyID] = 1
	AND NOT EXISTS (SELECT TOP 1 1
				FROM [dbo].[RD_COMPANY_CONFIG]
				WHERE [CompanyID] = c1.[RelationCompanyID]
					AND [RelationCompanyID] = c2.[RelationCompanyID]
					AND [RelationType] = c2.[RelationType])
GO