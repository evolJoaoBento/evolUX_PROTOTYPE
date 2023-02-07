IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_PRODUCTION_SUBSET_REPORT]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_PRODUCTION_SUBSET_REPORT] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_PRODUCTION_SUBSET_REPORT] 
	@ServiceCompanyID int,
	@RunIDList IDList READONLY
--WITH ENCRYPTION
AS
	SET NOCOUNT ON

	SELECT pd.RunID,
		pd.ServiceCompanyID,
		cs.CompanyCode [ServiceCompanyCode],
		cs.CompanyName [ServiceCompanyName],
		est.ExpCompanyID,
		ce.CompanyCode [ExpCompanyCode],
		ce.CompanyName [ExpCompanyName],
		et.ExpeditionType, 
		et.[Description] [ExpeditionTypeDesc],
		e.ExpCode,
		st.ServiceTaskCode,
		st.[Description] ServiceTaskDesc,
		pd.PaperMediaID,
		pd.StationMediaID,
		CAST(pd.HasColorPages as int) HasColorPages,
		et.[Priority] [ExpeditionPriority],
		e.[Priority] [ExpCodePriority],
		SUM(CASE WHEN pMedia.MaterialID is NULL THEN 0 ELSE 1 END) PaperCount,
		SUM(CASE WHEN sMedia.MaterialID is NULL THEN 0 ELSE 1 END) StationCount
	FROM RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	INNER JOIN
		RD_PLEX_TYPE pt WITH(NOLOCK)
	ON pt.PlexType = pd.PlexType
	INNER JOIN
		@RunIDList r
	ON r.ID = pd.RunID
	INNER JOIN
		RD_COMPANY cs WITH(NOLOCK)
	ON cs.CompanyID = pd.ServiceCompanyID
	INNER JOIN
		RD_EXPCODE e WITH(NOLOCK)
	ON pd.ExpCode = e.ExpCode
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	ON est.ExpCode = e.ExpCode
	INNER JOIN
		RD_COMPANY ce WITH(NOLOCK)
	ON ce.CompanyID = est.ExpCompanyID
	INNER JOIN
		RD_SERVICE_TASK st WITH(NOLOCK)
	ON st.ServiceTaskID = est.ServiceTaskID
	INNER JOIN
		RD_EXPEDITION_TYPE et WITH(NOLOCK)
	ON pd.ExpeditionType = et.ExpeditionType
	INNER JOIN
		RD_MATERIAL m WITH(NOLOCK)
	ON m.MaterialID = pd.EnvMaterialID
	INNER JOIN
		RD_FULLFILL_MATERIALCODE mc WITH(NOLOCK)
	ON mc.FullFillMaterialCode = m.FullFillMaterialCode
	LEFT OUTER JOIN
		RT_MEDIA_CONFIG pMedia WITH(NOLOCK)
	ON pMedia.MediaID = pd.PaperMediaID
	LEFT OUTER JOIN
		RT_MEDIA_CONFIG sMedia WITH(NOLOCK)
	ON sMedia.MediaID = pd.StationMediaID
	WHERE pd.ServiceCompanyID = @ServiceCompanyID
	GROUP BY pd.RunID, pd.ServiceCompanyID, cs.CompanyCode, cs.CompanyName, est.ExpCompanyID, ce.CompanyCode, ce.CompanyName, et.[Priority], e.[Priority], 
		et.ExpeditionType, et.[Description], e.ExpCode, pd.PaperMediaID, pd.StationMediaID, pd.HasColorPages, m.FullFillMaterialCode, mc.FullFillCapacity,
		m.MaterialRef, st.ServiceTaskCode, st.[Description], pt.PlexCode, pt.PlexType, m.MaterialID
	ORDER BY et.[Priority] DESC, e.[Priority] DESC, 
		pd.RunID, pd.ServiceCompanyID, PaperCount ASC, StationCount ASC
	
	SET NOCOUNT OFF
GO