IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_TASKS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASKS] AS' 
END
GO

ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASKS]
	@ServiceTaskID int = NULL
AS
BEGIN
	SELECT ServiceTaskID, ServiceTaskCode, [Description] ServiceTaskDesc, StationExceededDesc, ComplementServiceTaskID, ExternalExpeditionMode
	FROM RD_SERVICE_TASK WITH(NOLOCK)
	WHERE (@ServiceTaskID is NULL OR ServiceTaskID = @ServiceTaskID)
	ORDER BY ServiceTaskID
END
GO
--ALTERAR, APAGAR e ALTERAR (Adicionando novos tipos de serviços)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_TASK_SERVICE_TYPES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASK_SERVICE_TYPES] AS' 
END
GO
--Lista serviços associados ao tipo de tratamento
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASK_SERVICE_TYPES]
	@ServiceTaskID int
AS
BEGIN
	SELECT st.ServiceTypeID,
		st.ServiceTypeCode, --Código de Tipo de Serviço
		st.ServiceTypeDescription ServiceTypeDesc --Descrição do Tipo de Serviço
	FROM
		RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
	INNER JOIN
		RD_SERVICE_TYPE st
	ON	st.ServiceTypeID = stst.ServiceTypeID
	WHERE stst.ServiceTaskID = @ServiceTaskID
	ORDER BY st.ServiceTypeID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_SERVICE_TASKS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_SERVICE_TASKS] AS' 
END
GO
-- Listar Tipo Tratamento/Expedição (ExpCode) - Alterar/apagar/adiconar?
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_SERVICE_TASKS]
	@ServiceTaskID int = NULL,
	@ExpCompanyID int = NULL,
	@ExpCode varchar(10) = NULL
AS
BEGIN
	SELECT e.ExpCode, --Código Tratamento/Expedição
		e.[Description] ExpCodeDesc,
		est.ExpCompanyID,
		c.CompanyCode ExpCompanyCode, --Companhia de Expedição
		est.ServiceTaskID,
		st.ServiceTaskCode,
		st.[Description] ServiceTaskDesc,
		e.[DefaultExpCenterCode],
		e.[DefaultExpCompanyZone],
		e.[Priority],
		e.CheckExpCompanySepCodes, --Validação Integral do Código de Separação ==> 0 = 'Desativada' ELSE 'Ativada'
		e.PostalCodeStart --Posição inicial do CP4, no Código de Separação ==> NULL, não aplicável
	FROM
		RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK)
	ON	est.ExpCompanyID = c.CompanyID
	INNER JOIN
		RD_SERVICE_TASK st WITH(NOLOCK)
	ON	est.ServiceTaskID = st.ServiceTaskID
	INNER JOIN
		RD_EXPCODE e
	ON	e.ExpCode = est.ExpCode
	WHERE (@ServiceTaskID is NULL OR est.ServiceTaskID = @ServiceTaskID)
		AND (@ExpCompanyID is NULL OR est.ExpCompanyID = @ExpCompanyID)
		AND (@ExpCode is NULL OR est.ExpCode = @ExpCode)
	ORDER BY est.ExpCompanyID, e.[Priority] DESC
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCODE_EXPCENTER_SELECTIONS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCODE_EXPCENTER_SELECTIONS] AS' 
END
GO
--Carateristicas do Tipo Tratamento/expedição
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCODE_EXPCENTER_SELECTIONS]
	@ExpCode varchar(10),
	@ServiceCompanyList IDList READONLY
AS
BEGIN
	SELECT 
		eee.ExpCode,
		eee.ExpCenterCode,
		eee.ServiceCompanyID,
		eee.ExpeditionZone,
		ez.[Description] ExpeditionZoneDesc,
		eee.Description1,
		eee.Description2,
		eee.Description3
	FROM
		RD_EXPEDITION_EXPCENTER_EXPZONE eee WITH(NOLOCK)
	INNER JOIN
		@ServiceCompanyList c
	ON	eee.ServiceCompanyID = c.ID
	INNER JOIN
		RD_EXPEDITION_ZONE ez WITH(NOLOCK)
	ON	eee.ExpeditionZone = ez.ExpeditionZone
	WHERE eee.ExpCode = @ExpCode
	ORDER BY eee.ExpCenterCode
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_SERVICE_TASK]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_SERVICE_TASK] AS' 
END
GO

ALTER  PROCEDURE [dbo].[RD_UX_SET_SERVICE_TASK]
	@ServiceTaskCode varchar(4),
	@ServiceTaskDesc varchar(256),
	@ServiceTaskID int = NULL,
	@RefServiceTaskID int = NULL,
	@ComplementServiceTaskID int = NULL,
	@ExternalExpeditionMode tinyint = NULL,
	@StationExceededDesc varchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON
	IF (@ServiceTaskID is NULL OR @ServiceTaskID = 0)
	BEGIN
		IF (@RefServiceTaskID is NULL OR @RefServiceTaskID = 0
			OR NOT EXISTS(SELECT TOP 1 1 FROM RD_SERVICE_TASK WHERE ServiceTaskID = @RefServiceTaskID))
		BEGIN
			SELECT -1 ErrorID, 'MissingRefServiceTaskID' Error
			RETURN -1
		END
		IF (@ServiceTaskCode is NULL)
		BEGIN
			SELECT -2 ErrorID, 'MissingServiceTaskCode' Error
			RETURN -2
		END
		IF (EXISTS(SELECT TOP 1 1 FROM RD_SERVICE_TASK WHERE ServiceTaskCode = @ServiceTaskCode))
		BEGIN
			SELECT -3 ErrorID, 'ServiceTaskCodeAlreadyExists' Error
			RETURN -3
		END

		BEGIN TRANSACTION	
		DECLARE @ExpCompanyID int,
			@ExpCode varchar(10),
			@NewExpCode varchar(10)

		INSERT INTO RD_SERVICE_TASK(ServiceTaskID, ServiceTaskCode, [Description], StationExceededDesc, ComplementServiceTaskID, ExternalExpeditionMode)
		SELECT (SELECT ISNULL(MAX(ServiceTaskID),0)+1 FROM RD_SERVICE_TASK), @ServiceTaskCode, @ServiceTaskDesc, StationExceededDesc, ComplementServiceTaskID, ExternalExpeditionMode
		FROM RD_SERVICE_TASK WITH(NOLOCK)
		WHERE ServiceTaskID = @RefServiceTaskID

		IF (@@ERROR<>0)
		BEGIN
			ROLLBACK TRANSACTION
			SELECT -10 ErrorID, 'DataBaseError' Error
			RETURN -10
		END

		SELECT @ServiceTaskID = ServiceTaskID
		FROM RD_SERVICE_TASK
		WHERE ServiceTaskCode = @ServiceTaskCode

		DECLARE tCursor CURSOR LOCAL
		FOR SELECT ExpCompanyID, ExpCode
		FROM RD_EXPCOMPANY_SERVICE_TASK WITH(NOLOCK)
		WHERE ServiceTaskID = @RefServiceTaskID

		OPEN tCursor
		FETCH NEXT FROM tCursor INTO @ExpCompanyID, @ExpCode
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SELECT @NewExpCode = CompanyCode + @ServiceTaskCode
			FROM RD_COMPANY
			WHERE CompanyID = @ExpCompanyID

			IF (NOT EXISTS(SELECT TOP 1 1 FROM RD_EXPCODE WHERE ExpCode = @NewExpCode))
			BEGIN
				INSERT INTO RD_EXPCODE(ExpCode, DefaultExpCenterCode, DefaultExpCompanyZone, [Priority], [Description], CheckExpCompanySepCodes, PostalCodeStart)
				SELECT @NewExpCode, DefaultExpCenterCode, DefaultExpCompanyZone, [Priority], [Description], CheckExpCompanySepCodes, PostalCodeStart
				FROM RD_EXPCODE WITH(NOLOCK)
				WHERE ExpCode = @ExpCode
				IF (@@ERROR<>0)
				BEGIN
					ROLLBACK TRANSACTION
					SELECT -10 ErrorID, 'DataBaseError' Error
					RETURN -10
				END
			END

			INSERT INTO RD_EXPEDITION_EXPCENTER_EXPZONE(ExpCode, ExpCenterCode, ServiceCompanyID, ExpeditionZone, Description1, Description2, Description3)
			SELECT @NewExpCode, ExpCenterCode, ServiceCompanyID, ExpeditionZone, Description1, Description2, Description3
			FROM RD_EXPEDITION_EXPCENTER_EXPZONE WITH(NOLOCK)
			WHERE ExpCode = @ExpCode
			IF (@@ERROR<>0)
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -10 ErrorID, 'DataBaseError' Error
				RETURN -10
			END

			INSERT INTO RD_SERVICE_COMPANY_EXPCODE_CONFIG(ServiceCompanyID, ExpCode, ExpCenterCode, ExpLevel, FullFillMaterialCode, DocMaxSheets, Barcode)
			SELECT ServiceCompanyID, @NewExpCode, ExpCenterCode, ExpLevel, FullFillMaterialCode, DocMaxSheets, Barcode
			FROM RD_SERVICE_COMPANY_EXPCODE_CONFIG WITH(NOLOCK)
			WHERE ExpCode = @ExpCode
			IF (@@ERROR<>0)
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -10 ErrorID, 'DataBaseError' Error
				RETURN -10
			END

			DECLARE @StartCode numeric(18, 0), 
				@EndCode numeric(18, 0), 
				@IsPostalCode bit, 
				@Description varchar(50),
				@ExpCenterCode varchar(5),
				@ExpSepCodeComplement int, 
				@NewExpCompanyID int

			DECLARE eCursor CURSOR LOCAL
			FOR SELECT StartCode, EndCode, IsPostalCode, [Description], ExpCenterCode, ExpSepCodeComplement, NewExpCompanyID
			FROM RD_EXPCODE_EXPEDITION_SEPCODES
			WHERE ExpCode = @ExpCode
			OPEN tCursor
			FETCH NEXT FROM eCursor INTO @StartCode, @EndCode, @IsPostalCode, @Description, @ExpCenterCode, @ExpSepCodeComplement, @NewExpCompanyID
			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO RD_EXPCODE_EXPEDITION_SEPCODES(ExpSepID, ExpCode, StartCode, EndCode, IsPostalCode, [Description], ExpCenterCode, ExpSepCodeComplement, NewExpCompanyID) 
				SELECT ISNULL(MAX(ExpSepID),0) + 1, @NewExpCode, @StartCode, @EndCode, @IsPostalCode, @Description, @ExpCenterCode, @ExpSepCodeComplement, @NewExpCompanyID
				FROM RD_EXPCODE_EXPEDITION_SEPCODES
				IF (@@ERROR<>0)
				BEGIN
					ROLLBACK TRANSACTION
					SELECT -10 ErrorID, 'DataBaseError' Error
					RETURN -10
				END
				FETCH NEXT FROM eCursor INTO @StartCode, @EndCode, @IsPostalCode, @Description, @ExpCenterCode, @ExpSepCodeComplement, @NewExpCompanyID
			END
			CLOSE eCursor
			DEALLOCATE eCursor

			INSERT INTO RD_EXPCOMPANY_SERVICE_TASK(ExpCompanyID, ServiceTaskID, ExpCode)
			SELECT @ExpCompanyID, @ServiceTaskID, @NewExpCode
			IF (@@ERROR<>0)
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -10 ErrorID, 'DataBaseError' Error
				RETURN -10
			END
			FETCH NEXT FROM tCursor INTO @ExpCompanyID, @ExpCode
		END
		CLOSE tCursor
		DEALLOCATE tCursor

		INSERT INTO RD_SERVICE_TASK_SERVICE_TYPE(ServiceTaskID, ServiceTypeID)
		SELECT @ServiceTaskID, ServiceTypeID
		FROM RD_SERVICE_TASK_SERVICE_TYPE WITH(NOLOCK)
		WHERE ServiceTaskID = @RefServiceTaskID

		IF (@@ERROR<>0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		COMMIT TRANSACTION
	END
	ELSE
	BEGIN
		UPDATE RD_SERVICE_TASK
		SET [Description] = @ServiceTaskDesc,
			StationExceededDesc = @StationExceededDesc,
			ComplementServiceTaskID = @ComplementServiceTaskID,
			ExternalExpeditionMode = @ExternalExpeditionMode
		WHERE ServiceTaskID = @ServiceTaskID
	END
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_DELETE_SERVICE_TASK]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_DELETE_SERVICE_TASK] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_DELETE_SERVICE_TASK]
	@ServiceTaskID int
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRANSACTION	

	DELETE RD_SERVICE_TASK_SERVICE_TYPE
	WHERE ServiceTaskID = @ServiceTaskID

	DELETE RD_EXPCODE_EXPEDITION_SEPCODES
	FROM RD_EXPCODE_EXPEDITION_SEPCODES ees
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est
	ON est.ExpCode = ees.ExpCode
	WHERE ServiceTaskID = @ServiceTaskID

	DELETE RD_SERVICE_COMPANY_EXPCODE_CONFIG
	FROM RD_SERVICE_COMPANY_EXPCODE_CONFIG scec
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est
	ON est.ExpCode = scec.ExpCode
	WHERE ServiceTaskID = @ServiceTaskID

	DELETE RD_EXPEDITION_EXPCENTER_EXPZONE
	FROM RD_EXPEDITION_EXPCENTER_EXPZONE eee
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est
	ON est.ExpCode = eee.ExpCode
	WHERE ServiceTaskID = @ServiceTaskID

	SELECT ExpCode
	INTO #EXPCODE
	FROM RD_EXPCOMPANY_SERVICE_TASK
	WHERE ServiceTaskID = @ServiceTaskID

	DELETE RD_EXPCOMPANY_SERVICE_TASK
	WHERE ServiceTaskID = @ServiceTaskID

	DELETE RD_EXPCODE
	WHERE ExpCode in (SELECT ExpCode FROM #EXPCODE)

	DELETE RD_SERVICE_TASK
	WHERE ServiceTaskID = @ServiceTaskID

	COMMIT TRANSACTION
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_SERVICE_TASK_SERVICE_TYPE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_SERVICE_TASK_SERVICE_TYPE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_SET_SERVICE_TASK_SERVICE_TYPE]
	@ServiceTaskID int,
	@ServiceTypeID int
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO RD_SERVICE_TASK_SERVICE_TYPE(ServiceTaskID, ServiceTypeID)
	SELECT @ServiceTaskID, @ServiceTypeID
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM RD_SERVICE_TASK_SERVICE_TYPE
					WHERE ServiceTaskID = @ServiceTaskID
						AND ServiceTypeID = @ServiceTypeID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_DELETE_SERVICE_TASK_SERVICE_TYPE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_DELETE_SERVICE_TASK_SERVICE_TYPE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_DELETE_SERVICE_TASK_SERVICE_TYPE]
	@ServiceTaskID int,
	@ServiceTypeID int
AS
BEGIN
	SET NOCOUNT ON
	DELETE RD_SERVICE_TASK_SERVICE_TYPE
	WHERE ServiceTaskID = @ServiceTaskID
		AND ServiceTypeID = @ServiceTypeID
END
GO