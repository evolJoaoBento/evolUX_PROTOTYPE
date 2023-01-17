IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_PRODUCTION_SUBSET_REPORT]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_PRODUCTION_SUBSET_REPORT] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_PRODUCTION_SUBSET_REPORT] 
	@ServiceCompanyID int,
	@RunID int
--WITH ENCRYPTION
AS
	SET NOCOUNT ON

	SELECT et.ExpeditionType, 
		e.ExpCode,
		pd.PaperMediaID,
		pd.StationMediaID,
		CAST(pd.HasColorPages as int) HasColorPages,
		et.[Priority] [ExpeditionPriority],
		e.[Priority] [ExpCodePriority],
		SUM(CASE WHEN pMedia.MaterialID is NULL THEN 0 ELSE 1 END) PaperCount,
		SUM(CASE WHEN sMedia.MaterialID is NULL THEN 0 ELSE 1 END) StationCount
	FROM RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	INNER JOIN
		RD_EXPCODE e WITH(NOLOCK)
	ON pd.ExpCode = e.ExpCode
	INNER JOIN
		RD_EXPEDITION_TYPE et WITH(NOLOCK)
	ON pd.ExpeditionType = et.ExpeditionType
	LEFT OUTER JOIN
		RT_MEDIA_CONFIG pMedia WITH(NOLOCK)
	ON pMedia.MediaID = pd.PaperMediaID
	LEFT OUTER JOIN
		RT_MEDIA_CONFIG sMedia WITH(NOLOCK)
	ON sMedia.MediaID = pd.StationMediaID
	WHERE pd.RunID = @RunID 
		AND pd.ServiceCompanyID = @ServiceCompanyID
	GROUP BY et.[Priority], e.[Priority], et.ExpeditionType, e.ExpCode, pd.PaperMediaID, pd.StationMediaID, pd.HasColorPages
	ORDER BY et.[Priority] DESC, e.[Priority] DESC, PaperCount ASC, StationCount ASC
	
	SET NOCOUNT OFF
GO