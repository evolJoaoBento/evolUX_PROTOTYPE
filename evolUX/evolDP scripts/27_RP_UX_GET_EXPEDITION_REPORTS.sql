IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_GET_EXPEDITION_REPORTS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_GET_EXPEDITION_REPORTS] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_GET_EXPEDITION_REPORTS]
	@BusinessID int = NULL,
	@ServiceCompanyList IDList READONLY,
	@FromDate Datetime = NULL,
	@UntilDate Datetime = NULL
AS
BEGIN
	IF (@BusinessID = 0)
		SET @BusinessID = NULL

	SELECT	DISTINCT b.CompanyID,
			b.BusinessID,
			b.BusinessCode,
			b.[Description] [BusinessDescription],
			rp.ExpCompanyID,
			c.CompanyCode [ExpCompanyCode],
			ec.ClientName,
			ec.ContractNr,
			ec.ClientNr,
			ec.ContractID,
			pd.ServiceCompanyID,
			sc.CompanyCode [ServiceCompanyCode],
			sc.CompanyName [ServiceCompanyName],
			CONVERT(varchar,rp.ExpTimeStamp,112) ExpTimeDate, 
			rp.ExpReportNr,  
			rp.ExpTimeStamp,  
			rp.ExpTime,  
			rp.ExpReportID, 
			rp.ExpRegistReportID 
	FROM RT_EXPEDITION_REPORT rp WITH(NOLOCK)
	INNER JOIN
		RT_FILE_EXPEDITION_REPORT fe WITH(NOLOCK)
	ON	fe.ExpReportID = rp.ExpReportID
	INNER JOIN
		RD_EXPCOMPANY_CONTRACT ec WITH(NOLOCK)
	ON	ec.ContractID = rp.ContractID 
		AND ec.ExpCompanyID = rp.ExpCompanyID 
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK)
	ON c.CompanyID = ec.ExpCompanyID
	INNER JOIN
		RT_RUN r WITH(NOLOCK)
	ON r.RunID = fe.RunID
	INNER JOIN
		RD_BUSINESS b WITH(NOLOCK)
	ON b.BusinessID = r.BusinessID
	INNER JOIN
		RT_FILE_REGIST f WITH(NOLOCK)
	ON f.RunID = fe.RunID AND f.FileID = fe.FileID
	INNER JOIN
		RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	ON pd.ProdDetailID = f.ProdDetailID
		INNER JOIN
			@ServiceCompanyList s
		ON s.ID = pd.ServiceCompanyID
	INNER JOIN
		RD_COMPANY sc WITH(NOLOCK)
	ON sc.CompanyID = pd.ServiceCompanyID
	WHERE (@BusinessID is NULL OR b.BusinessID = @BusinessID)
		AND rp.ExpTimeStamp >= ISNULL(@FromDate, DATEADD(YEAR,-1, CURRENT_TIMESTAMP)) 
		AND rp.ExpTimeStamp <= ISNULL(@UntilDate, CURRENT_TIMESTAMP)
	ORDER BY pd.ServiceCompanyID, rp.ExpCompanyID, b.CompanyID, b.BusinessID, rp.ExpTimeStamp DESC, ec.ContractID
END
GO


