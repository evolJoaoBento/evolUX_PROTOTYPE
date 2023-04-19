--Release Version AND Key
INSERT INTO [dbo].[evolDP_VERSION](ReleaseID, Major, Minor, Revision, Patch, [Description], [Key])
SELECT ReleaseID + 1,1,6,4,15,'Restore and Fix: PostObjOrderMode to specify order of documents inside PostObj + RT_CHECK_ALL_INTEGRATION_FILES_DONE + HRT_PREHISTORY_STATE',[Key]
FROM evolDP_VERSION
WHERE ReleaseID = (SELECT MAX(ReleaseID) FROM evolDP_VERSION)
GO
IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'PostObjOrderMode' AND OBJECT_ID = OBJECT_ID(N'RD_BUSINESS'))
BEGIN
	ALTER TABLE dbo.RD_BUSINESS
	ADD PostObjOrderMode tinyint NULL

END 
GO
ALTER PROCEDURE [dbo].[RT_MAKE_NEW_FORK_FILE]
	@ProdDetailID int,
	@PostObjLotID int,
	@RunStateID int,
	@MaxSheets int = 0,
	@BannerCount int = 4 -- Nº de folhas de Banner utilizadas por Default
--WITH ENCRYPTION
AS
	SET NOCOUNT ON

	DECLARE @ProcCountNr int,
		@SheetsCount int,
		@PostObjRunID int,
		@PostObjSheets int,
		@PostObjFileSeq int,
		@PrintsCount int ,
		@DocsCount int ,
		@StartPostObjFileSeq int,
		@EndPostObjFileSeq int,
		@FileID int,
		@FileName varchar(256),
		@FileSetNr int,
		@PostObjsCount int,
		@ServiceCompanyCode varchar(6),
		@PostObjLotIDFileCount int,
		@BusinessCode varchar(10),
		@RunDate int,
		@RunSequence smallint,
		@SystemName varchar(10),
		@ExpCode varchar(10),
		@ExpCenterCode varchar(5),
		@ExpCenterCodeDesc varchar(50),
		@ExpLevel int,
		@ExpCompanyLevel int,
		@ExpCompanyLevelWeight float,
		@ExpeditionType int,
		@ServiceCompanyID int,
		@FilePlexCode char(5),
		@ServiceTaskCode varchar(4),
		@ServiceTaskDesc varchar(256),
		@ExpeditionTypeDesc varchar(50), 
		@ExpeditionZoneDesc varchar(50),
		@RegistMode bit,
		@BarcodeRegistMode bit, --LOP-20160918
		@SeparationMode bit,
		@BarCode varchar(256),
		@ExpeditionID int,
		@ExpeditionIDAttribute varchar(50),
		@CompanyRegistCode int,
		@ExpeditionClientNr int,
		@MaxStationsFlag bit,
		@FileType varchar(10),
		@ExpeditionLevelDesc varchar(50),
		@StationExceededDesc varchar(50),
		@PostObjRunIDLevel bit, 
		@Priority int,
		-- Inserts - LOP - 2012
		@FileCode varchar(2),
		--V1.6.4.15
		@PostObjOrderMode tinyint

	DECLARE @REMFERunStateID int,
		@EMFERunStateID int, 
		@SHORTFileNameID int,
		@ShortFileName varchar(256)

	SELECT @SHORTFileNameID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'SHORTFILENAME'

	SELECT @REMFERunStateID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'REGISTEMFE'

	SELECT @EMFERunStateID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'DOC1EMFE'

	SELECT @FileSetNr = COUNT(*)
	FROM RT_FILE_REGIST f
	WHERE f.ProdDetailID = @ProdDetailID

	SELECT @PostObjLotIDFileCount = COUNT(*)
	FROM RT_FILE_REGIST f
	WHERE f.ParentSetNumber = @PostObjLotID
		AND f.RunID = (SELECT RunID FROM RT_PRODUCTION_DETAIL WHERE ProdDetailID = @ProdDetailID)

	SELECT @PostObjRunID = pd.RunID, 
		@ShortFileName =  b.BusinessCode
				+ CAST(r.RunDate as varchar)
				+'_'
				+ CAST(pd.RunID as varchar)
				+'_',
		@FileName = b.BusinessCode 
				+ CASE WHEN SUBSTRING(b.BusinessCode,1,1) = 'C' 
					THEN
						CASE WHEN LEN(CAST(r.RunSequence as varchar))<2 THEN '0' + CAST(r.RunSequence as varchar) 
							ELSE CAST(r.RunSequence as varchar) END
					ELSE 
						CASE WHEN b.BusinessCode = 'S' 
						THEN
							ev.[EnvMediaName]
						ELSE 
							'' 
						END 
					END
				+ CAST(r.RunDate as varchar) 
				+ c.CompanyCode
				+ CASE WHEN (LEN(CAST(pd.ExpeditionType as varchar)) < 2) THEN
					REPLICATE ('0' , 2-LEN(CAST(pd.ExpeditionType as varchar)) )  + CAST(pd.ExpeditionType as varchar)
				ELSE
					CAST(pd.ExpeditionType as varchar)
				END
				+ pd.ExpCode
				+ pd.ExpCenterCode
				+ CAST(pd.ExpCompanyLevel as varchar)
				+ CASE WHEN (LEN(CAST(pd.ExpLevel as varchar)) < 2) THEN
					REPLICATE ('0' , 2-LEN(CAST(pd.ExpLevel as varchar)) )  + CAST(pd.ExpLevel as varchar)
				ELSE
					CAST(pd.ExpLevel as varchar)
				END
				+ '_' 
				+ CASE WHEN (LEN(CAST(@FileSetNr as varchar)) < 2) THEN
					REPLICATE ('0' , 2-LEN(CAST(@FileSetNr as varchar)) )  + CAST(@FileSetNr as varchar)
				ELSE
					CAST(@FileSetNr as varchar)
				END
				+'_'
				+ CAST(pd.RunID as varchar)
				+'_',
				@ServiceCompanyCode = c.CompanyCode,
		@BusinessCode = b.BusinessCode,
		@RunDate  = r.RunDate,
		@RunSequence = r.RunSequence,
		@SystemName = st.SystemName,
		@ExpCode = pd.ExpCode,
		@ExpCenterCode = pd.ExpCenterCode,
		@ExpCenterCodeDesc = ISNULL(eee.Description1,pd.ExpCenterCode),
		@ExpLevel = pd.ExpLevel,
		@ExpCompanyLevel = pd.ExpCompanyLevel,
		@ExpeditionType = pd.ExpeditionType,
		@ServiceCompanyID = pd.ServiceCompanyID,
		@FilePlexCode = UPPER(pt.PlexCode),
		@ServiceTaskCode = s.ServiceTaskCode,
		@ExpeditionTypeDesc = et.[Description] + ' [' + ec.ExpColumnB + ']',
		@ExpeditionZoneDesc = ez.[Description] + ' [' + ec.ExpColumnA + ']',
		@ServiceTaskDesc = s.[Description],
		@RegistMode = ISNULL(ect.RegistMode,0),
		@BarcodeRegistMode = ISNULL(ect.BarcodeRegistMode, ISNULL(ect.RegistMode,0)),
		@SeparationMode = ISNULL(ect.SeparationMode,0),
		@BarCode = scec.Barcode,
		@ExpCompanyLevelWeight = ec.MaxWeight,
		--V1.0.3.19 ou V1.4.0.1
		@ExpeditionLevelDesc = CASE WHEN ISNULL(ect.SeparationMode,0) = 1 
				THEN CAST(ROUND(ec.MaxWeight,0) as varchar) + ' g'  
				ELSE 'N/A' END,
		------------------------
		@StationExceededDesc =  s.StationExceededDesc,
		-- Inserts - LOP - 2012
		@FileCode = SUBSTRING(b.BusinessCode,1,1),
		--V1.6.4.15
		@PostObjOrderMode = b.PostObjOrderMode
	FROM RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	INNER JOIN
		RT_RUN r WITH(NOLOCK)
	ON 	r.RunID = pd.RunID
	INNER JOIN
		RD_BUSINESS b WITH(NOLOCK)
	ON	b.BusinessID = r.BusinessID
	INNER JOIN
		RD_RUN_TYPE rt WITH(NOLOCK)
	ON	rt.RunTypeID = r.RunTypeID
	INNER JOIN
		RD_SYSTEM st WITH(NOLOCK)
	ON	st.SystemID = rt.SystemID
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK)
	ON	c.CompanyID = pd.ServiceCompanyID
	INNER JOIN
		RD_PLEX_TYPE pt WITH(NOLOCK)
	ON	pt.PlexType = pd.PlexType
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	ON	est.ExpCode = pd.ExpCode
	INNER JOIN
		RD_SERVICE_TASK s WITH(NOLOCK)
	ON	s.ServiceTaskID = est.ServiceTaskID
	INNER JOIN
		RD_EXPEDITION_TYPE et WITH(NOLOCK)
	ON	et.ExpeditionType = pd.ExpeditionType
	INNER JOIN
		RD_EXPEDITION_EXPCENTER_EXPZONE eee WITH(NOLOCK)
	ON	eee.ExpCode = pd.ExpCode
		AND eee.ExpCenterCode = pd.ExpCenterCode
	INNER JOIN
		RD_EXPEDITION_ZONE ez WITH(NOLOCK)
	ON	eee.ExpeditionZone = ez.ExpeditionZone
	INNER JOIN
		RD_SERVICE_COMPANY_EXPCODE_CONFIG scec WITH(NOLOCK)
	ON	scec.ServiceCompanyID = pd.ServiceCompanyID
		AND scec.ExpCode = pd.ExpCode
		AND scec.ExpCenterCode = pd.ExpCenterCode
		AND scec.ExpLevel = pd.ExpLevel
	INNER JOIN
		RD_EXPCOMPANY_CONFIG ec WITH(NOLOCK)
	ON	ec.ExpCompanyID = est.ExpCompanyID
		AND ec.ExpeditionZone = eee.ExpeditionZone
		AND ec.ExpeditionType = pd.ExpeditionType
		AND ec.ExpCompanyLevel = pd.ExpCompanyLevel
		AND ec.StartDate = (SELECT MAX(StartDate)
					FROM RD_EXPCOMPANY_CONFIG WITH(NOLOCK)
					WHERE 	ExpCompanyID = ec.ExpCompanyID
						AND ExpeditionZone = ec.ExpeditionZone
						AND ExpeditionType = ec.ExpeditionType
						AND ExpCompanyLevel = ec.ExpCompanyLevel
						AND StartDate <= r.RunDate)
	INNER JOIN
		RD_EXPCOMPANY_TYPE ect WITH(NOLOCK)
	ON	ect.ExpeditionType = pd.ExpeditionType
		AND ect.ExpCompanyID = est.ExpCompanyID
	INNER JOIN
		RD_ENVELOPE_MEDIA ev WITH(NOLOCK)
	ON	ev.EnvMediaID = pd.EnvMediaID
	WHERE pd.ProdDetailID = @ProdDetailID
	
	SET @MaxStationsFlag = 0

	SELECT @MaxStationsFlag = CASE WHEN s.MaterialPosition < (SELECT COUNT(*) DistinctMaterial
																FROM RT_MEDIA_CONFIG mc WITH(NOLOCK)
																INNER JOIN
																	RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
																ON	mc.MediaID = pd.StationMediaID
																WHERE pd.ProdDetailID = @ProdDetailID) 
								THEN 1 ELSE 0 END
	FROM RD_SERVICE_COMPANY_RESTRICTION s WITH(NOLOCK)
	WHERE s.ServiceCompanyID = @ServiceCompanyID
		AND s.MaterialTypeID = (SELECT MaterialTypeID FROM RD_MATERIAL_TYPE WITH(NOLOCK) WHERE MaterialTypeCode = 'Station')

	-- @BarCode =  ExpBarCode(00) + PostObjIDBarCode(00) + FillIDBarCode(00) + FullFillBarcode(00)
	-- Querendo acrescentar mais códigos, acrescentar no fim.
	IF (LEN(@BarCode) >= 8)
	BEGIN
		-- Transforma ExpBarCode
		-- Se ExpBarCode = 03 e @RegistMode = 1 então ExpBarCode = 13
		-- Se ExpBarCode = 00 e @RegistMode = 1 então ExpBarCode = 10
		-- Se ExpBarCode = 11 e @RegistMode = 0 então ExpBarCode = 01
		SELECT @BarCode = CAST(@BarcodeRegistMode as varchar) + SUBSTRING(@BarCode,2,LEN(@BarCode)-1)
	END

	DECLARE @EntityValue1 varchar(256), 
			@EntityValue2 varchar(256), 
			@EntityValue3 varchar(256), 
			@EntityValue4 varchar(256), 
			@EntityValue5 varchar(256), 
			@EntityValue6 varchar(256), 
			@EntityValue7 varchar(256),
			@EntityValue8 varchar(256), 
			@EntityValue9 varchar(256), 
			@EntityValue10 varchar(256)

	DECLARE @CompanyID int,
		--V1.0.3.19 ou V1.4.0.1
		@ContractID int,
		--------
		@ExpCompanyID int,
		@ExpCompanyCode varchar(6),
		@ServiceTaskID int,
		@InternalExpeditionMode tinyint,
		@InternalCodeStart tinyint,
		@InternalCodeLen tinyint


	SELECT @CompanyID = b.CompanyID, @InternalExpeditionMode = ISNULL(b.InternalExpeditionMode,2),
		@InternalCodeStart = ISNULL(InternalCodeStart,10), 
		@InternalCodeLen = ISNULL(InternalCodeLen,10)
	FROM RT_RUN r WITH(NOLOCK)
	INNER JOIN
		RD_BUSINESS b WITH(NOLOCK)
	ON	r.BusinessID = b.BusinessID
	WHERE r.RunID = @PostObjRunID

	SELECT @ExpCompanyID = est.ExpCompanyID, @ExpCompanyCode = c.CompanyCode,
		--V1.0.3.19 ou V1.4.0.1
		@ContractID = ec.ContractID,
		--------
		@ServiceTaskID = est.ServiceTaskID,
		@CompanyRegistCode = ei.CompanyRegistCode,
		@ExpeditionClientNr = ec.ClientNr
	FROM RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	INNER JOIN
		RT_PRODUCTION_DETAIL p WITH(NOLOCK)
	ON	p.ProdDetailID = @ProdDetailID
		AND p.ExpCode = est.ExpCode
	INNER JOIN
		RD_COMPANY c
	ON	c.CompanyID = est.ExpCompanyID
	LEFT OUTER JOIN
		RD_ENVMEDIA_EXPCOMPANY_CONTRACT eec
	ON	p.EnvMediaID = eec.EnvMediaID
		AND eec.ExpCompanyID = est.ExpCompanyID
	LEFT OUTER JOIN
		RD_EXPCOMPANY_CONTRACT ec
	ON	ec.ExpCompanyID = est.ExpCompanyID
		AND ec.ContractID = eec.ContractID
	LEFT OUTER JOIN
		RD_EXPEDITION_ID ei
	ON 	ei.ExpCompanyID = est.ExpCompanyID

	BEGIN TRANSACTION

	INSERT INTO RT_FILE_REGIST(RunID, FileID, RunStateID, FileRunNr, [FileName], FilePath, LastConsistencyPoint, ParentFileID, ParentSetNumber, ProdDetailID, ErrorID, SuportType, FileCode, FileSequence, FileSetNr, FileRecCount)
	SELECT @PostObjRunID, ISNULL(MAX(FileID),0)+1, @RunStateID, 0 , @FileName, NULL, NULL, NULL, @PostObjLotID, @ProdDetailID, 0, 1, NULL, NULL, @FileSetNr, NULL
	FROM RT_FILE_REGIST
	WHERE RunID = @PostObjRunID

	SELECT @FileID = FileID, @FileName = [FileName] + CAST(FileID as varchar),@ShortFileName = @ShortFileName + CAST(FileID as varchar)
	FROM RT_FILE_REGIST
	WHERE [FileName] = @FileName AND FileRunNr = 0

	UPDATE RT_FILE_REGIST
	SET [FileName] = @FileName
	WHERE RunID = @PostObjRunID AND FileID = @FileID

	SELECT @ProcCountNr = ISNULL(MAX(ProcCountNr),0) + 1
	FROM RT_FILE_LOG
	WHERE RunID = @PostObjRunID AND FileID = @FileID
		AND RunStateID = @RunStateID
		
	INSERT INTO dbo.RT_FILE_LOG(RunID, FileID, RunStateID, ProcCountNr,StartTimeStamp)
	SELECT @PostObjRunID, @FileID, @RunStateID, @ProcCountNr, CURRENT_TIMESTAMP

	IF (NOT EXISTS(SELECT RunStateID FROM RT_FILE_LOG WHERE RunID = @PostObjRunID AND FileID = @FileID AND RunStateID = @SHORTFileNameID))
	BEGIN
		INSERT INTO dbo.RT_FILE_LOG(RunID, FileID, RunStateID, ProcCountNr, StartTimeStamp, EndTimeStamp, OutputName)
		SELECT @PostObjRunID, @FileID, @SHORTFileNameID, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @ShortFileName
		FROM RD_RUN_STATE
		WHERE RunStateID = @SHORTFileNameID --Para garantir que se o RunState nao estiver registado nao ha erro
	END
	ELSE
	BEGIN
		UPDATE RT_FILE_LOG
		SET StartTimeStamp = CURRENT_TIMESTAMP, EndTimeStamp = CURRENT_TIMESTAMP,
			OutputName = @ShortFileName
		WHERE RunID = @PostObjRunID AND FileID = @FileID AND RunStateID = @SHORTFileNameID
	END

	COMMIT TRANSACTION

	CREATE TABLE #FORK_REPORT
	(
		[TypeOrder1] int NOT NULL,
		[TypeOrder2] int NOT NULL,
		[TypeOrder3] int NOT NULL,
		[TypeOrder4] int NOT NULL,
		[RunDate] int NOT NULL,
		[RunID] int NOT NULL,
		[FileID] int NOT NULL,
		[SetID] int NOT NULL,
		[DocID] int NOT NULL,
		[TagName] varchar(20) NULL,
		[Field1] varchar(256) NULL,
		[Field2] varchar(256) NULL,
		[Field3] varchar(256) NULL,
		[Field4] varchar(256) NULL,
		[Field5] varchar(256) NULL,
		[Field6] varchar(256) NULL,
		[Field7] varchar(256) NULL,
		[Field8] varchar(256) NULL,
		[Field9] varchar(256) NULL,
		[Field10] varchar(256) NULL,
		[Field11] varchar(256) NULL,
		[Field12] varchar(256) NULL,
		[Field13] varchar(256) NULL,
		[Field14] varchar(256) NULL,
		[Field15] varchar(256) NULL,
		[Field16] varchar(256) NULL,
		[Field17] varchar(256) NULL,
		[Field18] varchar(256) NULL,
		[Field19] varchar(256) NULL,
		[Field20] varchar(256) NULL,
		[Field21] varchar(256) NULL,
		[Field22] varchar(256) NULL,
		[Field23] varchar(256) NULL,
		[Field24] varchar(256) NULL
	)

	DECLARE @PostObjID int,
		@DocLayout varchar(20),
		@DocType varchar(8),
		@DocPages int,
		@StartPosition numeric(18,0),
		@InputFile varchar(512),
		@PlexCode char(5),
		@DocSetRunID int,
		@DocSetFileID int,
		@DocSetSetID int,
		@DocID int,
		@SetOrder int,
		@ExpBarcode varchar(100),
		@PostObjExpBarcode varchar(100), -- 1.6.3.25 - LOP-20171203
		@ExpSepCodeComplement varchar(10), -- 1.6.4.14
		@ExpPostObjID numeric(18,0), -- 1.6.3.14
		@DocSheets int,
		@OldPostObjID int,
		@OldPostObjFileSeq int,
		@OldExpBarcode varchar(100),
		@OldPostObjExpBarcode varchar(100), -- 1.6.3.25 - LOP-20171203
		@OldExpSepCodeComplement varchar(10), -- 1.6.4.14
		@OldExpPostObjID numeric(18,0), -- 1.6.3.14
		@DocRunDate int,
		@ExpSepCode numeric(18,0),
		@OldExpSepCode numeric(18,0),
		@LastExpSepCodeBanner numeric(18,0),
		@AttCount int,
		@ProjectCode varchar(14),
		@OldProjectCode varchar(14),
		--20150617 - lop
		@DocSetsCount int,
		@OldDocSetRunID int,
		@OldDocSetFileID int,
		@OldDocSetSetID int
		
	DECLARE tCursor CURSOR LOCAL STATIC
	FOR 	SELECT ds.PostObjID, dc.DocLayout, dc.DocType, 
			d.DocPages, dsp.StartPosition, 
			fl2.OutputPath + fl2.OutputName as InputFile, 
			pt.PlexCode, d.RunID, d.FileID, 
			d.SetID, d.DocID, d.SetOrder,
			po.ExpBarCode,
			SUM(dpm.Quantity),
--			po.PostObjPrintSheets,
			r.RunDate,
			ds.ExpSepCode,
			ds.PostObjRunIDLevel,
			ds.[Priority],
			v.ProjectCode,
			ISNULL(ds.ExpSepCodeComplement,'0000'), -- 1.6.4.14
			po.ExpPostObjID -- 1.6.3.14
		FROM 
			RT_POSTAL_OBJECT po WITH(NOLOCK)
		INNER JOIN
			RT_DOCUMENT_SET ds WITH(NOLOCK)
		ON	ds.PostObjRunID = po.PostObjRunID
			AND ds.PostObjID = po.PostObjID
		INNER JOIN
			RT_DOCUMENT d WITH(NOLOCK)
		ON	d.RunID = ds.RunID
			AND d.FileID = ds.FileID
			AND d.SetID = ds.SetID
		INNER JOIN
			RD_DOCCODE dc WITH(NOLOCK)
		ON	d.DocCodeID = dc.DocCodeID
		INNER JOIN
			RD_PLEX_TYPE pt WITH(NOLOCK)
		ON	d.PlexType = pt.PlexType
		INNER JOIN
			RT_RUN r WITH(NOLOCK)
		ON	r.RunID = ds.RunID
		INNER JOIN
			RT_DOCUMENT_PRINT_MATERIAL dpm WITH(NOLOCK)
		ON	dpm.RunID = d.RunID
			AND dpm.FileID = d.FileID
			AND dpm.SetID = d.SetID
			AND dpm.DocID = d.DocID
		INNER JOIN
			RT_DOCUMENT_STATE_POSITION dsp WITH(NOLOCK)
		ON	dsp.RunID = d.RunID
			AND dsp.FileID = d.FileID
			AND dsp.SetID = d.SetID
			AND dsp.DocID = d.DocID
			AND dsp.RunStateID = @EMFERunStateID 
		INNER JOIN
			RT_FILE_LOG fl WITH(NOLOCK)
		ON	fl.RunID = d.RunID
			AND fl.FileID = d.FileID
			AND fl.RunStateID = @REMFERunStateID
			AND fl.EndTimeStamp is NOT NULL
		INNER JOIN
			RT_FILE_LOG fl2 WITH(NOLOCK)
		ON	fl2.RunID = ISNULL(dsp.InsertRunID,dsp.RunID) -- Alteração para os Insert's
			AND fl2.FileID = ISNULL(dsp.InsertFileID,dsp.FileID) -- Alteração para os Insert's
			AND fl2.RunStateID = dsp.RunStateID
			AND fl2.EndTimeStamp is NOT NULL
		INNER JOIN
			VW_FILE_VERSION v WITH(NOLOCK) -- Inserts - LOP - 2012
		ON	v.RunID = fl2.RunID
			AND v.FileID = fl2.FileID
		WHERE po.ProdDetailID = @ProdDetailID
			AND po.PostObjRunID = @PostObjRunID
			AND po.PostObjFileID is NULL
			AND po.PostObjLotID <= @PostObjLotID		
-------------------------------------------------------------
-- 2as Vias -------------------------------------------------
			AND dsp.Pages > 0
-------------------------------------------------------------
			AND fl.ProcCountNr = (SELECT MAX(ProcCountNr) 
								FROM RT_FILE_LOG WITH(NOLOCK)
								WHERE RunID = fl.RunID
									AND FileID = fl.FileID
									AND RunStateID = fl.RunStateID
									AND EndTimeStamp is NOT NULL)
			AND fl2.ProcCountNr = (SELECT MAX(ProcCountNr) 
								FROM RT_FILE_LOG WITH(NOLOCK)
								WHERE RunID = fl2.RunID
									AND FileID = fl2.FileID
									AND RunStateID = fl2.RunStateID
									AND EndTimeStamp is NOT NULL)
			AND NOT EXISTS (SELECT TOP 1 1
					FROM RT_DOCUMENT_STATE_POSITION WITH(NOLOCK)
					WHERE RunID = d.RunID
						AND FileID = d.FileID
						AND SetID = d.SetID
						AND DocID = d.DocID
						AND RunStateID = @RunStateID)
		GROUP BY ds.PostObjID, dc.DocLayout, dc.DocType, 
			d.DocPages, dsp.StartPosition, 
			fl2.OutputPath,
			fl2.OutputName, 
			pt.PlexCode, d.RunID, d.FileID, 
			d.SetID, d.DocID, d.SetOrder,
			po.ExpBarCode,
			ds.ExpSepCode, ds.[Priority], r.RunDate, ds.PostObjRunIDLevel, 
			v.ProjectCode,  -- Inserts - LOP - 2012
			ISNULL(ds.ExpSepCodeComplement,'0000'), -- 1.6.4.14
			po.ExpPostObjID -- 1.6.3.14
			-- d.SetOrder foi adicionado no ORDER BY para garantir que os documentos são tratados pela ordem que são colocados no objecto Postal
	ORDER BY ds.ExpSepCode, 
		ISNULL(ds.ExpSepCodeComplement,'0000'), -- 1.6.4.14
		ds.PostObjID, 
		ds.PostObjRunIDLevel DESC,
		ds.Priority DESC, 
		d.RunID, d.FileID, d.SetID, d.SetOrder 

	SELECT @SheetsCount = 0, @PostObjsCount = 0, 
		@PostObjFileSeq = 0, @OldPostObjID = 0,
		@OldPostObjFileSeq = 0,
		@PostObjSheets = 0,
		@OldExpBarcode = NULL,
		@OldPostObjExpBarcode = NULL, -- 1.6.3.25 - LOP-20171203
		@OldExpSepCodeComplement = NULL, -- 1.6.3.14
		@OldExpPostObjID = NULL, -- 1.6.3.14
		@PrintsCount = 0,
		@DocsCount = 0,
		@DocSetsCount = 0,
		@StartPostObjFileSeq = 0,
		@EndPostObjFileSeq = 0,
		@OldExpSepCode = -1,
		@LastExpSepCodeBanner = -1,
		-- Inserts - LOP - 2012
		@OldProjectCode = NULL,
		-- 20150617 - LOP
		@DocSetsCount = 0,
		@OldDocSetRunID = -1,
		@OldDocSetFileID = -1,
		@OldDocSetSetID = -1

	SELECT  @PostObjFileSeq = MAX(ISNULL(PostObjFileSeq,0)) + 1
	FROM RT_POSTAL_OBJECT
	WHERE ProdDetailID = @ProdDetailID

	-- Nr de Sequência Inicial
	SELECT @StartPostObjFileSeq = @PostObjFileSeq

	OPEN tCursor
	FETCH NEXT FROM tCursor INTO @PostObjID, @DocLayout, @DocType,
			@DocPages, @StartPosition, @InputFile,
			@PlexCode, @DocSetRunID, @DocSetFileID, @DocSetSetID,
			@DocID, @SetOrder, @ExpBarcode, @DocSheets,@DocRunDate, @ExpSepCode, @PostObjRunIDLevel, @Priority,
			@ProjectCode, -- Inserts - LOP - 2012
			@ExpSepCodeComplement, -- 1.6.3.14
			@ExpPostObjID -- 1.6.3.14
			
	IF (@@FETCH_STATUS = 0)
	BEGIN
		SELECT @OldExpSepCode = RIGHT(REPLICATE('0',@InternalCodeStart) + REVERSE(SUBSTRING(REVERSE(CAST(@ExpSepCode as varchar)),@InternalCodeStart-@InternalCodeLen+1,@InternalCodeLen)),@InternalCodeLen),
			@ExpeditionID = 0, @ExpeditionIDAttribute = ''
	END
	WHILE (@@FETCH_STATUS = 0 AND 
			((@SheetsCount<@MaxSheets OR @MaxSheets = 0) 
				OR @OldPostObjID=@PostObjID) -- Bug Fix: Se ainda estiver no mesmo objecto Postal
		)
	BEGIN
		-- Inserts - LOP - 2013
		IF (SUBSTRING(@ProjectCode,1,1)<> @FileCode)
		BEGIN
			SET @FileCode = 'X'
		END
		
		SELECT @DocsCount = @DocsCount + 1
		--20150716 - LOP - Conta DocumentSets que foram marcados originalmente
		IF ((@OldDocSetRunID<>@DocSetRunID OR @OldDocSetFileID<>@DocSetFileID OR @OldDocSetSetID<>@DocSetSetID)
			AND @PostObjRunIDLevel = 1)
		BEGIN
			SELECT @DocSetsCount = @DocSetsCount + 1
		END

		IF (@OldPostObjID<>@PostObjID)
		BEGIN
			SELECT @ExpeditionID = 0, @ExpeditionIDAttribute = ''
			--Tratando-se de Correio Registado
			IF (@RegistMode=1)
			BEGIN
				DELETE RT_POSTAL_OBJECT_ATTRIBUTE
				WHERE PostObjRunID = @PostObjRunID 
					AND PostObjID = @PostObjID
					AND AttributeName in (SELECT AttributeName
						FROM RT_DOCUMENT_ATTRIBUTE WITH(NOLOCK)
						WHERE RunID = @DocSetRunID AND FileID = @DocSetFileID
							AND SetID = @DocSetSetID AND DocID = @DocID
							AND AttributeName like 'PO_%')

				INSERT INTO RT_POSTAL_OBJECT_ATTRIBUTE(PostObjRunID, PostObjID, AttributeName, AttributeValue)
				SELECT @PostObjRunID, @PostObjID, AttributeName, AttributeValue
				FROM RT_DOCUMENT_ATTRIBUTE WITH(NOLOCK)
				WHERE RunID = @DocSetRunID AND FileID = @DocSetFileID
					AND SetID = @DocSetSetID AND DocID = @DocID
					AND AttributeName like 'PO_%'

				SELECT @AttCount = COUNT(DISTINCT AttributeValue)
				FROM RT_DOCUMENT_SET ds WITH(NOLOCK)
				INNER JOIN
					RT_DOCUMENT_ATTRIBUTE da WITH(NOLOCK)
				ON	da.RunID = ds.RunID
					AND da.FileID = ds.FileID
					AND da.SetID = ds.SetID
				WHERE ds.PostObjRunID = @PostObjRunID
					AND ds.PostObjID = @PostObjID
					AND da.AttributeName = 'EXPEDITIONID'

				IF (@AttCount>1)
				BEGIN
					--SQL2017-Start
					--RAISERROR 50204 'More than one document, inside Postal Object, with ExpeditionID'
					RAISERROR ('More than one document, inside Postal Object, with ExpeditionID', 16, 1)
					--SQL2017-End
					RETURN					
				END

				IF (@AttCount = 0)
				BEGIN
					--Gerar o Nº de Registo
					EXEC RTC_GET_EXPEDITIONID
						@ExpCompanyID,
						@ExpeditionID OUTPUT,
						@ExpeditionIDAttribute OUTPUT
					IF (@ExpeditionID<0)
					BEGIN
						--SQL2017-Start
						--RAISERROR 50204 'ExpeditionID for Expedition Company out of range'
						RAISERROR ('ExpeditionID for Expedition Company out of range', 16, 1)
						--SQL2017-End
						RETURN					
					END	
				END
				ELSE
				BEGIN
					SELECT 	@ExpeditionID = ISNULL(CAST(MAX(da2.AttributeValue) as int),0), 
						@ExpeditionIDAttribute = ISNULL(MAX(da1.AttributeValue),'')
					FROM RT_DOCUMENT_SET ds WITH(NOLOCK)
					INNER JOIN
						RT_DOCUMENT_ATTRIBUTE da1 WITH(NOLOCK)
					ON 	ds.PostObjRunID = @PostObjRunID
						AND ds.PostObjID = @PostObjID
						AND da1.RunID = ds.RunID
						AND da1.FileID = ds.FileID
						AND da1.SetID = ds.SetID
						AND da1.AttributeName = 'EXPEDITIONID'
					LEFT OUTER JOIN
						RT_DOCUMENT_ATTRIBUTE da2 WITH(NOLOCK)
					ON	da2.RunID = ds.RunID
						AND da2.FileID = ds.FileID
						AND da2.SetID = ds.SetID
						AND da2.AttributeName = 'EXPEDITIONID_VALUE'

					IF (@ExpeditionID < 0)
					BEGIN
						--SQL2017-Start
						--RAISERROR 50204 'Bad ExpeditionID in Document'
						RAISERROR ('Bad ExpeditionID in Document', 16, 1)
						--SQL2017-End
						RETURN					
					END	
				END

				DELETE RT_POSTAL_OBJECT_ATTRIBUTE
				WHERE PostObjRunID = @PostObjRunID 
					AND PostObjID = @PostObjID
					AND AttributeName = 'EXPEDITIONID'

				INSERT INTO RT_POSTAL_OBJECT_ATTRIBUTE(PostObjRunID, PostObjID, AttributeName, AttributeValue)
				SELECT @PostObjRunID, @PostObjID, 'EXPEDITIONID', @ExpeditionIDAttribute
				IF (@ExpeditionID>0)
				BEGIN
					SET @ExpBarcode = @ExpeditionIDAttribute
				END

				DELETE RT_POSTAL_OBJECT_ATTRIBUTE
				WHERE PostObjRunID = @PostObjRunID 
					AND PostObjID = @PostObjID
					AND AttributeName = 'EXPSEPCODE'

				INSERT INTO RT_POSTAL_OBJECT_ATTRIBUTE(PostObjRunID, PostObjID, AttributeName, AttributeValue)
				SELECT @PostObjRunID, @PostObjID, 'EXPSEPCODE', RIGHT(REPLICATE('0',10) + CAST(@ExpSepCode as varchar(10)),10)
			END

			--Um único objecto postal por ExpSepCode diferente no Correio Interno
			IF (@CompanyID = @ExpCompanyID AND @InternalExpeditionMode & 1 = 1)
			BEGIN
				--6,postalObjFileSeq,2,0,'POSTALBANNER', postObjID, EntityID
				INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
					RunDate, RunID, FileID, SetID, DocID, 
					Field1,Field2)
				SELECT 6,@PostObjFileSeq,2,0,
					'POSTALBANNER',
					0,0,0,0,0,
					CAST(@PostObjID as varchar),
					REPLICATE('0',10-LEN(CAST(@ExpSepCode as varchar))) + CAST(@ExpSepCode as varchar)
				SELECT @PrintsCount = @PrintsCount + CASE WHEN @FilePlexCode = 'DPLEX' THEN 2 ELSE 1 END

				--Avaliar se a contabilização de folhas de Banner é efectuada aqui
				-- Ou já foi efectuada no Aggregation
				--SELECT @BannerCount = @BannerCount + 1,
				--	@PostObjSheets = @PostObjSheets + 1

			END

			IF (@OldPostObjID>0)
			BEGIN
				SELECT @SheetsCount = @SheetsCount + @PostObjSheets, @PostObjsCount = @PostObjsCount + 1
	
				--Alteração para correio interno
				IF (@CompanyID = @ExpCompanyID AND @OldExpSepCode <> @LastExpSepCodeBanner AND @InternalExpeditionMode & 6 <> 0)
				BEGIN
					IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RTC_GET_ENTITY_VALUES]') AND type in (N'P', N'PC'))
					BEGIN
						EXEC RTC_GET_ENTITY_VALUES @PostObjRunID, @OldExpSepCode, @EntityValue1 OUTPUT, @EntityValue2 OUTPUT, @EntityValue3 OUTPUT, @EntityValue4 OUTPUT, @EntityValue5 OUTPUT, @EntityValue6 OUTPUT, @EntityValue7 OUTPUT, @EntityValue8 OUTPUT, @EntityValue9 OUTPUT, @EntityValue10 OUTPUT
					END		
					
					--6,postalObjFileSeq,-1,0,'ENTITYBANNER',EntityID
					INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
						RunDate, RunID, FileID, SetID, DocID, 
						Field1,Field2,
						Field3, Field4, Field5, Field6, Field7, Field8, Field9)
					SELECT 6,@OldPostObjFileSeq,-1,0,
						'ENTITYBANNER',
						0,0,0,0,0,
						REPLICATE('0',@InternalCodeLen-LEN(CAST(@OldExpSepCode as varchar))) + CAST(@OldExpSepCode as varchar),
						(@InternalExpeditionMode & 4) / 4,
					@EntityValue1, @EntityValue2, @EntityValue3, @EntityValue4, @EntityValue5, @EntityValue6, @EntityValue7

					SELECT @BannerCount = @BannerCount + 1, 
						@LastExpSepCodeBanner = @OldExpSepCode
				END
	
				--6, postalObjFileSeq, 0, 0, 'POSTALOBJECT', postObjID, PostObjBarcode, postalObjFileSeq, PostObjNrSheets, expBarCode
				INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
					RunDate, RunID, FileID, SetID, DocID, 
					Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8)
				SELECT 6,@OldPostObjFileSeq,0,0,'POSTALOBJECT',
					0,0,0,0,0,
					CAST(@OldPostObjID as varchar),
					'02' + REPLICATE(0,10-LEN(CAST(@PostObjRunID as varchar))) 
						+ CAST(@PostObjRunID as varchar) 
						+ REPLICATE(0,8-LEN(CAST(@OldPostObjID as varchar))) 
						+ CAST(@OldPostObjID as varchar),
					CAST(@OldPostObjFileSeq as varchar),
					CAST(@PostObjSheets as varchar),
					@OldExpBarcode,
					REPLICATE('0',10-LEN(CAST(@OldExpSepCode as varchar))) + CAST(@OldExpSepCode as varchar),
					@OldExpSepCodeComplement, --1.6.3.14
					@OldExpPostObjID -- 1.6.3.14

				--6, postalObjFileSeq, 1, stationNumber, 'STATION', postObjID, stationNumber, MaterialRef
				INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
					RunDate, RunID, FileID, SetID, DocID, 
					Field1, Field2, Field3)
				SELECT DISTINCT 6,@OldPostObjFileSeq,1,mc.MaterialPosition,'STATION',
					0,0,0,0,0,
					CAST(@OldPostObjID as varchar),
					CAST(mc.MaterialPosition as varchar),
					m.MaterialRef
				FROM RT_POSTAL_OBJECT po WITH(NOLOCK)
				INNER JOIN
					RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
				ON	po.ProdDetailID = pd.ProdDetailID
				INNER JOIN
					RT_DOCUMENT_SET ds WITH(NOLOCK)
				ON	ds.PostObjRunID = po.PostObjRunID
					AND ds.PostObjID = po.PostObjID
				INNER JOIN
					RT_DOCUMENT_STATION_MATERIAL dsm WITH(NOLOCK)
				ON	ds.RunID = dsm.RunID
					AND ds.FileID = dsm.FileID
					AND ds.SetID = dsm.SetID
				INNER JOIN
					RT_MEDIA_CONFIG mc WITH(NOLOCK)
				ON	mc.MediaID = pd.StationMediaID
					AND mc.MaterialID = dsm.MaterialID
				INNER JOIN
					RD_MATERIAL m WITH(NOLOCK)
				ON	m.MaterialID = dsm.MaterialID
				WHERE po.PostObjRunID = @PostObjRunID
					AND po.PostObjID = @OldPostObjID
				ORDER BY mc.MaterialPosition DESC
				SELECT @PostObjSheets = 0, @PostObjFileSeq = @PostObjFileSeq + 1
			END
			
			SELECT @PostObjExpBarcode = @ExpBarcode --LOP-20171203
		END
		--LOP - 20201111 - Fix para + de um doc num objeto postal com correio registado
		ELSE
		BEGIN
			SELECT @ExpBarcode = @OldExpBarcode
		END

		UPDATE RT_POSTAL_OBJECT
		SET PostObjFileID = @FileID, PostObjFileSeq = @PostObjFileSeq,
			ExpeditionID = @ExpeditionID
		WHERE PostObjRunID = @PostObjRunID AND PostObjID = @PostObjID

		--6, postalObjFileSeq, Order1 InPostalObj, Order 2 InPostalObj, 'DOCUMENT',
		--postObjID,docLayout,docType,docPages,'StartPosition', 'PostScript FileName with path', 
		--PlexType, RunID, FileID, SetID, DocID
		INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
				RunDate, RunID, FileID, SetID, DocID, 
				Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11)
		--Before V1.6.4.15
		/*
		SELECT 6,@PostObjFileSeq,100-@PostObjRunIDLevel, 2147483647-@Priority, 'DOCUMENT',
		*/
		--After V1.6.4.15
		SELECT 6, @PostObjFileSeq, 100-(CASE WHEN ISNULL(@PostObjOrderMode,0) = 0 THEN @PostObjRunIDLevel ELSE 0 END),
			2147483647-(CASE WHEN ISNULL(@PostObjOrderMode,0) = 0 THEN @Priority ELSE 0 END), 'DOCUMENT',
		--End V1.6.4.15
			@DocSetRunID,@DocSetFileID,@DocSetSetID,@SetOrder,@DocID,
			CAST(@PostObjID as varchar), 
			@DocLayout, 
			CASE WHEN LEN(@DocType) = 0 THEN '@' ELSE @DocType END,
			CAST(@DocPages as varchar), 
			CAST(@StartPosition as varchar(256)), 
			@InputFile,
			@PlexCode, 
			CAST(@DocSetRunID as varchar), 
			CAST(@DocSetFileID as varchar), 
			CAST(@DocSetSetID as varchar),
			CAST(@DocID as varchar)
		
		SELECT @OldPostObjID = @PostObjID, @OldExpBarcode = @ExpBarcode,
			@OldPostObjExpBarcode = @PostObjExpBarcode, -- 1.6.3.25 - LOP-20171203
			@OldExpSepCodeComplement = @ExpSepCodeComplement, -- 1.6.3.14
			@OldExpPostObjID = @ExpPostObjID, -- 1.6.3.14
			@OldPostObjFileSeq = @PostObjFileSeq,
			@OldExpSepCode = RIGHT(REPLICATE('0',@InternalCodeStart) + REVERSE(SUBSTRING(REVERSE(CAST(@ExpSepCode as varchar)),@InternalCodeStart-@InternalCodeLen+1,@InternalCodeLen)),@InternalCodeLen),
			@PostObjSheets = @PostObjSheets + @DocSheets,
			@PrintsCount = @PrintsCount + 
				CASE WHEN @FilePlexCode = 'DPLEX' THEN 
					CASE WHEN @PlexCode = 'SPLEX' THEN @DocPages * 2
						ELSE (@DocPages + @DocPages%2) END
					ELSE @DocPages END,
			 -- Inserts - LOP - 2012
			@OldProjectCode = @ProjectCode,
			--20150617 - LOP
			@OldDocSetRunID = @DocSetRunID,
			@OldDocSetFileID = @DocSetFileID,
			@OldDocSetSetID = @DocSetSetID


		FETCH NEXT FROM tCursor INTO @PostObjID, @DocLayout, @DocType,
				@DocPages, @StartPosition, @InputFile,
				@PlexCode, @DocSetRunID, @DocSetFileID, @DocSetSetID,
				@DocID,@SetOrder,@ExpBarcode,@DocSheets,@DocRunDate, @ExpSepCode,@PostObjRunIDLevel, @Priority,
				@ProjectCode, -- Inserts - LOP - 2012
				@ExpSepCodeComplement, -- 1.6.3.14
				@ExpPostObjID -- 1.6.3.14
	END
	CLOSE tCursor
	DEALLOCATE tCursor

	IF (@PostObjID>0)
	BEGIN
		SELECT @SheetsCount = @SheetsCount + @PostObjSheets, @PostObjsCount = @PostObjsCount + 1

		--Alteração para correio interno
		IF (@CompanyID = @ExpCompanyID AND @OldExpSepCode <> @LastExpSepCodeBanner AND @InternalExpeditionMode & 6 <> 0)
		BEGIN			
			IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RTC_GET_ENTITY_VALUES]') AND type in (N'P', N'PC'))
			BEGIN
				EXEC RTC_GET_ENTITY_VALUES @PostObjRunID, @OldExpSepCode, @EntityValue1 OUTPUT, @EntityValue2 OUTPUT, @EntityValue3 OUTPUT, @EntityValue4 OUTPUT, @EntityValue5 OUTPUT, @EntityValue6 OUTPUT, @EntityValue7 OUTPUT, @EntityValue8 OUTPUT, @EntityValue9 OUTPUT, @EntityValue10 OUTPUT
			END		
			
			--6,postalObjFileSeq,-1,0,'ENTITYBANNER',EntityID
			INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
				RunDate, RunID, FileID, SetID, DocID, 
				Field1,Field2,
				Field3, Field4, Field5, Field6, Field7, Field8, Field9)
			SELECT 6,@OldPostObjFileSeq,-1,0,
				'ENTITYBANNER',
				0,0,0,0,0,
				REPLICATE('0',@InternalCodeLen-LEN(CAST(@OldExpSepCode as varchar))) + CAST(@OldExpSepCode as varchar),
				(@InternalExpeditionMode & 4) / 4,
			@EntityValue1, @EntityValue2, @EntityValue3, @EntityValue4, @EntityValue5, @EntityValue6, @EntityValue7

			SELECT @BannerCount = @BannerCount + 1, 
				@LastExpSepCodeBanner = @OldExpSepCode
		END
		
		--6, postalObjFileSeq, 0, 0, 'POSTALOBJECT', postObjID, PostObjBarcode, postalObjFileSeq, PostObjNrSheets, expBarCode
		INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
				RunDate, RunID, FileID, SetID, DocID, 
				Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8)
		SELECT 6,@OldPostObjFileSeq,0,0,'POSTALOBJECT',
			0,0,0,0,0,
			CAST(@OldPostObjID as varchar),
			'02' + REPLICATE(0,10-LEN(CAST(@PostObjRunID as varchar))) 
				+ CAST(@PostObjRunID as varchar) 
				+ REPLICATE(0,8-LEN(CAST(@OldPostObjID as varchar))) 
				+ CAST(@OldPostObjID as varchar),
			CAST(@OldPostObjFileSeq as varchar),
			CAST(@PostObjSheets as varchar),
			@OldPostObjExpBarcode, -- 1.6.3.25 - LOP-20171203
			--@OldExpBarcode, -- 1.6.3.25 - LOP-20171203
			REPLICATE('0',10-LEN(CAST(@OldExpSepCode as varchar))) + CAST(@OldExpSepCode as varchar),
			@OldExpSepCodeComplement, --1.6.3.14
			@OldExpPostObjID -- 1.6.3.14

		--6, postalObjFileSeq, 1, stationNumber, 'STATION', postObjID, stationNumber, MaterialRef
		INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
				RunDate, RunID, FileID, SetID, DocID, 
				Field1, Field2, Field3)
		SELECT DISTINCT 6,@OldPostObjFileSeq,1,mc.MaterialPosition,'STATION',
			0,0,0,0,0,
			CAST(@OldPostObjID as varchar),
			CAST(mc.MaterialPosition as varchar),
			m.MaterialRef
		FROM RT_POSTAL_OBJECT po WITH(NOLOCK)
		INNER JOIN
			RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
		ON	po.ProdDetailID = pd.ProdDetailID
		INNER JOIN
			RT_DOCUMENT_SET ds WITH(NOLOCK)
		ON	ds.PostObjRunID = po.PostObjRunID
			AND ds.PostObjID = po.PostObjID
		INNER JOIN
			RT_DOCUMENT_STATION_MATERIAL dsm WITH(NOLOCK)
		ON	ds.RunID = dsm.RunID
			AND ds.FileID = dsm.FileID
			AND ds.SetID = dsm.SetID
		INNER JOIN
			RT_MEDIA_CONFIG mc WITH(NOLOCK)
		ON	mc.MediaID = pd.StationMediaID
			AND mc.MaterialID = dsm.MaterialID
		INNER JOIN
			RD_MATERIAL m WITH(NOLOCK)
		ON	m.MaterialID = dsm.MaterialID
		WHERE po.PostObjRunID = @PostObjRunID
			AND po.PostObjID = @OldPostObjID
		ORDER BY mc.MaterialPosition DESC
	END

	--Nr de Sequência Final
	SELECT @EndPostObjFileSeq = @PostObjFileSeq

	IF (@SheetsCount = 0)
	BEGIN
		EXEC RT_DELETE_FILE @PostObjRunID, @FileID

		SELECT RunID
		FROM RT_RUN WITH(NOLOCK)
		WHERE RunID is NULL
	END
	ELSE
	BEGIN
		EXEC RT_UPDATE_FILE_MATERIAL @PostObjRunID, @FileID, @BannerCount

		--1,0,trayid,0,'TRAY', materialRef, trayid, trayName, Quantity
		INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
			RunDate, RunID, FileID, SetID, DocID, 
			Field1, Field2, Field3, Field4)
		SELECT 1,0,mc.MaterialPosition,0,'TRAY',
			0,0,0,0,0,
			m.MaterialRef, CAST(mc.MaterialPosition as varchar), 
			m.MaterialRef, CAST(SUM(fm.Quantity) as varchar)
		FROM RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
		INNER JOIN
			RT_MEDIA_CONFIG mc WITH(NOLOCK)
		ON 	mc.MediaID = pd.PaperMediaID
		INNER JOIN
			RD_MATERIAL m WITH(NOLOCK)
		ON	m.MaterialID = mc.MaterialID
		INNER JOIN
			RT_FILE_MATERIAL fm WITH(NOLOCK)
		ON	fm.MaterialID = m.MaterialID
		WHERE p.ProdDetailID = @ProdDetailID
			AND fm.RunID = @PostObjRunID
			AND fm.FileID = @FileID
		GROUP BY m.MaterialRef, mc.MaterialPosition
		ORDER BY mc.MaterialPosition DESC
	
		IF (@MaxStationsFlag = 0)
		BEGIN
			--2,0,stationNumber,0,'STATION',materialref,stationNumber,Quantity	
			INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
				RunDate, RunID, FileID, SetID, DocID, 
				Field1, Field2, Field3)
			SELECT 2,0,CAST(Field2 as int),0,'STATION',
				0,0,0,0,0,
				Field3, Field2, COUNT(*)
			FROM #FORK_REPORT
			WHERE TypeOrder1 = 6 AND TagName = 'STATION'
			GROUP BY Field3,Field2
		END
		ELSE
		BEGIN
			--2,0,stationNumber,0,'STATION',materialref,stationNumber,Quantity	
			INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
				RunDate, RunID, FileID, SetID, DocID, 
				Field1, Field2, Field3)
			SELECT 2,0,0,0,'STATION',
				0,0,0,0,0,
				@StationExceededDesc, 0, COUNT(*)
			FROM #FORK_REPORT
			WHERE TypeOrder1 = 6 AND TagName = 'STATION'
		END
	
		--3,0,0,0,'ENVELOPE',materialref,Format
		INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
			RunDate, RunID, FileID, SetID, DocID, 
			Field1, Field2)
		SELECT 3,0,0,0,'ENVELOPE',
			0,0,0,0,0,
			m.MaterialRef, m.FullFillMaterialCode
		FROM RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
		INNER JOIN
			RD_MATERIAL m WITH(NOLOCK)
		ON	m.MaterialID = pd.EnvMaterialID
		WHERE pd.ProdDetailID = @ProdDetailID
	
		--4,0,0,0,'IMAGE',Nome
		INSERT INTO #FORK_REPORT(TypeOrder1, TypeOrder2, TypeOrder3, TypeOrder4, TagName, 
			RunDate, RunID, FileID, SetID, DocID, 
			Field1)
		SELECT DISTINCT 4,0,0,0,'IMAGE',
			0,0,0,0,0,
			di.ImageName
		FROM RT_POSTAL_OBJECT po WITH (NOLOCK)
		INNER JOIN
			RT_DOCUMENT_SET ds WITH(NOLOCK)
		ON	ds.PostObjRunID = po.PostObjRunID
			AND ds.PostObjID = po.PostObjID
		INNER JOIN
			RTC_DOCUMENT_IMAGES di WITH(NOLOCK)
		ON	di.RunID = ds.RunID
			AND di.FileID = ds.FileID
			AND di.SetID = ds.SetID
		WHERE po.PostObjRunID = @PostObjRunID
			AND po.PostObjFileID = @FileID
		
		DECLARE @FileBarCode varchar(20)
		SELECT @FileBarCode = '01' + REPLICATE('0',10-LEN(CAST(@PostObjRunID as varchar))) + CAST(@PostObjRunID as varchar) 
			+ REPLICATE('0',8-LEN(CAST(@FileID as varchar))) + CAST(@FileID as varchar)
		--0,0,0,0,'FILE',barCode,fileBarCode,recoverBarCode,
		  --ServiceTaskCode,expLevel,expCompany,expType,expCenterCode,
		  --expZone,PlexType,set,runid,runDate,FileName,recoverFileName,TotalPrints,
		  --TotalDocs,TotalPostObjs,StartSeqNum,EndSeqNum,
		  --Descrição do Ficheiro
		INSERT INTO #FORK_REPORT
		SELECT 0,0,0,0,
			0,0,0,0,0,
			'FILE', 
			@Barcode, @FileBarCode, @FileBarCode,
			@ServiceTaskCode, @ExpLevel, 
			@ExpCompanyCode, @ExpeditionTypeDesc, 
			@ExpCenterCodeDesc, @ExpeditionZoneDesc, 
			@FilePlexCode, @FileSetNr,
			@PostObjRunID, @RunDate,
			@FileName,
			@FileName,
			@PrintsCount,
			@DocsCount,
			@PostObjsCount,
			@StartPostObjFileSeq,
			@EndPostObjFileSeq,
			CASE WHEN SUBSTRING(@BusinessCode,1,1) = 'C' THEN 
				CASE WHEN LEN(CAST(@RunSequence as varchar))<2 THEN '0' + CAST(@RunSequence as varchar) 
						ELSE CAST(@RunSequence as varchar) END + ' - ' + @SystemName
			ELSE CASE WHEN @BusinessCode = 'B' THEN 'BANCA' WHEN @BusinessCode = 'S' THEN 'SEGUROS' ELSE @BusinessCode END 
				+ ' (' + CAST(@RunDate as varchar)  + '/' + CAST(@RunSequence as varchar) + '), ' + @ServiceTaskDesc 
				+ ', ' + @ExpCompanyCode + '/' + @ExpeditionTypeDesc + '/' + @ExpeditionZoneDesc  + '/' + @ExpCenterCode
			END,
			@ExpeditionLevelDesc,
			@CompanyRegistCode,
			@ExpeditionClientNr
	
		IF (EXISTS (SELECT * FROM RT_FILE_PRODUCTION WHERE RunID = @PostObjRunID AND FileID = @FileID))
		BEGIN
			DELETE RT_FILE_PRODUCTION
			WHERE  RunID = @PostObjRunID AND FileID = @FileID
		END
	
		INSERT INTO RT_FILE_PRODUCTION(RunID, FileID, TotalPrint, TotalDocs, TotalPostObjs, StartSeqNum, EndSeqNum,ServiceTaskCode, ExpCompanyCode, ExpeditionType, ExpeditionZone, PlexCode, ExpCenterCodeDesc, ExpeditionLevelDesc)
		SELECT @PostObjRunID, @FileID, @PrintsCount, @DocsCount, @PostObjsCount, @StartPostObjFileSeq, @PostObjFileSeq, @ServiceTaskCode, @ExpCompanyCode, @ExpeditionTypeDesc, @ExpeditionZoneDesc, @FilePlexCode, @ExpCenterCodeDesc, @ExpeditionLevelDesc

		--V1.0.3.19 ou V1.4.0.1
		IF (EXISTS (SELECT TOP 1 1 FROM RD_C_EXPCOMPANY_REGIST_DETAIL_FILE WITH(NOLOCK) WHERE ExpCompanyID = @ExpCompanyID AND ExpeditionType = @ExpeditionType))
		BEGIN
			SELECT @FileType = FileType
			FROM RD_C_EXPCOMPANY_REGIST_DETAIL_FILE WITH(NOLOCK)
			WHERE ExpCompanyID = @ExpCompanyID AND ExpeditionType = @ExpeditionType

			IF (EXISTS (SELECT * FROM RT_EXPCOMPANY_REGIST_DETAIL_FILE WHERE RunID = @PostObjRunID AND FileID = @FileID))
			BEGIN
				DELETE RT_EXPCOMPANY_REGIST_DETAIL_FILE
				WHERE  RunID = @PostObjRunID AND FileID = @FileID
			END
			INSERT INTO RT_EXPCOMPANY_REGIST_DETAIL_FILE(RunID, FileID, RecNumber, FileType, TotalPostObjs, StartSeqNum, EndSeqNum, ExpCompanyID, ContractID, CompanyRegistCode, ExpeditionType)
			SELECT @PostObjRunID, @FileID, 0, @FileType, @PostObjsCount, @StartPostObjFileSeq, @EndPostObjFileSeq, 
				@ExpCompanyID, @ContractID, @CompanyRegistCode, @ExpeditionType
		END
		-------------------------
		
		-- Inserts - LOP - 2013
		IF	(@FileCode IS NOT NULL)
		BEGIN
			UPDATE RT_FILE_REGIST
			SET FileCode = @FileCode
			WHERE RunID = @PostObjRunID AND FileID = @FileID
		END
	
		SELECT @FileID FileID, @ProcCountNr ProcCountNr,
			@ServiceCompanyCode ServiceCompanyCode, 
			@PostObjsCount PostObjsCount,
			@SheetsCount SheetsCount,
			@FileName [FileName],
			f.TagName,
			f.Field1,
			f.Field2,
			f.Field3,
			f.Field4,
			f.Field5,
			f.Field6,
			f.Field7,
			f.Field8,
			f.Field9,
			f.Field10,
			f.Field11,
			f.Field12,
			f.Field13,
			f.Field14,
			f.Field15,
			f.Field16,
			f.Field17,
			f.Field18,
			f.Field19,
			f.Field20,
			f.Field21,
			f.Field22,
			f.Field23,
			f.Field24,
			-- Ultimos 9 campos são ignorados no Report
			f.TypeOrder1, f.TypeOrder2, f.TypeOrder3, f.TypeOrder4, f.RunDate, f.RunID, f.FileID, f.SetID, f.DocID, @DocSetsCount DocSetsCount
		FROM #FORK_REPORT f
		ORDER BY f.TypeOrder1 ASC,
			f.TypeOrder2 ASC,
			f.TypeOrder3 ASC,
			f.TypeOrder4 ASC,
			f.RunDate ASC,
			f.RunID ASC,
			f.FileID ASC,
			f.SetID ASC,
			f.DocID ASC
	END

	DROP TABLE #FORK_REPORT
	SET NOCOUNT OFF
GO
ALTER  PROCEDURE [dbo].[RT_CHECK_ALL_INTEGRATION_FILES_DONE]
--WITH ENCRYPTION
AS
	SET NOCOUNT ON
	
	DECLARE @ExpeditionStateID int,
		@RegistFileRunStateID int,
		@RegistEmfeStateID int,
		@ArchiveStateID int,
		@EmailStateID int,
		@HtmlStateID int,
		@SuportType int
		
	SELECT @ExpeditionStateID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'EXPEDITION'

	SELECT @RegistFileRunStateID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'REGISTFILE'

	SELECT @RegistEmfeStateID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'REGISTEMFE'

	SELECT @ArchiveStateID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'ARCHIVE'

	SELECT @EmailStateID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'EMAIL'

	SELECT @HtmlStateID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'EHTMLARCHIVE'

	CREATE TABLE #FILE(RunID int, FileID int)

	DECLARE tCursor CURSOR LOCAL
	FOR 
	SELECT fr.RunID, fr.FileID,
		COUNT(ds.RunID) TotalDocsToFinishing,
		ISNULL(fexp.TotalDocsExpedition,0) TotalDocsExpedition,
		ISNULL(fexp.LastExpeditionTime,0) LastExpeditionTime,
		ISNULL(fexp.StartExpeditionTime,0) StartExpeditionTime
	FROM
		RT_FILE_REGIST fr WITH(NOLOCK)
	INNER JOIN
		RT_DOCUMENT_SET ds WITH(NOLOCK)
	ON	fr.RunID = ds.RunID
		AND fr.FileID = ds.FileID
	LEFT OUTER JOIN
		(SELECT ds1.RunID, ds1.FileID, COUNT(1) TotalDocsExpedition, MAX(ISNULL(flexp.EndTimeStamp,0)) LastExpeditionTime, MIN(ISNULL(flexp.EndTimeStamp,0)) StartExpeditionTime
		FROM RT_POSTAL_OBJECT po WITH(NOLOCK)
		INNER JOIN
			RT_DOCUMENT_SET ds1 WITH(NOLOCK)
		ON	po.PostObjRunID = ds1.PostObjRunID
			AND po.PostObjID = ds1.PostObjID			
		INNER JOIN
			RT_FILE_LOG flexp WITH(NOLOCK)
		ON po.PostObjRunID = flexp.RunID
			AND po.PostObjFileID = flexp.FileID
		WHERE flexp.RunStateID = @ExpeditionStateID
			AND flexp.EndTimeStamp is NOT NULL
			AND ds1.ErrorID = 0
			AND po.ErrorID = 0
			AND flexp.ErrorID = 0
		GROUP BY ds1.RunID, ds1.FileID) fexp
	ON fexp.Runid = fr.RunID
		AND fexp.FileID = fr.FileID     	
	WHERE 
		ds.suportType & 1 = 1
		--Só apanha o que não estiver em erro. Na query seguinte trata os com erro
		AND ds.ErrorID = 0
		AND fr.ErrorID = 0
		AND EXISTS (SELECT TOP 1 1 FROM RT_FILE_LOG fl WITH(NOLOCK)
					WHERE fl.RunID = fr.RunID AND fl.FileID = fr.FileID
					AND fl.EndTimeStamp is NOT NULL
					AND fl.RunStateID = @RegistFileRunStateID
					AND fl.ErrorID = 0)
		AND EXISTS (SELECT TOP 1 1 
				FROM RT_FILE_LOG WITH(NOLOCK) 
				WHERE RunID = fr.RunID 
					AND FileID = fr.FileID
					AND RunStateID = @RegistFileRunStateID
					AND EndTimeStamp is NOT NULL)
		AND NOT EXISTS (SELECT RunID 
				FROM RT_FILE_LOG WITH(NOLOCK)
				WHERE RunID = fr.RunID 
					AND FileID = fr.FileID 
					AND RunStateID = @ExpeditionStateID
					AND EndTimeStamp is NOT NULL)
	GROUP BY fr.RunID, fr.FileID, fexp.TotalDocsExpedition, fexp.LastExpeditionTime, fexp.StartExpeditionTime
	ORDER BY fr.RunID, fr.FileID

	DECLARE @RunID int, 
		@FileID int, 
		@TotalDocsToFinishing int, 
		@TotalDocsExpedition int,
		@StartExpeditionTime dateTime,
		@LastExpeditionTime dateTime
	OPEN tCursor

	FETCH NEXT FROM tCursor INTO @RunID, @FileID, @TotalDocsToFinishing, @TotalDocsExpedition, @LastExpeditionTime, @StartExpeditionTime

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@TotalDocsToFinishing = @TotalDocsExpedition)
		BEGIN
			INSERT INTO RT_FILE_LOG(RunID, FileID, RunStateID, ProcCountNr, StartTimeStamp, EndTimeStamp, OutputPath, OutputName, ErrorID)
			SELECT @RunID, @FileID, @ExpeditionStateID, ISNULL(MAX(ProcCountNr),0) + 1, @StartExpeditionTime, NULL, NULL, NULL, 0
			FROM RT_FILE_LOG WITH(NOLOCK)
			WHERE RunID = @RunID 
				AND FileID = @FileID
				AND RunStateID = @ExpeditionStateID

			UPDATE 	RT_FILE_LOG 
			SET 	EndTimeStamp = @LastExpeditionTime
			WHERE 	RunID = @RunID 
				AND FileID = @FileID
				AND RunStateID = @ExpeditionStateID

			INSERT INTO #FILE
			SELECT f.RunID, f.ParentFileID
			FROM RT_FILE_REGIST f WITH(NOLOCK)
			WHERE RunID = @RunID AND FileID = @FileID
				AND NOT EXISTS (SELECT TOP 1 1 FROM #FILE WHERE RunID = @RunID AND FileID = f.ParentFileID)
		END
		FETCH NEXT FROM tCursor INTO @RunID, @FileID, @TotalDocsToFinishing, @TotalDocsExpedition, @LastExpeditionTime, @StartExpeditionTime
	END
	CLOSE tCursor
	DEALLOCATE tCursor

	--Dá imediatemente expedidos os ficheiros que não têm nada para expedir
	DECLARE tCursor CURSOR LOCAL
	FOR 
	SELECT f.RunID, f.FileID, MAX(ISNULL(fl.EndTimeStamp,fl.StartTimeStamp)) LastExpeditionTime, f.SuportType
	FROM
		RT_FILE_REGIST f WITH(NOLOCK)
	INNER JOIN
		RT_FILE_LOG fl WITH(NOLOCK)
	ON	fl.RunID = f.RunID
		AND fl.FileID = f.FileID
	WHERE
		f.RunStateID = @RegistFileRunStateID
		AND
		NOT EXISTS (SELECT TOP 1 1 
				FROM RT_FILE_LOG WITH(NOLOCK)
				WHERE RunID = f.RunID 
					AND FileID = f.FileID 
					AND RunStateID = @ExpeditionStateID
					AND EndTimeStamp is NOT NULL
					AND ErrorID = 0)
		AND
		(
			f.ErrorID <> 0
		OR 
			(f.SuportType & 1 = 0 
			AND EXISTS (SELECT TOP 1 1
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = f.RunID 
						AND FileID = f.FileID 
						AND RunStateID = @RegistEmfeStateID
						AND EndTimeStamp is NOT NULL
						AND ErrorID = 0)
			AND (f.SuportType & 2 = 0
				OR EXISTS (SELECT TOP 1 1 
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = f.RunID 
						AND (FileID = f.FileID OR FileID = f.ParentFileID)
						AND RunStateID = @ArchiveStateID
						AND EndTimeStamp is NOT NULL
						AND ErrorID = 0))
			AND (f.SuportType & 96 = 0
				OR EXISTS (SELECT TOP 1 1
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = f.RunID 
						AND (FileID = f.FileID OR FileID = f.ParentFileID)
						AND RunStateID = @EmailStateID
						AND EndTimeStamp is NOT NULL
						AND ErrorID = 0))
			AND (f.SuportType & 32 = 0
				OR EXISTS (SELECT TOP 1 1
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = f.RunID 
						AND (FileID = f.FileID OR FileID = f.ParentFileID)
						AND RunStateID = @HtmlStateID
						AND EndTimeStamp is NOT NULL
						AND ErrorID = 0))
			)
		)
	GROUP BY f.RunID, f.FileID, f.SuportType
	ORDER BY f.RunID, f.FileID

	OPEN tCursor
	FETCH NEXT FROM tCursor INTO @RunID, @FileID, @LastExpeditionTime, @SuportType

	DECLARE @PendingInserts int, 
		@TotalInserts int,
		@InsertsCount int

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@SuportType & 12 = 4)
		BEGIN
			SELECT @TotalInserts = COUNT(*)
			FROM RT_INSERT WITH(NOLOCK)
			WHERE RunID = @RunID AND FileID = @FileID

			/*Se ficheiro não contém inserts*/
			IF (@TotalInserts = 0)
			BEGIN
				/* Ficheiro de Inserts pode ser dado como expedido */
				SET @SuportType = 0
			END
			ELSE
			BEGIN
				SELECT @InsertsCount = COUNT(*)
				FROM RT_INSERT WITH(NOLOCK)
				WHERE RunID = @RunID AND FileID = @FileID
					AND [ExpireDate] >= CAST(CONVERT(varchar(8),CURRENT_TIMESTAMP,112) as int)

				/* Se todos os Inserts já expiraram */
				IF (@InsertsCount = 0)
				BEGIN
					/* Verifica se todos os documentos que usaram os inserts, dentro do ficheiro, já foram expedidos*/
					SELECT @InsertsCount = COUNT(*) 
					FROM RT_DOCUMENT_STATE_POSITION dsp WITH(NOLOCK)
					INNER JOIN
						RT_DOCUMENT_SET ds WITH(NOLOCK)
					ON	ds.RunID = dsp.RunID
						AND ds.FileID = dsp.FileID
						AND ds.SetID = dsp.SetID
					INNER JOIN
						RT_POSTAL_OBJECT po WITH(NOLOCK)
					ON	ds.PostObjRunID = po.PostObjRunID 
						AND ds.PostObjID = po.PostObjID
					WHERE dsp.InsertRunID = @RunID
						AND dsp.InsertFileID = @FileID
						AND NOT EXISTS (SELECT TOP 1 1 FROM RT_FILE_LOG fl WITH(NOLOCK)
										WHERE RunID = po.PostObjRunID
											AND FileID = po.PostObjFileID
											AND RunStateID = @ExpeditionStateID
											AND EndTimeStamp is NOT NULL)
						AND ds.ErrorID = 0
						AND po.PostObjID = 0
					/* Se não há documentos, que utilizaram os inserts, por expedir*/
					IF (@InsertsCount = 0)
					BEGIN
						/* Ficheiro de Inserts pode ser dado como expedido */
						SET @SuportType = 0
					END
				END
			END
		END

		/* Se não se tratar de Inserts ou se se concluir que ficheiro pode ser dado por expedido*/
		IF (@SuportType & 4 = 0)
		BEGIN
			INSERT INTO RT_FILE_LOG(RunID, FileID, RunStateID, ProcCountNr, StartTimeStamp, EndTimeStamp, OutputPath, OutputName, ErrorID)
			SELECT @RunID, @FileID, @ExpeditionStateID, ISNULL(MAX(ProcCountNr),0) + 1, @LastExpeditionTime, NULL, NULL, NULL, 0
			FROM RT_FILE_LOG WITH(NOLOCK)
			WHERE RunID = @RunID 
				AND FileID = @FileID
				AND RunStateID = @ExpeditionStateID

			UPDATE 	RT_FILE_LOG 
			SET 	EndTimeStamp = @LastExpeditionTime
			WHERE 	RunID = @RunID 
				AND FileID = @FileID
				AND RunStateID = @ExpeditionStateID

			INSERT INTO #FILE
			SELECT f.RunID, f.ParentFileID
			FROM RT_FILE_REGIST f WITH(NOLOCK)
			WHERE RunID = @RunID AND FileID = @FileID
				AND NOT EXISTS (SELECT TOP 1 1 FROM #FILE WHERE RunID = @RunID AND FileID = f.ParentFileID)
		END
		FETCH NEXT FROM tCursor INTO @RunID, @FileID, @LastExpeditionTime, @SuportType
	END
	CLOSE tCursor
	DEALLOCATE tCursor

	--Efetuar tratamento de "PRE-HISTORY"
	DECLARE tCursor CURSOR LOCAL
	FOR 
	SELECT RunID, FileID
	FROM #FILE
	ORDER BY RunID, FileID

	FETCH NEXT FROM tCursor INTO @RunID, @FileID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC HRT_PREHISTORY_STATE @RunID = @RunID, @FileID = @FileID, @RunState = 'PREHISTORY'
		FETCH NEXT FROM tCursor INTO @RunID, @FileID
	END
	CLOSE tCursor
	DEALLOCATE tCursor
GO
ALTER PROCEDURE [dbo].[HRT_PREHISTORY_STATE] 
	@RunID int,
	@RunState varchar(20),
	@FileID int = NULL
AS
	DECLARE @RunStateID int,
		@ParentFileID int,
		@ArchFileCount int,
		@RunTypeName varchar(10),
		@errStr varchar(256),
		@ArchRunStateID int
	
	
	SELECT @RunStateID = NULL, @ParentFileID = NULL
	
	SELECT @RunTypeName = rt.RunTypeName 
	FROM RT_RUN r WITH(NOLOCK)
	INNER JOIN
		RD_RUN_TYPE rt WITH(NOLOCK)
	ON	r.RunTypeID = rt.RunTypeID
	WHERE RunID = @RunID

	BEGIN TRANSACTION
	
	IF (@RunTypeName is NULL)
	BEGIN
		SELECT @errStr = 'CONSYSTENCY PROBLEM RunID = ' + cast(@RunID as varchar) + ' not registered'
		--SQL2017-Start
		--RAISERROR 50001 @errStr
		RAISERROR (@errStr, 16, 1)
		--SQL2017-End
		GOTO END_ERROR
	END
	
	IF (@RunTypeName = 'INTEGRATE' AND @RunState = 'PREHISTORY')
	BEGIN
	
		SELECT @errStr = 'CONSYSTENCY PROBLEM RunID = ' + CAST(@RunID as varchar)
		IF @FileID is NULL
		BEGIN
			SELECT @errStr = @errStr + ' RunTypeName = ''INTEGRATE'' FileID MISSING'
			--SQL2017-Start
			--RAISERROR 50001 @errStr
			RAISERROR (@errStr, 16, 1)
			--SQL2017-End
			GOTO END_ERROR
		END
		
		SELECT @errStr = @errStr + ' FileID = ' + CAST(@FileID as varchar)
	
		SELECT @ParentFileID = ParentFileID
		FROM RT_FILE_REGIST 
		WHERE RunID = @RunID AND FileID = @FileID
		
		IF (@ParentFileID is NOT NULL)
		BEGIN
			SELECT @errStr = @errStr + ' not an Origin File'
			--SQL2017-Start
			--RAISERROR 50002 @errStr
			RAISERROR (@errStr, 16, 1)
			--SQL2017-End
			GOTO END_ERROR
		END
	
		-------------------------------------------------------------------------------------
		--ARCHIVE
		-------------------------------------------------------------------------------------
		SELECT @RunStateID = RunStateID
		FROM RD_RUN_STATE WITH(NOLOCK)
		WHERE RunStateName='ARCDISKDELETE'
		IF (@RunStateID is NULL)
		BEGIN
			SELECT @errStr = @errStr + ' unknown RunState = ''ARCDISKDELETE'''
			--SQL2017-Start
			--RAISERROR 50003 @errStr
			RAISERROR (@errStr, 16, 1)
			--SQL2017-End
			GOTO END_ERROR
		END

		SELECT @ArchRunStateID = RunStateID
		FROM RD_RUN_STATE  WITH(NOLOCK)
		WHERE RunStateName = 'ARCHIVE'
		IF (@ArchRunStateID is NULL)
		BEGIN
			SELECT @errStr = @errStr + ' unknown RunState = ''ARCHIVE'''
			--SQL2017-Start
			--RAISERROR 50003 @errStr
			RAISERROR (@errStr, 16, 1)
			--SQL2017-End
			GOTO END_ERROR
		END
		
		IF NOT EXISTS (SELECT TOP 1 1
					FROM HRT_PENDING_STATUS WITH(NOLOCK)
					WHERE RunID = @RunID AND FileID = @FileID 
						AND PendingStateID = @RunStateID)	
		BEGIN
			BEGIN TRANSACTION
			IF NOT EXISTS(SELECT TOP 1 1
				FROM
					RT_FILE_REGIST fr WITH(NOLOCK)
				INNER JOIN
					RT_FILE_REGIST frp WITH(NOLOCK)
				ON	fr.RunID = frp.RunID
					AND fr.FileID = frp.ParentFileID
				INNER JOIN
					RT_DOCUMENT d WITH(NOLOCK)
				ON	d.RunID = frp.RunID
					AND d.FileID = frp.FileID
				WHERE fr.RunID = @RunID AND fr.FileID = @FileID
						AND d.ErrorID = 0
						AND d.SuportType & 2 = 2)
			BEGIN
				-- Se não existiam documentos para arquivar dentro do ficheiro, dá ficheiro automáticamente por arquivado e expurgado
				INSERT INTO RT_FILE_LOG (RunID, FileID, RunStateID, ProcCountNr, StartTimeStamp, EndTimeStamp, OutputPath, OutputName, ErrorID)
				SELECT @RunID, @FileID, @ArchRunStateID, ISNULL(MAX(ProcCountNr),0) + 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, NULL, NULL,0
				FROM RT_FILE_LOG 
				WHERE RunID = @RunID AND FileID = @FileID AND RunStateID = @ArchRunStateID
			END

			INSERT INTO HRT_PENDING_STATUS(RunID, FileID, BaseTimeStamp, PendingStateID)	
			SELECT TOP 1 fr.RunID, fr.FileID, ISNULL(fl2.EndTimeStamp,fl1.EndTimeStamp), @RunStateID
			FROM RT_FILE_REGIST fr WITH(NOLOCK)
			INNER JOIN
				RT_FILE_LOG fl1 WITH(NOLOCK)
			ON	fr.RunID = fl1.RunID
				AND fr.FileID = fl1.FileID
				AND fl1.RunStateID = (SELECT RunStateID FROM RD_RUN_STATE WITH(NOLOCK) WHERE RunStateName = 'SOURCE')
				AND fl1.EndTimeStamp is NOT NULL
			LEFT OUTER JOIN
				RT_FILE_LOG fl2 WITH(NOLOCK)
			ON	fr.RunID = fl2.RunID
				AND fr.FileID = fl2.FileID
				AND fl2.RunStateID = (SELECT RunStateID FROM RD_RUN_STATE WITH(NOLOCK) WHERE RunStateName = 'ARCHIVEPCE')
				AND fl2.EndTimeStamp is NOT NULL
			WHERE fr.RunID = @RunID 
				AND fr.FileID = @FileID
			ORDER BY ISNULL(fl2.EndTimeStamp,fl1.EndTimeStamp) DESC
			COMMIT TRANSACTION
		END

		-------------------------------------------------------------------------------------
		--EMAIL
		-------------------------------------------------------------------------------------
		SELECT @RunStateID = RunStateID
		FROM RD_RUN_STATE WITH(NOLOCK)
		WHERE RunStateName='EMLDISKDELETE'
		IF (@RunStateID is NULL)
		BEGIN
			SELECT @errStr = @errStr + ' UnkNown RunState = ''EMLDISKDELETE'''
			--SQL2017-Start
			--RAISERROR 50003 @errStr
			RAISERROR (@errStr, 16, 1)
			--SQL2017-End
			GOTO END_ERROR
		END

		SELECT @ArchRunStateID = RunStateID
		FROM RD_RUN_STATE WITH(NOLOCK)
		WHERE RunStateName = 'EMAIL'
		IF (@ArchRunStateID is NULL)
		BEGIN
			SELECT @errStr = @errStr + ' UnkNown RunState = ''EMAIL'''
			--SQL2017-Start
			--RAISERROR 50003 @errStr
			RAISERROR (@errStr, 16, 1)
			--SQL2017-End
			GOTO END_ERROR
		END		
	
		IF NOT EXISTS (SELECT TOP 1 1 
					FROM HRT_PENDING_STATUS WITH(NOLOCK)
					WHERE RunID = @RunID AND FileID = @FileID 
						AND PendingStateID = @RunStateID)
		BEGIN
			BEGIN TRANSACTION
			IF NOT EXISTS(SELECT TOP 1 1
				FROM
					RT_FILE_REGIST fr WITH(NOLOCK)
				INNER JOIN
					RT_FILE_REGIST frp WITH(NOLOCK)
				ON	fr.RunID = frp.RunID
					AND fr.FileID = frp.ParentFileID
				INNER JOIN
					RT_DOCUMENT d WITH(NOLOCK)
				ON	d.RunID = frp.RunID
					AND d.FileID = frp.FileID
				WHERE fr.RunID = @RunID AND fr.FileID = @FileID
						AND d.ErrorID = 0
						AND d.SuportType & 96 = 96)
			BEGIN
				INSERT INTO RT_FILE_LOG(RunID, FileID, RunStateID, ProcCountNr, StartTimeStamp, EndTimeStamp, OutputPath, OutputName, ErrorID)
				SELECT @RunID, @FileID, @ArchRunStateID,ISNULL(MAX(ProcCountNr),0) + 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, NULL, NULL, 0
				FROM RT_FILE_LOG 
				WHERE RunID = @RunID AND FileID = @FileID AND RunStateID = @ArchRunStateID
			END

			INSERT INTO HRT_PENDING_STATUS(RunID, FileID, BaseTimeStamp, PendingStateID)	
			SELECT TOP 1 fr.RunID, fr.FileID, ISNULL(fl2.EndTimeStamp,fl1.EndTimeStamp), @RunStateID
			FROM RT_FILE_REGIST fr WITH(NOLOCK)
			INNER JOIN
				RT_FILE_LOG fl1 WITH(NOLOCK)
			ON	fr.RunID = fl1.RunID
				AND fr.FileID = fl1.FileID
				AND fl1.RunStateID = (SELECT RunStateID FROM RD_RUN_STATE WITH(NOLOCK) WHERE RunStateName = 'SOURCE')
				AND fl1.EndTimeStamp is NOT NULL
			LEFT OUTER JOIN
				RT_FILE_LOG fl2 WITH(NOLOCK)
			ON	fr.RunID = fl2.RunID
				AND fr.FileID = fl2.FileID
				AND fl2.RunStateID = (SELECT RunStateID FROM RD_RUN_STATE WITH(NOLOCK) WHERE RunStateName = 'EMAILPCE')
				AND fl2.EndTimeStamp is NOT NULL
			WHERE fr.RunID = @RunID 
				AND fr.FileID = @FileID
			ORDER BY ISNULL(fl2.EndTimeStamp,fl1.EndTimeStamp) DESC
			COMMIT TRANSACTION
		END	
		-------------------------------------------------------------------------------------
		--eHTML
		-------------------------------------------------------------------------------------
		SELECT @RunStateID = RunStateID
		FROM RD_RUN_STATE WITH(NOLOCK)
		WHERE RunStateName='EHTMLDISKDELETE'
		IF (@RunStateID is NULL)
		BEGIN
			SELECT @errStr = @errStr + ' UnkNown RunState = ''EHTMLDISKDELETE'''
			--SQL2017-Start
			--RAISERROR 50003 @errStr
			RAISERROR (@errStr, 16, 1)
			--SQL2017-End
			GOTO END_ERROR
		END

		SELECT @ArchRunStateID = RunStateID
		FROM RD_RUN_STATE WITH(NOLOCK)
		WHERE RunStateName = 'EHTMLARCHIVE'
		IF (@ArchRunStateID is NULL)
		BEGIN
			SELECT @errStr = @errStr + ' UnkNown RunState = ''EHTMLARCHIVE'''
			--SQL2017-Start
			--RAISERROR 50003 @errStr
			RAISERROR (@errStr, 16, 1)
			--SQL2017-End
			GOTO END_ERROR
		END		
	
		IF NOT EXISTS (SELECT TOP 1 1 
					FROM HRT_PENDING_STATUS WITH(NOLOCK)
					WHERE RunID = @RunID AND FileID = @FileID 
						AND PendingStateID = @RunStateID)
		BEGIN
			BEGIN TRANSACTION
			IF NOT EXISTS(SELECT TOP 1 1
				FROM
					RT_FILE_REGIST fr WITH(NOLOCK)
				INNER JOIN
					RT_FILE_REGIST frp WITH(NOLOCK)
				ON	fr.RunID = frp.RunID
					AND fr.FileID = frp.ParentFileID
				INNER JOIN
					RT_DOCUMENT d WITH(NOLOCK)
				ON	d.RunID = frp.RunID
					AND d.FileID = frp.FileID
				WHERE fr.RunID = @RunID AND fr.FileID = @FileID
						AND d.ErrorID = 0
						AND d.SuportType & 96 = 96)
			BEGIN
				INSERT INTO RT_FILE_LOG(RunID, FileID, RunStateID, ProcCountNr, StartTimeStamp, EndTimeStamp, OutputPath, OutputName, ErrorID)
				SELECT @RunID, @FileID, @ArchRunStateID,ISNULL(MAX(ProcCountNr),0) + 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, NULL, NULL, 0
				FROM RT_FILE_LOG 
				WHERE RunID = @RunID AND FileID = @FileID AND RunStateID = @ArchRunStateID
			END

			INSERT INTO HRT_PENDING_STATUS(RunID, FileID, BaseTimeStamp, PendingStateID)	
			SELECT TOP 1 fr.RunID, fr.FileID, ISNULL(fl2.EndTimeStamp,fl1.EndTimeStamp), @RunStateID
			FROM RT_FILE_REGIST fr WITH(NOLOCK)
			INNER JOIN
				RT_FILE_LOG fl1 WITH(NOLOCK)
			ON	fr.RunID = fl1.RunID
				AND fr.FileID = fl1.FileID
				AND fl1.RunStateID = (SELECT RunStateID FROM RD_RUN_STATE WITH(NOLOCK) WHERE RunStateName = 'SOURCE')
				AND fl1.EndTimeStamp is NOT NULL
			LEFT OUTER JOIN
				RT_FILE_LOG fl2 WITH(NOLOCK)
			ON	fr.RunID = fl2.RunID
				AND fr.FileID = fl2.FileID
				AND fl2.RunStateID = (SELECT RunStateID FROM RD_RUN_STATE WITH(NOLOCK) WHERE RunStateName = 'EHTMLPREP')
				AND fl2.EndTimeStamp is NOT NULL
			WHERE fr.RunID = @RunID 
				AND fr.FileID = @FileID
			ORDER BY ISNULL(fl2.EndTimeStamp,fl1.EndTimeStamp) DESC
			COMMIT TRANSACTION
		END
	END
	
	COMMIT TRANSACTION
GOTO END_SP
END_ERROR:
	ROLLBACK TRANSACTION
END_SP:
END_PROC:
GO
