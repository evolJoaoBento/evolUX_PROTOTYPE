IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'RegistTimeStamp' AND OBJECT_ID = OBJECT_ID(N'RTT_FILE_EXPEDITION_REPORT_LIST'))
BEGIN
	ALTER TABLE [dbo].[RTT_FILE_EXPEDITION_REPORT_LIST]
	ADD RegistTimeStamp datetime NULL
END
GO
UPDATE [dbo].[RTT_FILE_EXPEDITION_REPORT_LIST]
SET RegistTimeStamp = CURRENT_TIMESTAMP
WHERE RegistTimeStamp is NULL
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[schema_name].[constraint_name]') AND type = N'C')
BEGIN
	ALTER TABLE [dbo].[RTT_FILE_EXPEDITION_REPORT_LIST]
	ADD CONSTRAINT DF_RTT_FILE_EXPEDITION_REPORT_LIST_RegistTimeStamp DEFAULT GETDATE() FOR RegistTimeStamp
END
GO
ALTER TABLE [dbo].[RTT_FILE_EXPEDITION_REPORT_LIST]
ALTER COLUMN RegistTimeStamp datetime NOT NULL
GO
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
			ec.ContractID,
			pd.ServiceCompanyID,
			sc.CompanyCode [ServiceCompanyCode],
			sc.CompanyName [ServiceCompanyName]
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
		@ServiceCompanyList s
	ON s.ID = pd.ServiceCompanyID
	INNER JOIN
		RD_COMPANY sc WITH(NOLOCK)
	ON	sc.CompanyID = pd.ServiceCompanyID
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
	LEFT OUTER JOIN
		RTT_FILE_EXPEDITION_REPORT_LIST tt
	ON tt.RunID = f.RunID AND tt.FileID = f.FileID
		AND DATEADD(hour, -2, CURRENT_TIMESTAMP) < tt.RegistTimeStamp
	WHERE f.ErrorID = 0
		AND (@BusinessID is NULL OR b.BusinessID = @BusinessID)
		AND stst.ServiceTypeID = (SELECT ServiceTypeID
								FROM RD_SERVICE_TYPE
								WHERE ServiceTypeCode = 'EXPEDITION')
		AND tt.RunID is NULL
	ORDER BY pd.ServiceCompanyID, est.ExpCompanyID, b.CompanyID, b.BusinessID, f.RunID, eec.ContractID, f.FileID
END
GO
