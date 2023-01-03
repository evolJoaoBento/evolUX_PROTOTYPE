IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RT_UX_REGIST_PRINT]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RT_UX_REGIST_PRINT] AS' 
END
GO
ALTER PROCEDURE [dbo].[RT_UX_REGIST_PRINT] 
	@FileBarcode varchar(20),
	@UserName varchar(50),
	@ServiceCompanyList IDList READONLY
--WITH ENCRYPTION
AS
	SET NOCOUNT ON

	DECLARE @RunID int,
		@FileID int,
		@Code int,
		@RunStateID int

	BEGIN TRANSACTION
	IF LEN(@FileBarcode) <> 20
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -1 ResultID, 'Código de Barras de Ficheiro Inválido!' ResultStr
		RETURN -1
	END
	SET @Code = CAST(SUBSTRING(@FileBarcode, 1, 2) as int)
	SET @RunID = CAST(SUBSTRING(@FileBarcode, 3, 10) as int)
	SET @FileID = CAST(SUBSTRING(@FileBarcode, 13, 8) as int)
	IF @RunID = 0
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -2 ResultID, 'Erro na leitura do Código de Barras: RunID Inválido!' ResultStr
		RETURN -2
	END
	IF @FileID = 0
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -3 ResultID, 'Erro na leitura do Código de Barras: FileID Inválido!' ResultStr
		RETURN -3
	END
	IF @Code <> 1
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -4 ResultID, 'Código de Barras Inválido: Não é um o Código de Barras de Ficheiro!' ResultStr
		RETURN -4
	END
	IF (NOT EXISTS(SELECT TOP 1 1 FROM RT_FILE_REGIST 
			WHERE FileID = @FileID
				AND RunID = @RunID))
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -5 ResultID, 'O Ficheiro não se encontra registado na evolDP!' ResultStr
		RETURN -5
	END
	IF NOT EXISTS(SELECT TOP 1 1 FROM RT_FILE_REGIST 
			WHERE FileID = @FileID AND RunID = @RunID AND ErrorID = 0)
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -9 ResultID, 'O Ficheiro está marcado com erro na evolDP, não pode ser dado por impresso!' ResultStr
		RETURN -9
	END
	IF (NOT EXISTS(SELECT TOP 1 1
			FROM RT_FILE_REGIST f WITH(NOLOCK)
			INNER JOIN
				RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
			ON f.ProdDetailID = pd.ProdDetailID
			WHERE f.FileID = @FileID
				AND f.RunID = @RunID
				AND pd.ServiceCompanyID in (SELECT ID
					FROM @ServiceCompanyList)))
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -8 ResultID, 'O Ficheiro não se encontra registado na evolDP para o respetivo Service Provider!' ResultStr
		RETURN -8
	END
	SELECT @RunStateID = RunStateID FROM RD_RUN_STATE WITH(NOLOCK) WHERE RunStateName = 'SEND2PRINTER'
	IF (NOT EXISTS(SELECT TOP 1 1 FROM RT_FILE_LOG 
				WHERE RunID = @RunID 
				AND FileID = @FileID 
				AND RunStateID = @RunStateID
				AND ErrorID = 0))
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -6 ResultID, 'O Ficheiro ainda não foi enviado para uma Impressora!' ResultStr
		RETURN -6
	END
	IF (NOT EXISTS(SELECT TOP 1 1 FROM RT_FILE_LOG 
			WHERE RunID = @RunID 
				AND FileID = @FileID 
				AND RunStateID = @RunStateID
				AND EndTimeStamp is NOT NULL
				AND ErrorID = 0))
	BEGIN
		UPDATE RT_FILE_LOG 
		SET EndTimeStamp = CURRENT_TIMESTAMP
		WHERE RunID = @RunID AND FileID = @FileID AND RunStateID = @RunStateID AND ErrorID = 0
	END
	SELECT @RunStateID = RunStateID FROM RD_RUN_STATE WITH(NOLOCK) WHERE RunStateName = 'PRINTED'
	IF (NOT EXISTS(SELECT TOP 1 1 FROM RT_FILE_LOG 
			WHERE RunID = @RunID 
				AND FileID = @FileID 
				AND RunStateID = @RunStateID
				AND EndTimeStamp is NOT NULL
				AND ErrorID = 0))
	BEGIN
		INSERT INTO RT_FILE_LOG(RunID, FileID, RunStateID, ProcCountNr, StartTimeStamp, EndTimeStamp, OutputPath, OutputName, ErrorID)
		SELECT TOP 1 @RunID, @FileID, @RunStateID, ISNULL(MAX(ProcCountNr),0)+1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, NULL, @UserName, 0
		FROM RT_FILE_LOG
		WHERE RunID = @RunID 
			AND FileID = @FileID 
			AND RunStateID = @RunStateID
	END
	ELSE
	BEGIN
		ROLLBACK TRANSACTION
		SELECT -7 ResultID, 'O Ficheiro já foi dado por impresso!' ResultStr
		RETURN -7
	END
	COMMIT TRANSACTION
	SELECT 0 ResultID,  'Registo Efectuado!' ResultStr
RETURN 0
GO