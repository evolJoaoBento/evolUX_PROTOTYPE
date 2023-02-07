IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_PRODUCTION_REPORT_FILTER]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_PRODUCTION_REPORT_FILTER] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_PRODUCTION_REPORT_FILTER] 
	@RunIDList IDList READONLY,
	@ServiceCompanyID int,
	@PaperMediaID int,
	@StationMediaID int,
	@ExpeditionType int = NULL,
	@ExpCompanyID int = NULL,
	@ServiceTaskID int NULL,
	@HasColorPages bit = NULL,
	@PlexType int = NULL,
	@FilterOnlyPrint bit = 0
--WITH ENCRYPTION
AS
	SET NOCOUNT ON

	IF OBJECT_ID('tempdb..##RP_UX_PROD_REPORT') IS NOT NULL
	DROP TABLE [dbo].[##RP_UX_PROD_REPORT]

	DECLARE @SQLString nvarchar(2000)
	
	DECLARE
		@PaperCount int,
		@StationCount int,
		@PaperTypeID int

	SELECT @PaperTypeID = MaterialTypeID
	FROM RT_MEDIA WITH(NOLOCK)
	WHERE MediaID = @PaperMediaID
	
	SELECT @PaperCount = COUNT(DISTINCT MaterialID)
	FROM RT_MEDIA_CONFIG WITH(NOLOCK)
	WHERE MediaID = @PaperMediaID

	SELECT @StationCount = COUNT(*)
	FROM RT_MEDIA_CONFIG WITH(NOLOCK)
	WHERE MediaID = @StationMediaID

	DECLARE @ToPrintStateID int,
			@Send2PrintStateID int,
			@ShortFileNameID int,
			@RecoverStateID int

	SELECT @RecoverStateID = RunStateID 
	FROM RD_RUN_STATE WITH(NOLOCK)
	WHERE RunStateName = 'RECOVER'

	SELECT @ToPrintStateID = RunStateID
	FROM RD_RUN_STATE WITH(NOLOCK)
	WHERE RunStateName = 'TOPRINT'

	SELECT @Send2PrintStateID = RunStateID
	FROM RD_RUN_STATE WITH(NOLOCK)
	WHERE RunStateName = 'SEND2PRINTER'

	SELECT @ShortFileNameID = RunStateID
	FROM RD_RUN_STATE WITH(NOLOCK)
	WHERE RunStateName = 'SHORTFILENAME'
	
	DECLARE @PrintColorDPLEX varchar(260),
            @PrintColorSPLEX varchar(260),
            @PrintBlackDPLEX varchar(260),
            @PrintBlackSPLEX varchar(260),
            @ServiceTypeCode varchar(15)
	
	SELECT @PrintColorDPLEX = '',
            @PrintColorSPLEX = '',
            @PrintBlackDPLEX = '',
            @PrintBlackSPLEX = ''
	            
	DECLARE pCursor CURSOR LOCAL FOR
	SELECT ServiceTypeCode
	FROM RD_SERVICE_TYPE WITH(NOLOCK)
	WHERE ServiceTypeCode like 'PRINT%'
	
	OPEN pCursor
	FETCH NEXT FROM pCursor INTO @ServiceTypeCode
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@ServiceTypeCode like 'PRINTCOLOR%')
        BEGIN
			IF (@ServiceTypeCode like '%SPLEX')
			BEGIN
				SELECT @PrintColorSPLEX = @PrintColorSPLEX + '<PRINTSERVICETYPE>' + @ServiceTypeCode +'</PRINTSERVICETYPE>'
			END
			ELSE
			BEGIN
				SELECT @PrintColorDPLEX = @PrintColorDPLEX + '<PRINTSERVICETYPE>' + @ServiceTypeCode +'</PRINTSERVICETYPE>'
			END
		END
		ELSE
		BEGIN
			IF (@ServiceTypeCode like '%SPLEX')
			BEGIN
				SELECT @PrintBlackSPLEX = @PrintBlackSPLEX + '<PRINTSERVICETYPE>' + @ServiceTypeCode +'</PRINTSERVICETYPE>'
			END
			ELSE
			BEGIN
				SELECT @PrintBlackDPLEX = @PrintBlackDPLEX + '<PRINTSERVICETYPE>' + @ServiceTypeCode +'</PRINTSERVICETYPE>'
			END
		END
		FETCH NEXT FROM pCursor INTO @ServiceTypeCode
	END
	
	CLOSE pCursor
	DEALLOCATE pCursor
	
	IF (@PrintBlackSPLEX = '')
		SET @PrintBlackSPLEX = @PrintBlackDPLEX
	
	IF (@PrintColorDPLEX = '')
		SET @PrintColorDPLEX = @PrintBlackDPLEX
	
	IF (@PrintColorSPLEX = '')
		SET @PrintColorSPLEX = @PrintColorDPLEX

	DECLARE @MaterialRef varchar(20),
			@MaterialPosition int,
			@MaterialID int,
			@PaperMaterialColumnName varchar(100),
			@StationMaterialColumnName varchar(100)

	SELECT	@PaperMaterialColumnName = '[Paper|#@MaterialRef@#]',
			@StationMaterialColumnName = '[Station|[#@MaterialPosition@#]] #@MaterialRef@#]'

	SELECT @SQLString = 'CREATE TABLE ##RP_UX_PROD_REPORT(
		[ExpeditionPriority] int NULL,
		[ExpCodePriority] int NULL,
		[RunID] int NULL,
		[FileID] int NULL,
		[FilePath] varchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[FileName] varchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ShortFileName] varchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[FilePrinterSpecs] varchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[FilePrintedFlag] bit NULL,
		[RegistDetailFileName] varchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[RegistDetailShortFileName] varchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[RegistDetailFilePrinterSpecs] varchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[RegistDetailFilePrintedFlag] bit NULL,
		[RegistDetailFileRecNumber] int NULL,
		[ServiceTaskCode] varchar(4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[PrinterOperator] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Printer] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[PlexCode] varchar(5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[PlexType] tinyint NULL,
		[StartSeqNum] int NULL,
		[EndSeqNum] int NULL,
		[EnvMaterialID] int NULL,
		[EnvMaterialRef] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[FullFillMaterialCode] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[FullFillCapacity] smallint NULL,
		[ExpLevel] int NULL,
		[ExpCompanyCode] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ExpCenterCode] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ExpZone] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ExpType] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ExpeditionLevel] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[TotalPrint] int NULL,
		[TotalPostObjs] int NULL'

	-- PaperMedia
	DECLARE tCursor CURSOR LOCAL
	FOR SELECT DISTINCT m.MaterialRef, m.MaterialID, mc.MaterialPosition
	FROM
		RD_MATERIAL m WITH(NOLOCK)
	INNER JOIN
		RT_MEDIA_CONFIG mc WITH(NOLOCK)
	ON	mc.MaterialID = m.MaterialID
	WHERE mc.MediaID = @PaperMediaID
	ORDER BY mc.MaterialPosition ASC

	OPEN tCursor
	FETCH NEXT FROM tCursor INTO @MaterialRef, @MaterialID, @MaterialPosition
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @SQLString = @SQLString + ', ' + REPLACE(@PaperMaterialColumnName,'#@MaterialRef@#',@MaterialRef) + ' int  DEFAULT 0' 
		FETCH NEXT FROM tCursor INTO @MaterialRef, @MaterialID, @MaterialPosition
	END
	CLOSE tCursor
	DEALLOCATE tCursor

	-- StationMedia
	DECLARE tCursor CURSOR LOCAL
	FOR SELECT m.MaterialRef, mc.MaterialPosition, m.MaterialID
	FROM
		RD_MATERIAL m WITH(NOLOCK)
	INNER JOIN
		RT_MEDIA_CONFIG mc WITH(NOLOCK)
	ON	mc.MaterialID = m.MaterialID
	WHERE mc.MediaID = @StationMediaID

	OPEN tCursor
	FETCH NEXT FROM tCursor INTO @MaterialRef, @MaterialPosition, @MaterialID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @SQLString = @SQLString + ', ' + REPLACE(REPLACE(@StationMaterialColumnName,'#@MaterialPosition@#', CAST(@MaterialPosition as varchar)), '#@MaterialRef@#', @MaterialRef) + ' int DEFAULT 0'
		FETCH NEXT FROM tCursor INTO @MaterialRef, @MaterialPosition, @MaterialID
	END
	CLOSE tCursor
	DEALLOCATE tCursor

	SELECT @SQLString = @SQLString + ')'
	EXEC sp_executesql @SQLString
	CREATE UNIQUE INDEX [UX_PROD_REPORT] ON ##RP_UX_PROD_REPORT([RunID],[FileID]) 

	INSERT INTO ##RP_UX_PROD_REPORT([RunID], [FileID], 
		[FilePath],
		[FileName],
		[ShortFileName],
		[FilePrinterSpecs],
		[FilePrintedFlag], 
		[RegistDetailFileName],
		[RegistDetailShortFileName],
		[RegistDetailFilePrinterSpecs],
		[RegistDetailFilePrintedFlag],
		[RegistDetailFileRecNumber],
		[PrinterOperator], [Printer],
		[PlexType],
		[EnvMaterialID],
		[EnvMaterialRef], 
		[FullFillMaterialCode], 
		[FullFillCapacity], 
		[ExpLevel], 
		[ExpeditionPriority], 
		[ExpCodePriority])
	SELECT
		f.RunID,
		f.FileID,
		fl1.OutputPath,
		ISNULL(fl1.OutputName, f.[FileName]),
		fl3.OutputName,
		CASE WHEN pd.HasColorPages = 1 THEN 
			CASE WHEN pd.PlexType = 1 --'SPLEX' 
				THEN @PrintColorSPLEX ELSE @PrintColorDPLEX END
		ELSE
			CASE WHEN pd.PlexType = 1 --'SPLEX' 
				THEN @PrintBlackSPLEX ELSE  @PrintBlackDPLEX END 
		END,
		CASE WHEN fl2.ProcCountNr is NOT NULL THEN 1 ELSE 0 END,
		ISNULL(er.[FileName],''),
		ISNULL(REPLACE(REPLACE(er.[FileName],SUBSTRING(er.[FileName],LEN(er.[FileName])-CHARINDEX('.',REVERSE(er.[FileName]),1)+1,CHARINDEX('.',REVERSE(er.[FileName]),1)),
			SUBSTRING(fl3.OutputName,LEN(fl3.OutputName)-CHARINDEX('.',REVERSE(fl3.OutputName),1)+1,CHARINDEX('.',REVERSE(fl3.OutputName),1))),
			f.[FileName],SUBSTRING(fl3.OutputName,1,LEN(fl3.OutputName)-CHARINDEX('.',REVERSE(fl3.OutputName),1))),''),
		'', 
		CASE WHEN er.PrintedTimeStamp is NOT NULL THEN 1 ELSE 0 END,
		er.RecNumber,
		fl2.OutputName, fl2.OutputPath,
		pd.PlexType,
		m.MaterialID,
		m.MaterialRef, m.FullFillMaterialCode, fm.FullFillCapacity,
		pd.ExpLevel,
		et.[Priority], e.[Priority]
	FROM 
		RT_FILE_REGIST f WITH(NOLOCK)
	INNER JOIN
		@RunIDList r
	ON	r.ID = f.RunID
	INNER JOIN
		RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	ON pd.ProdDetailID = f.ProdDetailID
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	ON est.ExpCode = pd.ExpCode
	INNER JOIN
		RD_EXPCODE e WITH(NOLOCK)
	ON e.ExpCode = est.ExpCode
	INNER JOIN
		RD_EXPEDITION_TYPE et WITH(NOLOCK)
	ON et.[ExpeditionType] = pd.[ExpeditionType]
	LEFT OUTER JOIN
		RT_FILE_LOG fl1 WITH(NOLOCK)
	ON	f.RunID = fl1.RunID AND f.FileID = fl1.FileID
		AND fl1.RunStateID = @ToPrintStateID
		AND fl1.ProcCountNr = (SELECT MAX(ProcCountNr)
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = fl1.RunID AND FileID = fl1.FileID AND fl1.EndTimeStamp is NOT NULL
						AND RunStateID = fl1.RunStateID AND ErrorID = 0)
	LEFT OUTER JOIN
		RT_FILE_LOG fl2 WITH(NOLOCK)
	ON f.RunID = fl2.RunID AND f.FileID = fl2.FileID
		AND fl2.RunStateID = @Send2PrintStateID
		AND fl2.ProcCountNr = (SELECT MAX(ProcCountNr)
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = fl2.RunID AND FileID = fl2.FileID
						AND RunStateID = fl2.RunStateID AND ErrorID = 0)
	LEFT OUTER JOIN
		RT_EXPCOMPANY_REGIST_DETAIL_FILE er WITH(NOLOCK)
	ON	er.RunID = f.RunID AND er.FileID = f.FileID
		AND er.RecNumber = (SELECT TOP 1 RecNumber 
					FROM RT_EXPCOMPANY_REGIST_DETAIL_FILE WITH(NOLOCK)
					WHERE RunID = er.RunID 
						AND FileID = er.FileID 
					ORDER BY CASE WHEN ToPrintTimeStamp is NULL THEN 0 ELSE 1 END ASC,
						CASE WHEN PrintedTimeStamp is NULL THEN 0 ELSE 1 END ASC, 
						RecNumber ASC)
	LEFT OUTER JOIN
		RT_FILE_LOG fl3 WITH(NOLOCK)
	ON f.RunID = fl3.RunID AND f.FileID = fl3.FileID
		AND fl3.RunStateID = @ShortFileNameID
		AND fl3.ProcCountNr = (SELECT MAX(ProcCountNr)
							FROM RT_FILE_LOG WITH(NOLOCK)
							WHERE RunID = fl3.RunID AND FileID = fl3.FileID
								AND RunStateID = fl3.RunStateID AND ErrorID = 0)
	INNER JOIN
		RD_MATERIAL m WITH(NOLOCK)
	ON m.MaterialID = pd.EnvMaterialID
	LEFT OUTER JOIN
		RD_FULLFILL_MATERIALCODE fm WITH(NOLOCK)
	ON fm.FullFillMaterialCode COLLATE SQL_Latin1_General_CP1_CI_AS = m.FullFillMaterialCode COLLATE SQL_Latin1_General_CP1_CI_AS
	WHERE f.ErrorID = 0
		AND pd.ServiceCompanyID = @ServiceCompanyID
		AND pd.PaperMediaID = @PaperMediaID
		AND ISNULL(pd.StationMediaID, 0) = ISNULL(@StationMediaID, 0)
		AND (@ExpeditionType is NULL OR pd.ExpeditionType = @ExpeditionType)
		AND (@ExpCompanyID is NULL OR est.ExpCompanyID = @ExpCompanyID)
		AND (@ServiceTaskID is NULL OR est.ServiceTaskID = @ServiceTaskID)
		AND (@HasColorPages is NULL OR ISNULL(pd.HasColorPages, 0) = @HasColorPages)
		AND (@PlexType is NULL OR pd.PlexType = @PlexType)


	--Remover ficheiros de recuperações já impressos
	DELETE ##RP_UX_PROD_REPORT
	FROM ##RP_UX_PROD_REPORT u
	INNER JOIN
		RT_FILE_RECOVER fr WITH(NOLOCK)
	ON fr.ProdRunID = u.RunID
		AND fr.RecFileID = u.FileID
	WHERE fr.ProdFileID <> fr.RecFileID
		AND EXISTS (SELECT TOP 1 1 
					FROM RT_FILE_LOG WITH(NOLOCK)
						WHERE RunID = fr.ProdRunID
							AND FileID = fr.RecFileID
							AND RunStateID = @Send2PrintStateID
							AND ErrorID = 0)

	-- Update de Dados de ficheiros de recuperações
	UPDATE ##RP_UX_PROD_REPORT
	SET
		[ServiceTaskCode] = fp.ServiceTaskCode, 
		[PlexCode] = fp.PlexCode,
		[StartSeqNum] = fp.StartSeqNum,
		[EndSeqNum] = fp.EndSeqNum,
		[ExpCompanyCode] = fp.ExpCompanyCode, 
		[ExpCenterCode] = fp.ExpCenterCodeDesc, 
		[ExpZone] = fp.ExpeditionZone, 
		[ExpType] = fp.ExpeditionType, 
		[ExpeditionLevel] = fp.ExpeditionLevelDesc,
		[TotalPrint] = fp.TotalPrint, 
		[TotalPostObjs] = fp.TotalPostObjs
	FROM ##RP_UX_PROD_REPORT u
	INNER JOIN
		RT_FILE_RECOVER fp WITH(NOLOCK)
	ON	fp.ProdRunID = u.RunID
		AND fp.RecFileID = u.FileID
	WHERE fp.ProdFileID <> fp.RecFileID

	-- Update de Dados de ficheiros produção originais
	UPDATE ##RP_UX_PROD_REPORT
	SET
		[ServiceTaskCode] = fp.ServiceTaskCode, 
		[PlexCode] = fp.PlexCode,
		[StartSeqNum] = fp.StartSeqNum,
		[EndSeqNum] = fp.EndSeqNum,
		[ExpCompanyCode] = fp.ExpCompanyCode, 
		[ExpCenterCode] = fp.ExpCenterCodeDesc, 
		[ExpZone] = fp.ExpeditionZone, 
		[ExpType] = fp.ExpeditionType, 
		[ExpeditionLevel] = fp.ExpeditionLevelDesc,
		[TotalPrint] = fp.TotalPrint, 
		[TotalPostObjs] = fp.TotalPostObjs
	FROM ##RP_UX_PROD_REPORT u
	INNER JOIN
		RT_FILE_PRODUCTION fp WITH(NOLOCK)
	ON	fp.RunID = u.RunID AND fp.FileID = u.FileID

	--Contagens de materiais
	DECLARE @RunID int,
			@FileID int,
			@MaterialCount int

	--Update de contagens de papeis
	DECLARE tCursor CURSOR LOCAL
	FOR SELECT f.RunID, f.FileID, mc.MaterialPosition, m.MaterialRef, SUM(fm.Quantity)
	FROM ##RP_UX_PROD_REPORT u
	INNER JOIN
		RT_FILE_REGIST f WITH(NOLOCK)
	ON f.RunID = u.RunID AND f.FileID = u.FileID
	INNER JOIN
		RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	ON pd.ProdDetailID = f.ProdDetailID
	INNER JOIN
		RT_MEDIA_CONFIG mc WITH(NOLOCK)
	ON mc.MediaID = pd.PaperMediaID
	INNER JOIN
		RT_FILE_MATERIAL fm WITH(NOLOCK)
	ON fm.RunID = f.RunID AND f.FileID = fm.FileID
		AND fm.MaterialID = mc.MaterialID
	INNER JOIN
		RD_MATERIAL m WITH(NOLOCK)
	ON m.MaterialID = mc.MaterialID
	GROUP BY f.RunID, f.FileID, mc.MaterialPosition, m.MaterialRef

	OPEN tCursor
	FETCH NEXT FROM tCursor INTO @RunID, @FileID, @MaterialPosition, @MaterialRef, @MaterialCount
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @SQLString = 'UPDATE ##RP_UX_PROD_REPORT SET ' + REPLACE(@PaperMaterialColumnName,'#@MaterialRef@#',@MaterialRef) + 
			' = ' + CAST(@MaterialCount as varchar) + '
			WHERE RunID = ' + CAST(@RunID as varchar) + ' AND FileID = ' + CAST(@FileID as varchar)
		EXEC sp_executesql @SQLString
		FETCH NEXT FROM tCursor INTO @RunID, @FileID, @MaterialPosition, @MaterialRef, @MaterialCount
	END

	CLOSE tCursor
	DEALLOCATE tCursor

	--Update de contagens de Adicionais
	DECLARE tCursor CURSOR LOCAL
	FOR SELECT f.RunID, f.FileID, mc.MaterialPosition, m.MaterialRef, SUM(fm.Quantity)
	FROM ##RP_UX_PROD_REPORT u
	INNER JOIN
		RT_FILE_REGIST f WITH(NOLOCK)
	ON f.RunID = u.RunID AND f.FileID = u.FileID
	INNER JOIN
		RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	ON pd.ProdDetailID = f.ProdDetailID
	INNER JOIN
		RT_MEDIA_CONFIG mc WITH(NOLOCK)
	ON mc.MediaID = pd.StationMediaID
	INNER JOIN
		RT_FILE_MATERIAL fm WITH(NOLOCK)
	ON fm.RunID = f.RunID AND f.FileID = fm.FileID
		AND fm.MaterialID = mc.MaterialID
	INNER JOIN
		RD_MATERIAL m WITH(NOLOCK)
	ON m.MaterialID = mc.MaterialID
	GROUP BY f.RunID, f.FileID, mc.MaterialPosition, m.MaterialRef

	OPEN tCursor
	FETCH NEXT FROM tCursor INTO @RunID, @FileID, @MaterialPosition, @MaterialRef, @MaterialCount
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @SQLString = 'UPDATE ##RP_UX_PROD_REPORT SET ' + REPLACE(REPLACE(@StationMaterialColumnName,'#@MaterialPosition@#', CAST(@MaterialPosition as varchar)), '#@MaterialRef@#', @MaterialRef) + 
			' = ' + CAST(@MaterialCount as varchar) + '
			WHERE RunID = ' + CAST(@RunID as varchar) + ' AND FileID = ' + CAST(@FileID as varchar)
		EXEC sp_executesql @SQLString
		FETCH NEXT FROM tCursor INTO @RunID, @FileID, @MaterialPosition, @MaterialRef, @MaterialCount
	END

	CLOSE tCursor
	DEALLOCATE tCursor

	SELECT u.*
	FROM ##RP_UX_PROD_REPORT u
	WHERE (@FilterOnlyPrint = 0 OR CAST(u.[FilePrintedFlag] as int) = 0 OR ISNULL(CAST(u.[RegistDetailFilePrintedFlag] as int),1) = 0)
	ORDER BY u.[ExpeditionPriority] DESC, u.[ExpCodePriority] DESC, (CAST(u.[FilePrintedFlag] as int) + ISNULL(CAST(u.[RegistDetailFilePrintedFlag] as int),1)) ASC, 
		u.PlexType ASC, u.FullFillCapacity DESC, u.EnvMaterialID ASC, u.ExpLevel DESC

	DROP TABLE ##RP_UX_PROD_REPORT
	SET NOCOUNT OFF
GO