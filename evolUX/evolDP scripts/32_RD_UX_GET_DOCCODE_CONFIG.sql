IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_DOCCODE_CONFIG]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_DOCCODE_CONFIG] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_DOCCODE_CONFIG]
	@DocCodeID int,
	@StartDate int = NULL,
	@MaxDateFlag bit = 0
AS
BEGIN
	SET NOCOUNT ON

	SELECT CAST((CASE WHEN CAST(CONVERT(varchar,CURRENT_TIMESTAMP,112) as int) > dc.StartDate THEN 0 ELSE 1 END) as bit) IsEditable,
	d.DocCodeID,
	d.[Description] DocDescription,
	d.PrintMatchCode,
	dc.StartDate,
	dc.AggrCompatibility,
	dc.EnvMediaID,
	evg.[Description] EnvMediaDesc,
	dc.ExpeditionType,
	dc.ExpCode,
	ce.CompanyID ExpCompanyID,
	ce.CompanyName ExpCompanyName,
	st.ServiceTaskID,
	st.ServiceTaskCode,
	st.[Description] ServiceTaskDesc,
	dc.SuportType,
	dc.[Priority], 
	--dc.AgingDays, --Descontinuado
	dc.CaducityDate,
	dc.MaxProdDate,
	dc.ProdMaxSheets,
	dc.ArchCaducityDate
	FROM
		RD_DOCCODE_CONFIG dc WITH(NOLOCK)
	INNER JOIN
		RD_DOCCODE d WITH(NOLOCK)
	ON d.DocCodeID = dc.DocCodeID
	INNER JOIN
		RD_ENVELOPE_MEDIA_GROUP evg WITH(NOLOCK)
	ON evg.[EnvMediaGroupID] = dc.EnvMediaID
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	ON est.ExpCode = dc.ExpCode
	INNER JOIN
		RD_COMPANY ce WITH(NOLOCK)
	ON ce.CompanyID = est.ExpCompanyID
	INNER JOIN
		RD_SERVICE_TASK st WITH(NOLOCK)
	ON st.ServiceTaskID = est.ServiceTaskID
	WHERE dc.DocCodeID = @DocCodeID
		AND 
		(
			dc.StartDate = @StartDate
		OR
			(@MaxDateFlag = 0 AND @StartDate is NULL)
		OR
			(@MaxDateFlag = 1 AND dc.StartDate = (SELECT MAX(StartDate) FROM RD_DOCCODE_CONFIG WHIT(NOLOCK) WHERE DocCodeID = dc.DocCodeID))
		)
	ORDER BY dc.StartDate DESC
END
GO