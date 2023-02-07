IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_PRINT_SUBSET_REPORT]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_PRINT_SUBSET_REPORT] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_PRINT_SUBSET_REPORT] 
	@ServiceCompanyID int,
	@RunIDList IDList READONLY
--WITH ENCRYPTION
AS
	SET NOCOUNT ON
	DECLARE @SQLString nvarchar(2000)

	SELECT DISTINCT est.ExpCompanyID,
		ce.CompanyCode [ExpCompanyCode],
		ce.CompanyName [ExpCompanyName],
		et.ExpeditionType, 
		et.[Description] [ExpeditionTypeDesc],
		e.ExpCode,
		st.ServiceTaskID,
		st.ServiceTaskCode,
		st.[Description] ServiceTaskDesc,
		pt.PlexType,
		pt.PlexCode,
		pd.PaperMediaID,
		CAST('' as varchar(1000)) PaperMaterialList,
		pd.StationMediaID,
		CAST('' as varchar(1000)) StationMaterialList,
		pd.HasColorPages,
		et.[Priority] [ExpeditionPriority],
		e.[Priority] [ExpCodePriority]
	INTO #PRINT_SUBSET_REPORT
	FROM RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	INNER JOIN
		RD_PLEX_TYPE pt WITH(NOLOCK)
	ON pt.PlexType = pd.PlexType
	INNER JOIN
		@RunIDList r
	ON r.ID = pd.RunID
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
	LEFT OUTER JOIN
		RT_MEDIA_CONFIG pMedia WITH(NOLOCK)
	ON pMedia.MediaID = pd.PaperMediaID
	LEFT OUTER JOIN
		RT_MEDIA_CONFIG sMedia WITH(NOLOCK)
	ON sMedia.MediaID = pd.StationMediaID
	WHERE pd.ServiceCompanyID = @ServiceCompanyID

	DECLARE @MediaID int,
			@MaterialPosition int,
			@MaterialRef varchar(20),
			@MaterialList varchar(1000),
			@OldMediaID int

	-- PaperMedia
	SELECT @MaterialList = '', @OldMediaID = 0
	DECLARE tCursor CURSOR LOCAL
	FOR SELECT DISTINCT mc.MediaID, m.MaterialRef, mc.MaterialPosition
	FROM
		RD_MATERIAL m WITH(NOLOCK)
	INNER JOIN
		RT_MEDIA_CONFIG mc WITH(NOLOCK)
	ON	mc.MaterialID = m.MaterialID
	INNER JOIN
		#PRINT_SUBSET_REPORT p WITH(NOLOCK)
	ON p.PaperMediaID = mc.MediaID
	ORDER BY mc.MediaID, mc.MaterialPosition ASC

	OPEN tCursor
	FETCH NEXT FROM tCursor INTO @MediaID, @MaterialRef, @MaterialPosition
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF(@OldMediaID > 0)
		BEGIN
			IF (@OldMediaID <> @MediaID)
			BEGIN
				UPDATE #PRINT_SUBSET_REPORT
				SET PaperMaterialList = @MaterialList
				WHERE PaperMediaID = @OldMediaID

				SET @MaterialList = ''
			END
			ELSE
			BEGIN
				SELECT @MaterialList = @MaterialList + '|' 
			END
		END
		
		SELECT @OldMediaID = @MediaID, @MaterialList = @MaterialList + @MaterialRef
		FETCH NEXT FROM tCursor INTO @MediaID, @MaterialRef, @MaterialPosition
	END
	CLOSE tCursor
	DEALLOCATE tCursor
	IF(@OldMediaID > 0)
	BEGIN
		UPDATE #PRINT_SUBSET_REPORT
		SET PaperMaterialList = @MaterialList
		WHERE PaperMediaID = @MediaID
	END

	-- StationMedia
	SELECT @MaterialList = '', @OldMediaID = 0
	DECLARE tCursor CURSOR LOCAL
	FOR SELECT DISTINCT mc.MediaID, m.MaterialRef, mc.MaterialPosition
	FROM
		RD_MATERIAL m WITH(NOLOCK)
	INNER JOIN
		RT_MEDIA_CONFIG mc WITH(NOLOCK)
	ON	mc.MaterialID = m.MaterialID
	INNER JOIN
		#PRINT_SUBSET_REPORT p WITH(NOLOCK)
	ON p.StationMediaID = mc.MediaID
	ORDER BY mc.MediaID, mc.MaterialPosition ASC

	OPEN tCursor
	FETCH NEXT FROM tCursor INTO @MediaID, @MaterialRef, @MaterialPosition
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF(@OldMediaID > 0)
		BEGIN
			IF (@OldMediaID <> @MediaID)
			BEGIN
				UPDATE #PRINT_SUBSET_REPORT
				SET StationMaterialList = @MaterialList
				WHERE StationMediaID = @OldMediaID

				SET @MaterialList = ''
			END
			ELSE
			BEGIN
				SELECT @MaterialList = @MaterialList + ' | ' 
			END
		END
		
		SELECT @OldMediaID = @MediaID, @MaterialList = @MaterialList + @MaterialRef
		FETCH NEXT FROM tCursor INTO @MediaID, @MaterialRef, @MaterialPosition
	END
	CLOSE tCursor
	DEALLOCATE tCursor
	IF(@OldMediaID > 0)
	BEGIN
		UPDATE #PRINT_SUBSET_REPORT
		SET StationMaterialList = @MaterialList
		WHERE StationMediaID = @MediaID
	END
	
	SELECT ExpCompanyID,
		[ExpCompanyCode],
		[ExpCompanyName],
		ExpeditionType, 
		[ExpeditionTypeDesc],
		ExpCode,
		ServiceTaskID,
		ServiceTaskCode,
		ServiceTaskDesc,
		PlexType,
		PlexCode,
		PaperMediaID,
		PaperMaterialList,
		StationMediaID,
		StationMaterialList,
		HasColorPages,
		[ExpeditionPriority],
		[ExpCodePriority]
	FROM #PRINT_SUBSET_REPORT
	ORDER BY [ExpeditionPriority] DESC, [ExpCodePriority] DESC
	SET NOCOUNT OFF
GO