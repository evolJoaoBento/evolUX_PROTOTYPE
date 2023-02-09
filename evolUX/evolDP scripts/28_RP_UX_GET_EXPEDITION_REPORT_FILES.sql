
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_GET_EXPEDITION_REPORT_FILES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_GET_EXPEDITION_REPORT_FILES] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_GET_EXPEDITION_REPORT_FILES]
	@ExpReportID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		f.RunID,
		f.FileID,
		f.[FileName],
		ec.ExpCompanyLevel, 
		ec.MaxWeight, 
		fp.ExpeditionLevelDesc,
		fr.Quantity [PostalObjects],
		fp.TotalPostObjs
	FROM RT_EXPEDITION_REPORT er WITH(NOLOCK)
	INNER JOIN
		RT_FILE_EXPEDITION_REPORT fr WITH(NOLOCK)
	ON er.ExpReportID = fr.ExpReportID
	INNER JOIN
		RT_FILE_REGIST f WITH(NOLOCK)
	ON f.RunID = fr.RunID AND f.FileID = fr.FileID
	INNER JOIN
		RT_FILE_PRODUCTION fp
	ON fp.RunID = f.RunID AND fp.FileID = f.FileID
	INNER JOIN
		RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	ON f.ProdDetailID = pd.ProdDetailID
	INNER JOIN
		RD_EXPCOMPANY_CONFIG ec WITH(NOLOCK)
	ON ec.ExpCompanyID = er.ExpCompanyID
		AND ec.ExpeditionType = pd.ExpeditionType
		AND ec.StartDate = fr.ExpDate
		AND ec.ExpCompanyLevel = fr.ExpCompanyLevel
	INNER JOIN
		RD_EXPEDITION_EXPCENTER_EXPZONE eee WITH(NOLOCK)
	ON	eee.ExpCode = pd.ExpCode
		AND eee.ExpCenterCode = pd.ExpCenterCode 
		AND eee.ExpeditionZone = ec.ExpeditionZone
	WHERE er.ExpReportID = @ExpReportID
END
GO
