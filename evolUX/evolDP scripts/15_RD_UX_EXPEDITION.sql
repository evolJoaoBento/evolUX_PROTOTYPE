IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPEDITION_TYPE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_TYPE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_TYPE]
	@ExpeditionType int = NULL
AS
	SET NOCOUNT ON

	SELECT ExpeditionType, [Priority], [Description]
	FROM RD_EXPEDITION_TYPE WITH(NOLOCK)
	WHERE @ExpeditionType is NULL
		OR @ExpeditionType = ExpeditionType
	ORDER BY [Priority] DESC, [Description]
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_TYPE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_TYPE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_TYPE]
	@ExpeditionType int = NULL,
	@ExpCompanyID int = NULL,
	@ExpCompanyList IDList READONLY
AS
	SET NOCOUNT ON
	SELECT
		ect.ExpCompanyID,
		ect.ExpeditionType,
		ect.RegistMode, --Controlo de Registo ==> 1 - Ativo, 0 - Desativo
		ect.SeparationMode, --Separação física de escalões de expedição ==> 1 - Ativo, 0 - Desativo
		ect.BarcodeRegistMode,
		CAST(CASE WHEN ect.RegistMode = 1 AND cerd.ExpCompanyID is NOT NULL THEN 1 ELSE 0 END as bit) GenerateDetailRegist, --Gerar Ficheiro de Detalhe de Registos ==> 1 - Ativo, 0 - Desativo
		CAST(CASE WHEN ISNULL(cerd.Registado, 0) = 1 THEN 1 ELSE 0 END as bit) DRFlagRegistado,
		CAST(CASE WHEN ISNULL(cerd.Encomenda,0) = 1 THEN 1 ELSE 0 END as bit) DRFlagEncomenda,
		CAST(CASE WHEN ISNULL(cerd.Pessoal,0) = 1 THEN 1 ELSE 0 END as bit) DRFlagPessoal,
		et.[Priority]
	FROM
		RD_EXPCOMPANY_TYPE ect
	INNER JOIN
		RD_EXPEDITION_TYPE et
	ON ect.ExpeditionType = et.ExpeditionType
	LEFT OUTER JOIN
		RD_C_EXPCOMPANY_REGIST_DETAIL_FILE cerd
	ON  cerd.ExpCompanyID =  ect.ExpCompanyID
		AND cerd.ExpeditionType = ect.ExpeditionType
	WHERE (@ExpeditionType is NULL OR @ExpeditionType = et.ExpeditionType)
		AND
			(ect.ExpCompanyID = @ExpCompanyID
			OR ect.ExpCompanyID in (SELECT ID FROM @ExpCompanyList))
	ORDER BY ect.ExpCompanyID, et.[Priority] DESC

RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_EXPCOMPANY_TYPE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_EXPCOMPANY_TYPE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_SET_EXPCOMPANY_TYPE]
	@ExpeditionType int,
	@ExpCompanyID int,
	@RegistMode bit,
	@SeparationMode int,
	@BarcodeRegistMode bit
AS
	SET NOCOUNT ON
	IF (EXISTS (SELECT TOP 1 1 FROM RD_EXPCOMPANY_TYPE 
		WHERE ExpeditionType = @ExpeditionType
			AND ExpCompanyID = @ExpCompanyID))
	BEGIN
		UPDATE RD_EXPCOMPANY_TYPE
		SET RegistMode = @RegistMode, SeparationMode = @SeparationMode, BarcodeRegistMode = @BarcodeRegistMode
		WHERE ExpeditionType = @ExpeditionType
			AND ExpCompanyID = @ExpCompanyID
	END
	ELSE
	BEGIN
		DECLARE @Priority int,
				@ExpeditionTypeRef int,
				@StartDate int
		SELECT @Priority = MAX(e.[Priority])
		FROM RD_EXPCOMPANY_CONFIG ec WITH(NOLOCK)
		INNER JOIN
			RD_EXPCOMPANY_TYPE et WITH(NOLOCK)
		ON et.ExpCompanyID = ec.ExpCompanyID
			AND et.ExpeditionType = ec.ExpeditionType
		INNER JOIN
			RD_EXPEDITION_TYPE e WITH(NOLOCK)
		ON e.ExpeditionType = et.ExpeditionType
		WHERE e.[Priority] <= (SELECT [Priority]
							FROM RD_EXPEDITION_TYPE
							WHERE ExpeditionType = @ExpeditionType)

		SELECT @ExpeditionTypeRef = MAX(e.ExpeditionType), @StartDate = MAX(ec.StartDate)
		FROM RD_EXPCOMPANY_CONFIG ec WITH(NOLOCK)
		INNER JOIN
			RD_EXPCOMPANY_TYPE et WITH(NOLOCK)
		ON et.ExpCompanyID = ec.ExpCompanyID
			AND et.ExpeditionType = ec.ExpeditionType
		INNER JOIN
			RD_EXPEDITION_TYPE e WITH(NOLOCK)
		ON e.ExpeditionType = et.ExpeditionType
		WHERE e.[Priority] = @Priority
			AND e.ExpeditionType <> @ExpeditionType
							

		INSERT INTO RD_EXPCOMPANY_TYPE(ExpCompanyID, ExpeditionType, RegistMode, SeparationMode, BarcodeRegistMode)
		SELECT @ExpCompanyID, @ExpeditionType, @RegistMode, @SeparationMode, @BarcodeRegistMode
		
		INSERT INTO RD_EXPCOMPANY_CONFIG(ExpCompanyID, ExpeditionZone, ExpeditionType, ExpCompanyLevel, StartDate, UnitCost, MaxWeight, ExpColumnA, ExpColumnB, ExpColumnE, ExpColumnI, ExpColumnF)
		SELECT ec.ExpCompanyID, ec.ExpeditionZone, @ExpeditionType, ec.ExpCompanyLevel, ec.StartDate, ec.UnitCost, ec.MaxWeight, ec.ExpColumnA, ec.ExpColumnB, ec.ExpColumnE, ec.ExpColumnI, ec.ExpColumnF
		FROM RD_EXPCOMPANY_CONFIG ec WITH(NOLOCK)
		INNER JOIN
			RD_EXPCOMPANY_TYPE et WITH(NOLOCK)
		ON et.ExpCompanyID = ec.ExpCompanyID
			AND et.ExpeditionType = ec.ExpeditionType
		INNER JOIN
			RD_EXPEDITION_TYPE e WITH(NOLOCK)
		ON e.ExpeditionType = et.ExpeditionType
		WHERE ec.ExpCompanyID = @ExpCompanyID
			AND ec.ExpeditionType = @ExpeditionTypeRef
			AND ec.StartDate = @StartDate
	END
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPEDITION_ZONE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_ZONE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_ZONE]
	@ExpeditionZone int = NULL
AS
	SET NOCOUNT ON

	SELECT ExpeditionZone, [Description]
	FROM RD_EXPEDITION_ZONE WITH(NOLOCK)
	WHERE @ExpeditionZone is NULL
		OR @ExpeditionZone = ExpeditionZone
	ORDER BY ExpeditionZone ASC
RETURN
GO
INSERT INTO RD_EXPEDITION_REFERENCE(ExpCompanyID, ColumnName, ValueID, Description)
SELECT DISTINCT ec.ExpCompanyID, 'A', ec.ExpColumnA, ez.[Description]
FROM RD_EXPCOMPANY_CONFIG ec WITH(NOLOCK)
INNER JOIN
	RD_EXPEDITION_ZONE ez WITH(NOLOCK)
ON ec.ExpeditionZone = ez.ExpeditionZone
LEFT OUTER JOIN
	RD_EXPEDITION_REFERENCE er WITH(NOLOCK)
ON er.ExpCompanyID = ec.ExpCompanyID
	AND er.ValueID = ec.ExpColumnA
	AND er.[ColumnName] = 'A'
WHERE er.ValueID is NULL
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_ZONE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_ZONE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_ZONE]
	@ExpeditionZone int = NULL,
	@ExpCompanyID int = NULL,
	@ExpCompanyList IDList READONLY
AS
	SET NOCOUNT ON

	SELECT DISTINCT ec.ExpCompanyID, ez.ExpeditionZone, s.ServiceTaskID, s.[Description] ServiceTaskDesc, e.[Priority], eee.ExpCenterCode, eee.ServiceCompanyID, cs.CompanyName [ServiceCompanyName]
	FROM RD_EXPEDITION_ZONE ez WITH(NOLOCK)
	INNER JOIN
		RD_EXPCOMPANY_CONFIG ec WITH(NOLOCK)
	ON ec.ExpeditionZone = ez.ExpeditionZone
	INNER JOIN
		RD_COMPANY ce WITH(NOLOCK)
	ON ce.CompanyID = ec.ExpCompanyID
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	ON est.ExpCompanyID = ec.ExpCompanyID
	INNER JOIN
		RD_EXPEDITION_EXPCENTER_EXPZONE eee WITH(NOLOCK)
	ON eee.ExpeditionZone = ez.ExpeditionZone
		AND eee.ExpCode = est.ExpCode
	INNER JOIN
		RD_SERVICE_TASK s WITH(NOLOCK)
	ON s.ServiceTaskID = est.ServiceTaskID
	INNER JOIN
		RD_COMPANY cs WITH(NOLOCK)
	ON cs.CompanyID = eee.ServiceCompanyID
	INNER JOIN
		RD_EXPCODE e WITH(NOLOCK)
	ON e.ExpCode = eee.ExpCode
	WHERE (@ExpeditionZone is NULL OR @ExpeditionZone = ez.ExpeditionZone)
		AND
			(ec.ExpCompanyID = @ExpCompanyID
			OR ec.ExpCompanyID in (SELECT ID FROM @ExpCompanyList))
	ORDER BY ez.ExpeditionZone, ec.ExpCompanyID, eee.ExpCenterCode, eee.ServiceCompanyID, s.ServiceTaskID, e.[Priority] DESC
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_SERVICE_TASK]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_SERVICE_TASK] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_SERVICE_TASK]
	@ExpCode varchar(10) = NULL
AS
BEGIN
	SELECT ExpCode, ExpCompanyID, ServiceTaskID
	FROM RD_EXPCOMPANY_SERVICE_TASK
	WHERE @ExpCode is NULL OR ExpCode = @ExpCode
	ORDER BY ExpCode
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_ZONE_TYPE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_ZONE_TYPE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_ZONE_TYPE]
	@ExpCompanyID int
AS
BEGIN
	SELECT 	c.CompanyName ExpCompanyName,
		ez.[ExpeditionZone],
		ez.[Description] as [ExpeditionZoneDesc],
		et.[ExpeditionType],
		et.[Description] as [ExpeditionTypeDesc]
	FROM
		(SELECT DISTINCT ExpCompanyID, ExpeditionZone, ExpeditionType
		FROM RD_EXPCOMPANY_CONFIG) ec
	INNER JOIN
		RD_COMPANY c
	ON c.CompanyID = ec.ExpCompanyID
	INNER JOIN
		RD_EXPEDITION_ZONE ez
	ON ez.ExpeditionZone = ec.ExpeditionZone
	INNER JOIN
		RD_EXPEDITION_TYPE et
	ON et.ExpeditionType = ec.ExpeditionType
	WHERE ec.ExpCompanyID = @ExpCompanyID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_CONFIGS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_CONFIGS] AS' 
END
GO
--Configuracoes de escalão (listar e alterar)
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_CONFIGS]
	@ExpCompanyID int,
	@StartDate int = NULL,
	@ExpeditionType int = NULL,
	@ExpeditionZone int = NULL
AS
BEGIN
	SELECT 
		ec.ExpCompanyID,
		ez.ExpeditionZone,
		ez.[Description] [ExpeditionZoneDesc],
		et.ExpeditionType,
		et.[Description] [ExpeditionTypeDesc],
		ec.ExpCompanyLevel,
		ec.MaxWeight,  --Peso Máximo
		ec.StartDate, --Data de Efetivação
		ec.UnitCost, --Custo Unitário
		ec.ExpColumnA, --Zona de Taxação
		ec.ExpColumnB, --Produto
		ec.ExpColumnE, --Velocidade
		ec.ExpColumnI, --Serviços Especiais
		ec.ExpColumnF -- Formato
	FROM
		RD_EXPCOMPANY_CONFIG ec WITH(NOLOCK)
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK)
	ON	ec.ExpCompanyID = c.CompanyID
	INNER JOIN
		RD_EXPEDITION_Zone ez WITH(NOLOCK)
	ON	ec.ExpeditionZone = ez.ExpeditionZone
	INNER JOIN
		RD_EXPEDITION_TYPE et
	ON	ec.ExpeditionType = et.ExpeditionType
	WHERE ec.ExpCompanyID = @ExpCompanyID
		AND (@ExpeditionZone is NULL OR ec.ExpeditionZone = @ExpeditionZone)
		AND (@ExpeditionType is NULL OR ec.ExpeditionType = @ExpeditionType)
		AND (@StartDate is NULL OR ec.StartDate = @StartDate)
		--(SELECT MAX(StartDate) 
		--		FROM RD_EXPCOMPANY_CONFIG WITH(NOLOCK)
		--		WHERE ExpCompanyID = ec.ExpCompanyID
		--		AND ExpeditionZone = ec.ExpeditionZone
		--		AND ExpeditionType = ec.ExpeditionType
		--		AND ExpCompanyLevel = ec.ExpCompanyLevel)
	ORDER BY ec.StartDate DESC, ec.ExpeditionType, ec.ExpeditionZone, ec.MaxWeight 
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_EXPCOMPANY_CONFIGS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_EXPCOMPANY_CONFIGS] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_SET_EXPCOMPANY_CONFIGS]
	@ExpCompanyID int,
	@ExpeditionType int,
	@ExpeditionZone int,
	@ExpCompanyLevel int, 
	@StartDate int, 
	@UnitCost float, 
	@MaxWeight int = NULL, 
	@ExpColumnA varchar(10), 
	@ExpColumnB varchar(10), 
	@ExpColumnE varchar(10), 
	@ExpColumnI varchar(10) = NULL, 
	@ExpColumnF varchar(10) = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF (NOT EXISTS (SELECT TOP 1 1 FROM RD_EXPCOMPANY_CONFIG WHERE ExpCompanyID = @ExpCompanyID AND StartDate = @StartDate))
	BEGIN		
		DECLARE @RefStartDate int

		SELECT @RefStartDate = ISNULL(MAX(StartDate),0)
		FROM RD_EXPCOMPANY_CONFIG
		WHERE ExpCompanyID = @ExpCompanyID
			AND StartDate < @StartDate

		INSERT INTO RD_EXPCOMPANY_CONFIG(ExpCompanyID, ExpeditionZone, ExpeditionType, ExpCompanyLevel, StartDate, UnitCost, MaxWeight, ExpColumnA, ExpColumnB, ExpColumnE, ExpColumnI, ExpColumnF)
		SELECT ExpCompanyID, ExpeditionZone, ExpeditionType, ExpCompanyLevel, @StartDate, UnitCost, MaxWeight, ExpColumnA, ExpColumnB, ExpColumnE, ExpColumnI, ExpColumnF
		FROM RD_EXPCOMPANY_CONFIG
		WHERE ExpCompanyID = @ExpCompanyID
			AND StartDate = @RefStartDate
	END

	IF (NOT EXISTS (SELECT TOP 1 1 FROM RD_EXPCOMPANY_CONFIG 
			WHERE ExpCompanyID = @ExpCompanyID AND StartDate = @StartDate
			AND ExpeditionZone = @ExpeditionZone AND ExpeditionType = @ExpeditionType
			AND ExpCompanyLevel = ISNULL(@ExpCompanyLevel,-1)))
	BEGIN
		IF (NOT EXISTS (SELECT TOP 1 1 FROM RD_EXPCOMPANY_CONFIG 
			WHERE ExpCompanyID = @ExpCompanyID AND StartDate = @StartDate
			AND ExpeditionZone = @ExpeditionZone AND ExpeditionType = @ExpeditionType
			AND ISNULL(MaxWeight,2147483647) = ISNULL(@MaxWeight,2147483647)))
		BEGIN
			DECLARE @MinRefExpCompanyLevel int,
					@MaxRefExpCompanyLevel int,
					@RefExpCompanyLevel int

			SELECT @MinRefExpCompanyLevel = ISNULL(MAX(ExpCompanyLevel),0)
			FROM RD_EXPCOMPANY_CONFIG
			WHERE ExpCompanyID = @ExpCompanyID AND StartDate = @StartDate
				AND ExpeditionZone = @ExpeditionZone AND ExpeditionType = @ExpeditionType
				AND ISNULL(MaxWeight,2147483647) < ISNULL(@MaxWeight,2147483647)

			SELECT @MaxRefExpCompanyLevel = ISNULL(MIN(ExpCompanyLevel),0)
			FROM RD_EXPCOMPANY_CONFIG
			WHERE ExpCompanyID = @ExpCompanyID AND StartDate = @StartDate
				AND ExpeditionZone = @ExpeditionZone AND ExpeditionType = @ExpeditionType
				AND ISNULL(MaxWeight,2147483647) > ISNULL(@MaxWeight,2147483647)

			SELECT @RefExpCompanyLevel = @MinRefExpCompanyLevel + (@MaxRefExpCompanyLevel - @MinRefExpCompanyLevel)/2
			WHILE (EXISTS (SELECT TOP 1 1 FROM RD_EXPCOMPANY_CONFIG
			WHERE ExpCompanyID = @ExpCompanyID AND StartDate = @StartDate
				AND ExpeditionZone = @ExpeditionZone AND ExpeditionType = @ExpeditionType
				AND ExpCompanyLevel = @RefExpCompanyLevel)
				OR @RefExpCompanyLevel = 0)
			BEGIN
				SELECT @RefExpCompanyLevel = @RefExpCompanyLevel + 1
			END

			INSERT INTO RD_EXPCOMPANY_CONFIG(ExpCompanyID, ExpeditionZone, ExpeditionType, ExpCompanyLevel, StartDate, UnitCost, MaxWeight, ExpColumnA, ExpColumnB, ExpColumnE, ExpColumnI, ExpColumnF)
			SELECT @ExpCompanyID, @ExpeditionZone, @ExpeditionType, @RefExpCompanyLevel, @StartDate, @UnitCost, @MaxWeight, @ExpColumnA, @ExpColumnB, @ExpColumnE, @ExpColumnI, @ExpColumnF
		END
		ELSE
		BEGIN
			UPDATE RD_EXPCOMPANY_CONFIG
			SET UnitCost = @UnitCost, 
				ExpCompanyLevel = @ExpCompanyLevel, 
				ExpColumnA = @ExpColumnA, 
				ExpColumnB = @ExpColumnB, 
				ExpColumnE = @ExpColumnE, 
				ExpColumnI = @ExpColumnI, 
				ExpColumnF = @ExpColumnF
			WHERE ExpCompanyID = @ExpCompanyID AND StartDate = @StartDate
				AND ExpeditionZone = @ExpeditionZone AND ExpeditionType = @ExpeditionType
				AND ISNULL(MaxWeight,2147483647) = ISNULL(@MaxWeight,2147483647)
		END
	END
	ELSE
	BEGIN
		UPDATE RD_EXPCOMPANY_CONFIG
		SET UnitCost = @UnitCost, 
			MaxWeight = @MaxWeight, 
			ExpColumnA = @ExpColumnA, 
			ExpColumnB = @ExpColumnB, 
			ExpColumnE = @ExpColumnE, 
			ExpColumnI = @ExpColumnI, 
			ExpColumnF = @ExpColumnF
		WHERE ExpCompanyID = @ExpCompanyID AND StartDate = @StartDate
			AND ExpeditionZone = @ExpeditionZone AND ExpeditionType = @ExpeditionType
			AND ExpCompanyLevel = @ExpCompanyLevel
	END
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_EXPEDITION_IDS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_EXPEDITION_IDS] AS' 
END
GO
--Faixa de registos (listar, alterar e adicionar)
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_EXPEDITION_IDS]
	@ExpCompanyID int
AS
BEGIN
	SELECT 
		REPLICATE('0',8-LEN(CAST(ei.CompanyRegistCode as varchar))) + CAST(ei.CompanyRegistCode as varchar) CompanyRegistCode, -- SRP - Serviço de Regsto Privativo
		REPLICATE('0',8-LEN(CAST(ei.StartExpeditionID as varchar))) + CAST(ei.StartExpeditionID as varchar) StartExpeditionID, --Registo Inicial
		REPLICATE('0',8-LEN(CAST(ei.EndExpeditionID as varchar))) + CAST(ei.EndExpeditionID as varchar) EndExpeditionID, --Registo Final 
		REPLICATE('0',8-LEN(ISNULL(CAST(ISNULL(ei.LastExpeditionID,0) as varchar),'00000000'))) + ISNULL(CAST(ISNULL(ei.LastExpeditionID,0) as varchar),'') LastExpeditionID, --Último Registo Utilizado
		ISNULL(ei.RegistCodePrefix,'RP') RegistCodePrefix, --Prefixo
		ISNULL(ei.RegistCodeSuffix,'PT') RegistCodeSuffix --Sufixo
	FROM  RD_EXPEDITION_ID ei
	WHERE ei.ExpCompanyID = @ExpCompanyID
		AND ei.EndExpeditionID > ISNULL(ei.LastExpeditionID, 0)
	ORDER BY ISNULL(ei.RegistCodePrefix,'RP') DESC, ei.StartExpeditionID ASC
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_EXPCOMPANY_EXPEDITION_IDS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_EXPCOMPANY_EXPEDITION_IDS] AS' 
END
GO
--Faixa de registos (listar, alterar e adicionar)
ALTER  PROCEDURE [dbo].[RD_UX_SET_EXPCOMPANY_EXPEDITION_IDS]
	@ExpCompanyID int,
	@StartExpeditionID int,
	@EndExpeditionID int,
	@CompanyRegistCode int,
	@RegistCodePrefix char(2),
	@RegistCodeSuffix char(2),
	@LastExpeditionID int = NULL
AS
BEGIN
	SET NOCOUNT ON
	IF (NOT EXISTS (SELECT TOP 1 1 FROM RD_EXPEDITION_ID WHERE ExpCompanyID = @ExpCompanyID AND StartExpeditionID = @StartExpeditionID))
	BEGIN
		INSERT INTO RD_EXPEDITION_ID(ExpCompanyID, StartExpeditionID, EndExpeditionID, CompanyRegistCode, LastExpeditionID, RegistCodePrefix, RegistCodeSuffix)
		SELECT @ExpCompanyID, @StartExpeditionID, @EndExpeditionID, @CompanyRegistCode, @LastExpeditionID, @RegistCodePrefix, @RegistCodeSuffix
	END
	ELSE
	BEGIN
		UPDATE RD_EXPEDITION_ID
		SET EndExpeditionID = @EndExpeditionID,
			LastExpeditionID = ISNULL(@LastExpeditionID,LastExpeditionID),
			RegistCodePrefix = @RegistCodePrefix,
			RegistCodeSuffix = @RegistCodeSuffix
		WHERE ExpCompanyID = @ExpCompanyID
			AND StartExpeditionID = @StartExpeditionID
	END
	RETURN @StartExpeditionID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_CONTRACTS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_CONTRACTS] AS' 
END
GO
--Contratos (Listar/Adicionar e alterar)
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_CONTRACTS]
	@ExpCompanyID int
AS
BEGIN
	SELECT ec.ContractID,
		ContractNr,
		ClientNr, 
		ClientName, 
		ClientNIF,  
		ClientAddress,
		ClientPostalCode,
		ClientPostalCodeDescription,
		PurchaseOrderNr, --Nr Ordem de Compra
		CompanyExpeditionCode --Cód. de Cliente para BarCode de Expedição
	FROM RD_EXPCOMPANY_CONTRACT ec WITH(NOLOCK)
	WHERE ec.ExpCompanyID = @ExpCompanyID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_EXPCOMPANY_CONTRACTS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_EXPCOMPANY_CONTRACTS] AS' 
END
GO
--Contratos (Listar/Adicionar e alterar)
ALTER  PROCEDURE [dbo].[RD_UX_SET_EXPCOMPANY_CONTRACTS]
	@ExpCompanyID int,
	@ContractID int = NULL,
	@ContractNr int,
	@ClientNr int, 
	@ClientName varchar(90), 
	@ClientNIF varchar(9),  
	@ClientAddress varchar(100),
	@ClientPostalCode varchar(10) = NULL,
	@ClientPostalCodeDescription varchar(50) = NULL,
	@PurchaseOrderNr numeric(15,0) = NULL, --Nr Ordem de Compra
	@CompanyExpeditionCode char(2) = NULL
AS
BEGIN
	IF (@ContractID is NULL)
	BEGIN
		INSERT INTO RD_EXPCOMPANY_CONTRACT(ExpCompanyID, ContractID, ContractNr, ClientNr, ClientName, ClientNIF, ClientAddress, CompanyExpeditionCode, ClientPostalCode, ClientPostalCodeDescription, PurchaseOrderNr)
		SELECT ExpCompanyID, ISNULL(MAX(ContractID),0)+ 1, 
			@ContractNr,
			@ClientNr, 
			@ClientName, 
			@ClientNIF,  
			@ClientAddress,
			@ClientPostalCode,
			@ClientPostalCodeDescription,
			@PurchaseOrderNr, 
			@CompanyExpeditionCode 
			FROM RD_EXPCOMPANY_CONTRACT
			WHERE ExpCompanyID = @ExpCompanyID
			GROUP BY ExpCompanyID
	END
	ELSE
	BEGIN
		SELECT @ContractID = ContractID
		FROM RD_EXPCOMPANY_CONTRACT
		WHERE ExpCompanyID = @ExpCompanyID AND ContractNr = @ContractNr AND ContractNr = @ContractNr

		IF (@ContractID is NULL)
		BEGIN
			UPDATE RD_EXPCOMPANY_CONTRACT
			SET ContractNr = @ContractNr,
				ClientNr = @ClientNr, 
				ClientName = @ClientName, 
				ClientNIF = @ClientNIF,  
				ClientAddress = @ClientAddress,
				ClientPostalCode = @ClientPostalCode,
				ClientPostalCodeDescription = @ClientPostalCodeDescription,
				PurchaseOrderNr = @PurchaseOrderNr,
				CompanyExpeditionCode = @CompanyExpeditionCode
			WHERE ExpCompanyID = @ExpCompanyID AND ContractID = @ContractID
		END
	END
	SELECT @ContractID = ContractID
	FROM RD_EXPCOMPANY_CONTRACT
	WHERE ExpCompanyID = @ExpCompanyID AND ContractNr = @ContractNr AND ContractNr = @ContractNr

	RETURN @ContractID
END
GO
