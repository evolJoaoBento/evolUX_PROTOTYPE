IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RT_UX_REGIST_PARCIAL_RECOVER]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RT_UX_REGIST_PARCIAL_RECOVER] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RT_UX_REGIST_PARCIAL_RECOVER]
	@StartBarcode varchar(20),
	@EndBarcode varchar(20),
	@UserName varchar(50),
	@ServiceCompanyList IDList READONLY,
	@PermissionLevel bit = 0
--WITH ENCRYPTION
AS
	SET NOCOUNT ON
	DECLARE @RetStr varchar(5000)

	BEGIN TRANSACTION
	DECLARE @StartRunID int,
		@StartPostObjID int,
		@EndRunID int,
		@EndPostObjID int,
		@FileID int,
		@RunID int,
		@Code int,
		@StartSeqNum int,
		@EndSeqNum int

	IF (LEN(@StartBarcode) <> 20 AND LEN(@StartBarcode) <> 1)
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -11 ErrorID, 'FirstPostObjInvalidBarcodeSize' Error
		RETURN -11
	END

	IF (LEN(@EndBarcode) <> 20 AND LEN(@EndBarcode) <> 1)
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -1 ErrorID, 'LastPostObjInvalidBarcodeSize' Error
		RETURN -1
	END

	IF @StartBarcode = '0' and @EndBarcode = '0'
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -4 ErrorID, 'InvalidParcialRecoverBarCode' Error
		RETURN -4
	END

	IF (@StartBarcode = '0')
	BEGIN
		SET @Code = CAST(SUBSTRING(@EndBarcode, 1, 2) as int)
		IF @Code <> 2
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -2 ErrorID, 'LastPostObjInvalidBarcode' Error
			RETURN -2
		END
		SET @EndRunID = CAST(SUBSTRING(@EndBarcode, 3, 10) as int)
		SET @EndPostObjID = CAST(SUBSTRING(@EndBarcode, 13, 8) as int)
		
		SET @RunID = @EndRunID
		
		IF NOT EXISTS(SELECT TOP 1 1
				FROM RT_POSTAL_OBJECT po WITH(NOLOCK),
					RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
				WHERE po.PostObjID = @EndPostObjID
				AND po.PostObjRunID = @RunID
				AND po.ProdDetailID = pd.ProdDetailID
				AND pd.ServiceCompanyID in (SELECT ID
					FROM @ServiceCompanyList))
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -3 ErrorID, 'LastPostObjNotFoundInServiceCompany' Error
			RETURN -3
		END
		SELECT @FileID = PostObjFileID, @EndSeqNum = PostObjFileSeq 
		FROM RT_POSTAL_OBJECT
		WHERE PostObjRunID = @RunID AND PostObjID = @EndPostObjID
		
		SELECT TOP 1 @StartPostObjID = PostObjID, @StartSeqNum = PostObjFileSeq
		FROM  RT_POSTAL_OBJECT
		WHERE PostObjRunID = @RunID AND PostObjFileID = @FileID
		ORDER BY StartPosition
	END

	IF (@EndBarcode = '0')
	BEGIN
		SET @Code = cast(SUBSTRING(@StartBarcode, 1, 2) as int)
		IF @Code <> 2
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -12 ErrorID, 'FirstPostObjInvalidBarcode' Error
			RETURN -12
		END
		SET @StartRunID = cast(SUBSTRING(@StartBarcode, 3, 10) as int)
		SET @StartPostObjID = cast(SUBSTRING(@StartBarcode, 13, 8) as int)
		SET @RunID = @StartRunID
		
		IF NOT EXISTS(SELECT * 
				FROM RT_POSTAL_OBJECT po WITH(NOLOCK),
					RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
				WHERE po.PostObjID = @StartPostObjID
				AND po.PostObjRunID = @RunID
				AND po.ProdDetailID = pd.ProdDetailID
				AND pd.ServiceCompanyID in (SELECT ID
					FROM @ServiceCompanyList))
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -13 ErrorID, 'FirstPostObjNotFoundInServiceCompany' Error
			RETURN -13
		END
		SELECT @FileID = PostObjFileID, @StartSeqNum = PostObjFileSeq 
		FROM RT_POSTAL_OBJECT
		WHERE PostObjRunID = @RunID AND PostObjID = @StartPostObjID
		SELECT TOP 1 @EndPostObjID = PostObjID, @EndSeqNum = PostObjFileSeq
		FROM  RT_POSTAL_OBJECT
		WHERE PostObjRunID = @RunID AND PostObjFileID = @FileID
		ORDER BY EndPosition DESC
	END

	IF (@StartBarcode <> '0' AND @EndBarcode <> '0')
	BEGIN
		SET @Code = cast(SUBSTRING(@StartBarcode, 1, 2) as int)
		IF @Code <> 2
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -12 ErrorID, 'FirstPostObjInvalidBarcode' Error
			RETURN -12
		END
		SET @StartRunID = cast(SUBSTRING(@StartBarcode, 3, 10) as int)
		SET @StartPostObjID = cast(SUBSTRING(@StartBarcode, 13, 8) as int)
		IF NOT EXISTS(SELECT * 
				FROM RT_POSTAL_OBJECT po WITH(NOLOCK),
					RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
				WHERE po.PostObjID = @StartPostObjID
				AND po.PostObjRunID = @StartRunID
				AND po.ProdDetailID = pd.ProdDetailID
				AND pd.ServiceCompanyID in (SELECT ID
					FROM @ServiceCompanyList))
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -13 ErrorID, 'FirstPostObjNotFoundInServiceCompany' Error
			RETURN -13
		END
		SET @Code = cast(SUBSTRING(@EndBarcode, 1, 2) as int)
		IF @Code <> 2
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -2 ErrorID, 'LastPostObjInvalidBarcode' Error
			RETURN -2
		END
		SET @EndRunID = cast(SUBSTRING(@EndBarcode, 3, 10) as int)
		SET @EndPostObjID = cast(SUBSTRING(@EndBarcode, 13, 8) as int)
		
		IF NOT EXISTS(SELECT * 
				FROM RT_POSTAL_OBJECT po WITH(NOLOCK),
					RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
				WHERE po.PostObjID = @EndPostObjID
				AND po.PostObjRunID = @EndRunID
				AND po.ProdDetailID = pd.ProdDetailID
				AND pd.ServiceCompanyID in (SELECT ID
					FROM @ServiceCompanyList))
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -3 ErrorID, 'LastPostObjNotFoundInServiceCompany' Error
			RETURN -3
		END
		
		IF @StartRunID <> @EndRunID 
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -5 ErrorID, 'InvalidParcialRecoverDifferentProductionRun' Error
			RETURN -5
		END
		ELSE SET @RunID = @StartRunID

		DECLARE @SecondFileID int
		
		SELECT @FileID = PostObjFileID, @StartSeqNum = PostObjFileSeq
		FROM RT_POSTAL_OBJECT
		WHERE PostObjRunID = @EndRunID AND PostObjID = @StartPostObjID
		
		SELECT @SecondFileID = PostObjFileID, @EndSeqNum = PostObjFileSeq 
		FROM RT_POSTAL_OBJECT
		WHERE PostObjRunID = @EndRunID AND PostObjID = @EndPostObjID
		
		IF @FileID <> @SecondFileID 
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -6 ErrorID, 'InvalidParcialRecoverDifferentProductionFile' Error
			RETURN -6
		END
	END
	IF (@StartSeqNum>@EndSeqNum)
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -7 ErrorID, 'InvalidParcialRecoverReversedOrder' Error
		RETURN -7
	END
	IF (@PermissionLevel = 0 
		AND EXISTS(SELECT * 
		FROM RT_FILE_LOG WITH(NOLOCK) 
		WHERE RunID = @RunID AND FileID = @FileID 
			AND RunStateID = (SELECT RunStateID 
						FROM RD_RUN_STATE
						WHERE RunStateName = 'EXPEDITION')))
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -8 ErrorID, '!' Error
		RETURN -8
	END

	IF (NOT EXISTS (SELECT * 
		FROM RT_FILE_LOG WITH(NOLOCK) 
		WHERE RunID = @RunID AND FileID = @FileID 
			AND RunStateID = (SELECT RunStateID 
						FROM RD_RUN_STATE
						WHERE RunStateName = 'TOPRINT'))
		OR				
		EXISTS(SELECT * 
		FROM RT_FILE_LOG WITH(NOLOCK) 
		WHERE RunID = @RunID AND FileID = @FileID 
			AND RunStateID = (SELECT RunStateID 
						FROM RD_RUN_STATE
						WHERE RunStateName = 'FCSDISKDELETE'))
		)
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -9 ErrorID, 'InvalidRecoverAlreadyPurged' Error
		RETURN -9
	END

	INSERT INTO RT_RECOVER_REGIST(RunID, FileID, RequestID, StartPostObjID, EndPostObjID, Reason, UserName, RegistrationTimeStamp, RecNumber, RecStartTime, RecEndTime)
	SELECT @RunID, @FileID, ISNULL(MAX(RequestID),0)+1, @StartPostObjID, @EndPostObjID, NULL, @UserName, CURRENT_TIMESTAMP, NULL, NULL, NULL
	FROM RT_RECOVER_REGIST
	WHERE RunID = @RunID AND FileID = @FileID
	IF (@@Error <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -101 ErrorID, 'NotSuccess' Error
		RETURN -101
	END

	UPDATE RT_POSTAL_OBJECT
	SET ToRecover = 1
	WHERE PostObjRunID = @RunID AND PostObjFileID = @FileID
		AND PostObjFileSeq between  @StartSeqNum AND @EndSeqNum
	IF (@@Error <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -102 ErrorID, 'NotSuccess' Error
		RETURN -102
	END
	SELECT 0 ErrorID, 'Success' Error
	COMMIT TRANSACTION
RETURN 0
GO