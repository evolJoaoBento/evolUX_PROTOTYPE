IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_SERVICECOMPANY_PENDING_RECOVER]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_SERVICECOMPANY_PENDING_RECOVER] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_SERVICECOMPANY_PENDING_RECOVER]
	@ServiceCompanyID int
--WITH ENCRYPTION
AS
	SET NOCOUNT ON
	SELECT	rr.RunID, fr.[FileName], 
		CASE WHEN rr.StartPostObjID = 0 AND rr.EndPostObjID = 0
			THEN 1 --'Total' 
			ELSE 2 --'Parcial'
			END AS RecoverType, 
		rr.StartPostObjID, 
		rr.EndPostObjID, 
		rr.UserName 
	FROM
		RT_RECOVER_REGIST rr WITH(NOLOCK)
	INNER JOIN 
		RT_FILE_REGIST fr WITH(NOLOCK)
	ON rr.RunID = fr.RunID 
		AND rr.FileID = fr.FileID
	INNER JOIN
		RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	ON fr.ProdDetailID = pd.ProdDetailID
	WHERE rr.RecStartTime is NULL 
		AND pd.ServiceCompanyID = @ServiceCompanyID
	ORDER BY rr.RunID, rr.FileID, rr.RequestID 
GO