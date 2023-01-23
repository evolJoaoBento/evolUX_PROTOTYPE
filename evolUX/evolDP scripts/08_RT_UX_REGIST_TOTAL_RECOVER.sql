IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RT_UX_REGIST_TOTAL_RECOVER]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RT_UX_REGIST_TOTAL_RECOVER] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RT_UX_REGIST_TOTAL_RECOVER]
	@FileBarcode varchar(20),
	@UserName varchar(50),
	@ServiceCompanyList IDList READONLY,
	@PermissionLevel bit = 0
--WITH ENCRYPTION
AS
	SET NOCOUNT ON

	DECLARE @RunID int,
		@FileID int,
		@Code int,
		@RunStateID int

	IF LEN(@FileBarcode) <> 20
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -1 ErrorID, 'FileInvalidBarcodeSize' Error
		RETURN -1
	END

	SET @Code = CAST(SUBSTRING(@FileBarcode, 1, 2) as int)
	SET @RunID = CAST(SUBSTRING(@FileBarcode, 3, 10) as int)
	SET @FileID = CAST(SUBSTRING(@FileBarcode, 13, 8) as int)
	IF @RunID = 0
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -2 ErrorID, 'FileInvalidBarcode' Error
		RETURN -2
	END
	IF @FileID = 0
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -3 ErrorID, 'FileInvalidBarcode' Error
		RETURN -3
	END
	IF @Code <> 1
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -4 ErrorID, 'FileInvalidBarcode' Error
		RETURN -4
	END
	IF (NOT EXISTS(SELECT TOP 1 1 FROM RT_FILE_REGIST 
			WHERE FileID = @FileID
				AND RunID = @RunID))
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -5 ErrorID, 'FileNotFound' Error
		RETURN -5
	END
	ELSE
	BEGIN
		IF (@PermissionLevel = 0 
			AND EXISTS(SELECT TOP 1 1 
			FROM RT_FILE_LOG WITH(NOLOCK) 
			WHERE RunID = @RunID AND FileID = @FileID 
				AND RunStateID = (SELECT RunStateID 
							FROM RD_RUN_STATE WITH(NOLOCK)
							WHERE RunStateName = 'EXPEDITION')))
		BEGIN
			SELECT -12 ErrorID, 'InvalidRecoverAlreadyDispached' Error
			RETURN -12
		END
	END

	IF (NOT EXISTS (SELECT TOP 1 1
		FROM RT_FILE_LOG WITH(NOLOCK) 
		WHERE RunID = @RunID AND FileID = @FileID 
			AND RunStateID = (SELECT RunStateID 
						FROM RD_RUN_STATE WITH(NOLOCK)
						WHERE RunStateName = 'TOPRINT'))
		OR				
		EXISTS(SELECT * 
		FROM RT_FILE_LOG WITH(NOLOCK) 
		WHERE RunID = @RunID AND FileID = @FileID 
			AND RunStateID = (SELECT RunStateID 
						FROM RD_RUN_STATE WITH(NOLOCK)
						WHERE RunStateName = 'FCSDISKDELETE'))
		)
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -13 ErrorID, 'InvalidRecoverAlreadyPurged' Error
		return -13
	END

	IF NOT EXISTS(SELECT * 
			FROM RT_FILE_REGIST f WITH(NOLOCK),
				RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
			WHERE f.FileID=@FileID
			AND f.RunID=@RunID
			AND f.ProdDetailID = pd.ProdDetailID
			AND pd.ServiceCompanyID in (SELECT ID
					FROM @ServiceCompanyList))
	BEGIN
		SELECT -8 ErrorID, 'FileNotFoundInServiceCompany' Error
		return -8
	END
	SELECT @RunStateID = RunStateID
	FROM RT_FILE_REGIST
	WHERE RunID = @RunID
		AND FileID = @FileID
	IF (@RunStateID <> (SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'FORK'))
	BEGIN
		SELECT -14 ErrorID, 'InvalidTotalRecover4RecoveredFiles' Error
		return -14
	END
	BEGIN TRANSACTION
	INSERT INTO RT_RECOVER_REGIST(RunID, FileID, RequestID, StartPostObjID, EndPostObjID, Reason, UserName, RegistrationTimeStamp, RecNumber, RecStartTime, RecEndTime)
	SELECT @RunID, @FileID, ISNULL(MAX(RequestID),0)+1, 0, 0, NULL, @UserName, CURRENT_TIMESTAMP, NULL, NULL, NULL
	FROM RT_RECOVER_REGIST
	WHERE RunID = @RunID AND FileID = @FileID

	UPDATE RT_POSTAL_OBJECT
	SET ToRecover = 1
	WHERE PostObjRunID = @RunID AND PostObjFileID = @FileID

	SELECT 0 ErrorID, 'Success' Error

	COMMIT TRANSACTION
RETURN 0
GO


