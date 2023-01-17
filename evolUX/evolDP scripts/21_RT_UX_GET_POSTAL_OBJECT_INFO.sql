IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RT_UX_GET_POSTAL_OBJECT_INFO]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RT_UX_GET_POSTAL_OBJECT_INFO] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RT_UX_GET_POSTAL_OBJECT_INFO]
	@PostObjBarcode varchar(20),
	@ServiceCompanyList IDList READONLY
--WITH ENCRYPTION
AS
	SET NOCOUNT ON
	DECLARE @RetStr varchar(5000)
	DECLARE @SQLstr nvarchar(4000)
	
	DECLARE @PostObjRunID int,
		@PostObjID int,
		@PostObjFileID int,
		@Code int,
		@SeqNum int,
		@SheetSeqNum int,
		@FileName varchar(256)

	SELECT @PostObjRunID = 0, @PostObjFileID = 0, @PostObjID = 0

	IF Len(@PostObjBarcode) <> 20 and  Len(@PostObjBarcode) <> 1
	BEGIN
		SELECT 'PostObjInvalidBarcodeSize' Error, @PostObjRunID PostObjRunID, @PostObjFileID PostObjFileID, @PostObjID PostObjID, '' [FileName], 0 [SequenceNumber], 0 [FirstSheetSequenceNumber]
		RETURN
	END

	SET @Code = cast(SUBSTRING(@PostObjBarcode, 1, 2) as int)
	IF @Code <> 2
	BEGIN
		SELECT 'PostObjInvalidBarcode' Error, @PostObjRunID PostObjRunID, @PostObjFileID PostObjFileID, @PostObjID PostObjID, '' [FileName], 0 [SequenceNumber], 0 [FirstSheetSequenceNumber]
		RETURN
	END

	SET @PostObjRunID = CAST(SUBSTRING(@PostObjBarcode, 3, 10) as int)
	SET @PostObjID = CAST(SUBSTRING(@PostObjBarcode, 13, 8) as int)
		
	SELECT @PostObjFileID = PostObjFileID, @SeqNum = PostObjFileSeq 
	FROM RT_POSTAL_OBJECT WITH(NOLOCK)
	WHERE PostObjRunID = @PostObjRunID AND PostObjID = @PostObjID

	IF (@@ROWCOUNT > 0)
	BEGIN
		IF NOT EXISTS(SELECT TOP 1 1
				FROM RT_POSTAL_OBJECT po WITH(NOLOCK)
				INNER JOIN
					RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
				ON	po.ProdDetailID = pd.ProdDetailID
				WHERE po.PostObjID = @PostObjID
				AND po.PostObjRunID = @PostObjRunID
				AND pd.ServiceCompanyID in (SELECT ID
											FROM @ServiceCompanyList))
		BEGIN
			SELECT 'PostObjNotFoundInServiceCompany' Error, @PostObjRunID PostObjRunID, @PostObjFileID PostObjFileID, @PostObjID PostObjID, '' [FileName], 0 [SequenceNumber], 0 [FirstSheetSequenceNumber]
			RETURN
		END
	END
	ELSE
	BEGIN
		SELECT 'PostObjNotFound' Error, @PostObjRunID PostObjRunID, @PostObjFileID PostObjFileID, @PostObjID PostObjID, '' [FileName], 0 [SequenceNumber], 0 [FirstSheetSequenceNumber]
		RETURN
	END

	SELECT @SheetSeqNum = SUM(d.Quantity) + 1
	FROM RT_POSTAL_OBJECT po WITH(NOLOCK)
	INNER JOIN
		RT_DOCUMENT_SET ds WITH(NOLOCK)
	ON	ds.PostObjRunID = po.PostObjRunID
		AND ds.PostObjID = po.PostObjID
	INNER JOIN
		RT_DOCUMENT_PRINT_MATERIAL d WITH(NOLOCK)
	ON	d.RunID = ds.RunID
		AND d.FileID = ds.FileID
		AND d.SetID = ds.SetID
	WHERE po.PostObjRunID = @PostObjRunID
		AND po.PostObjFileID = @PostObjFileID
		AND po.PostObjFileSeq < @SeqNum
		
	IF (@SheetSeqNum is NULL)
		SELECT @SheetSeqNum = 1

	SELECT @FileName = fl.[OutputName]
	FROM RT_FILE_LOG fl WITH(NOLOCK)
	WHERE fl.RunID = @PostObjRunID
		AND fl.FileID = @PostObjFileID
		AND fl.RunStateID = (SELECT RunStateID 
					FROM RD_RUN_STATE 
					WHERE RunStateName = 'TOPRINT')
		AND fl.ProcCountNr = (SELECT MAX(ProcCountNr)
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = fl.RunID
						AND FileID = fl.FileID
						AND RunStateID = fl.RunStateID
						AND ErrorID = 0
						AND EndTimeStamp is NOT NULL)

	IF (@FileName is NULL)
	BEGIN
		SELECT 'PostObjNotFound' Error, @PostObjRunID PostObjRunID, @PostObjFileID PostObjFileID, @PostObjID PostObjID, '' [FileName], 0 [SequenceNumber], 0 [FirstSheetSequenceNumber]
	END
	ELSE
	BEGIN
		SELECT 'Success' Error, @PostObjRunID PostObjRunID, @PostObjFileID PostObjFileID, @PostObjID PostObjID,
			@FileName [FileName], --Nome do Ficheiro de Produção
			@SeqNum [SequenceNumber], --Nº de Sequência dentro do ficheiro
			@SheetSeqNum [FirstSheetSequenceNumber] --Nº Sequencial da 1ª Folha do Objecto Postal
	END
RETURN