IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPEDITION_COMPANIES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_COMPANIES] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_COMPANIES]
AS
BEGIN
	SELECT 	et.ExpCompanyID,
		c.CompanyCode,
		c.CompanyName,
		c.CompanyAddress,
		c.CompanyPostalCode,
		c.CompanyPostalCodeDescription,
		c.CompanyCountry
	FROM
		(SELECT DISTINCT ExpCompanyID 
		FROM RD_EXPCOMPANY_TYPE WITH(NOLOCK)) et
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK) 
	ON	et.ExpCompanyID = c.CompanyID
	ORDER BY et.ExpCompanyID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_ZONES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_ZONES] AS' 
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
	@ExpeditionZone int,
	@ExpeditionType int
AS
BEGIN
	SELECT 
		c.CompanyName ExpCompanyName,
		ec.ExpCompanyLevel,
		ez.[Description] [ExpeditionZoneDesc],
		et.[Description] [ExpeditionTypeDesc],
		ec.MaxWeight,  --Peso Máximo
		ec.StartDate, --Data de Efetivação
		ec.UnitCost, --Custo Unitário
		ec.ExpColumnB, --Produto
		ec.ExpColumnE, --Velocidade
		ec.ExpColumnI --Serviços Especiais 
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
		AND ec.ExpeditionZone = @ExpeditionZone
		AND ec.ExpeditionType = @ExpeditionType
		AND ec.StartDate = (SELECT MAX(StartDate) 
				FROM RD_EXPCOMPANY_CONFIG WITH(NOLOCK)
				WHERE ExpCompanyID = ec.ExpCompanyID
				AND ExpeditionZone = ec.ExpeditionZone
				AND ExpeditionType = ec.ExpeditionType
				AND ExpCompanyLevel = ec.ExpCompanyLevel)
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_CONFIGS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_CONFIGS] AS' 
END
GO
-- Configuracoes para tipos de expedição (alterar)
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_TYPES]
	@ExpCompanyID int
AS
BEGIN
	SELECT
		ect.ExpeditionType,
		et.[Description] [ExpeditionTypeDesc],
		ect.RegistMode, --Controlo de Registo ==> 1 - Ativo, 0 - Desativo
		ect.SeparationMode, --Separação física de escalões de expedição ==> 1 - Ativo, 0 - Desativo
		CASE WHEN ect.RegistMode = 1 AND cerd.ExpCompanyID is NOT NULL THEN 1 ELSE 0 END GenerateDetailRegist, --Gerar Ficheiro de Detalhe de Registos ==> 1 - Ativo, 0 - Desativo
		CASE WHEN ISNULL(cerd.Registado, 0) = 1 THEN 1 ELSE 0 END DRFlagRegistado,
		CASE WHEN ISNULL(cerd.Encomenda,0) = 1 THEN 1 ELSE 0 END DRFlagEncomenda,
		CASE WHEN ISNULL(cerd.Pessoal,0) = 1 THEN 1 ELSE 0 END DRFlagPessoal
	FROM
		RD_EXPCOMPANY_TYPE ect
	INNER JOIN
		RD_EXPEDITION_TYPE et
	ON ect.ExpeditionType = et.ExpeditionType
	LEFT OUTER JOIN
		RD_C_EXPCOMPANY_REGIST_DETAIL_FILE cerd
	ON  cerd.ExpCompanyID =  ect.ExpCompanyID
		AND cerd.ExpeditionType = ect.ExpeditionType
	WHERE ect.ExpCompanyID = @ExpCompanyID
	ORDER BY et.[Priority] DESC
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
		REPLICATE('0',8-LEN(ISNULL(CAST(ei.LastExpeditionID as varchar),'00000000'))) + ISNULL(CAST(ei.LastExpeditionID as varchar),'') LastExpeditionID, --Último Registo Utilizado
		ISNULL(ei.RegistCodePrefix,'RP') RegistCodePrefix, --Prefixo
		ISNULL(ei.RegistCodeSuffix,'PT') RegistCodeSuffix --Sufixo
	FROM  RD_EXPEDITION_ID ei
	WHERE ei.ExpCompanyID = @ExpCompanyID
		AND ei.EndExpeditionID > ISNULL(ei.LastExpeditionID, 0)
	ORDER BY ISNULL(ei.RegistCodePrefix,'RP') DESC, ei.StartExpeditionID ASC
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
