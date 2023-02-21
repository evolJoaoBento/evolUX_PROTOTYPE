ALTER  PROCEDURE [dbo].[RD_NEW_DOCCODE_CONFIG]
	@DocLayout varchar(20) = NULL,
	@DocType varchar(8) = NULL,
	@StartDate int, 
	@AggrCompatibility tinyint = 0,
	@EnvMediaID int, 
	@ExpeditionType int, 
	@ExpCompanyID int = NULL, 
	@ServiceTaskID int = NULL,
	@SuportType tinyint = 1,
	@Priority int = 0, 
	@AgingDays smallint = 0, 
	@CaducityDate varchar(50) = '31/12/9999', 
	@MaxProdDate varchar(50) = 'dd/mm/yyyy', 
	@ProdMaxSheets smallint = NULL, 
	@ExceptionLevel1ID int = NULL,
	@ExceptionLevel2ID int = NULL,
	@ExceptionLevel3ID int = NULL,
	@Description varchar(256) = 'AutoRegist',
	@ArchCaducityDate varchar(50) = '31/12/9999',
	@PrintMatchCode varchar(10) = '',
	@DocCodeID int = NULL,
	@ExpCode varchar(10) = NULL
--WITH ENCRYPTION
AS
	SET NOCOUNT ON
	BEGIN TRANSACTION
	DECLARE
		@ErrorNumber int,
		@ErrorMsg varchar(255)

	IF (@DocCodeID is NULL)
	BEGIN
		IF (@DocLayout is NULL OR @DocType is NULL)
		BEGIN
				SELECT @ErrorNumber = 16, @ErrorMsg = 'Missing DocLayout/DocType parameters'
				GOTO ErrorHandler
		END
	END
	IF (@ExpCode is NULL)
	BEGIN
		IF (@ExpCompanyID is NULL OR @ServiceTaskID is NULL)
		BEGIN
				SELECT @ErrorNumber = 16, @ErrorMsg = 'Missing ExpCompanyID/ServiceTaskID parameters'
				GOTO ErrorHandler
		END
	END

	IF (@DocCodeID is NULL)
	BEGIN
		SELECT @DocLayout = REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(@DocLayout)),CHAR(9),''),CHAR(10),''),CHAR(13),'')
		SELECT @DocType = REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(@DocType)),CHAR(9),''),CHAR(10),''),CHAR(13),'')

		EXEC RD_REGIST_DOCCODE @DocLayout, @DocType, @ExceptionLevel1ID, @ExceptionLevel2ID, @ExceptionLevel3ID, @Description, @PrintMatchCode

		SELECT @DocCodeID = DocCodeID
		FROM RD_DOCCODE WITH(NOLOCK)
		WHERE DocLayout = @DocLayout AND DocType = @DocType
			AND ISNULL(@ExceptionLevel1ID,-1) = ISNULL(ExceptionLevel1ID,-1)
			AND ISNULL(@ExceptionLevel2ID,-1) = ISNULL(ExceptionLevel2ID,-1)
			AND ISNULL(@ExceptionLevel3ID,-1) = ISNULL(ExceptionLevel3ID,-1)
	END
	ELSE
	BEGIN
		UPDATE RD_DOCCODE
		SET [Description] = @Description, PrintMatchCode = @PrintMatchCode
		WHERE DocCodeID = @DocCodeID
	END

	IF (@ExpCode is NULL)
	BEGIN
		SELECT @ExpCode = ExpCode
		FROM RD_EXPCOMPANY_SERVICE_TASK WITH(NOLOCK)
		WHERE ExpCompanyID = @ExpCompanyID AND ServiceTaskID = @ServiceTaskID

		IF (@ExpCode is NULL)
		BEGIN
			IF (@SuportType & 1 = 1)
			BEGIN
				DECLARE @CompanyName varchar(256)
				SELECT @CompanyName = CompanyName
				FROM RD_COMPANY WITH(NOLOCK)
				WHERE CompanyID = @ExpCompanyID

				DECLARE @ServiceTaskCode char(3)
				SELECT @ServiceTaskCode = ServiceTaskCode
				FROM RD_SERVICE_TASK WITH(NOLOCK)
				WHERE ServiceTaskID = @ServiceTaskID
				
				SELECT @ErrorNumber = 16, @ErrorMsg = 'ExpCode [Combination of ExpCompany with ServiceTask] not registred for ExpCompany = ' + @CompanyName + ' and ServiceTaskCode = ' + @ServiceTaskCode
				GOTO ErrorHandler
			END
			ELSE
			BEGIN
				SELECT TOP 1 @ExpCode = ExpCode
				FROM RD_EXPCOMPANY_SERVICE_TASK WITH(NOLOCK)
			ORDER BY ExpCompanyID DESC, ServiceTaskID ASC
			END
		END
	END

	IF (@MaxProdDate = 'dd/mm/yyyy')
	BEGIN
		SET @AgingDays = 0
	END
	ELSE
	BEGIN
		SET @AgingDays = 9999
	END

	IF (EXISTS (SELECT TOP 1 1 FROM RD_DOCCODE_CONFIG WITH(NOLOCK) WHERE DocCodeID = @DocCodeID AND StartDate = @StartDate))
	BEGIN
		UPDATE RD_DOCCODE_CONFIG
		SET AggrCompatibility = @AggrCompatibility, 
			EnvMediaID = @EnvMediaID, 
			ExpeditionType = @ExpeditionType, 
			ExpCode = @ExpCode, 
			SuportType = @SuportType, 
			[Priority] = @Priority,  
			AgingDays = @AgingDays, 
			CaducityDate = @CaducityDate, 
			ArchCaducityDate = @ArchCaducityDate, 
			MaxProdDate = @MaxProdDate, 
			ProdMaxSheets = @ProdMaxSheets
		WHERE DocCodeID = @DocCodeID AND StartDate = @StartDate
	END
	ELSE
	BEGIN
		INSERT INTO RD_DOCCODE_CONFIG(DocCodeID, StartDate, AggrCompatibility, EnvMediaID, ExpeditionType, ExpCode, SuportType, [Priority], AgingDays, CaducityDate, MaxProdDate, ProdMaxSheets,ArchCaducityDate)
		SELECT @DocCodeID, @StartDate, @AggrCompatibility, @EnvMediaID, @ExpeditionType, @ExpCode, @SuportType, @Priority,  @AgingDays, @CaducityDate, @MaxProdDate, @ProdMaxSheets,@ArchCaducityDate
	END
	COMMIT TRANSACTION
	SET NOCOUNT OFF
RETURN
ErrorHandler:
	RAISERROR (@ErrorMsg, 16, 1)
	ROLLBACK TRANSACTION
GO
