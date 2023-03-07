IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_COMPANY_RESTRICTIONS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_RESTRICTIONS] AS' 
END
GO
--Configuração de restrições (listar/alterar)
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_RESTRICTIONS]
	@ServiceCompanyID int = NULL
AS
BEGIN
	SELECT
		scr.ServiceCompanyID,
		mt.MaterialTypeID,
		mt.MaterialTypeDescription MaterialTypeDesc,
		mt.MaterialTypeCode,
		scr.MaterialPosition, --Nº Máximo de Bandejas/Estações disponíveis
		scr.FileSheetsCutoffLevel,  --Nível Máximo de Folhas por Ficheiro, NULL ==> Não Aplicável
		scr.RestrictionMode 
	FROM
		RD_SERVICE_COMPANY_RESTRICTION scr WITH(NOLOCK)
	INNER JOIN
		RD_MATERIAL_TYPE mt WITH(NOLOCK)
	ON	scr.MaterialTypeID = mt.MaterialTypeID
	WHERE (@ServiceCompanyID is NULL OR scr.ServiceCompanyID = @ServiceCompanyID)
		AND mt.MaterialTypeCode not in ('RollPaper')
	ORDER BY scr.ServiceCompanyID, mt.MaterialTypeID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_SERVICE_COMPANY_RESTRICTION]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_SERVICE_COMPANY_RESTRICTION] AS' 
END
GO
--Configuração de restrições (listar/alterar)
ALTER  PROCEDURE [dbo].[RD_UX_SET_SERVICE_COMPANY_RESTRICTION]
	@ServiceCompanyID int,
	@MaterialTypeID int,
	@MaterialPosition int,
	@FileSheetsCutoffLevel int = NULL,
	@RestrictionMode bit = 0
AS
BEGIN
	UPDATE RD_SERVICE_COMPANY_RESTRICTION
	SET MaterialPosition = @MaterialPosition,
		FileSheetsCutoffLevel = @FileSheetsCutoffLevel,
		RestrictionMode = @RestrictionMode
	WHERE ServiceCompanyID = @ServiceCompanyID
		AND MaterialTypeID = @MaterialTypeID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICES_RESUME]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICES_RESUME] AS' 
END
GO
-- Tipos de serviços prestados pela companhia
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICES_RESUME]
	@ServiceCompanyID int = NULL
AS
BEGIN
	SELECT DISTINCT 
		scsc.ServiceCompanyID,
		st.ServiceTypeID,
		st.ServiceTypeCode,
		st.ServiceTypeDescription ServiceTypeDesc,
		s.ServiceID,
		s.ServiceCode,
		s.ServiceDescription ServiceDesc
	FROM dbo.RD_SERVICE_TYPE st WITH(NOLOCK)
	INNER JOIN
		dbo.RD_SERVICE s WITH(NOLOCK)
	ON	s.ServiceTypeID = st.ServiceTypeID
	INNER JOIN
		dbo.RD_SERVICE_COMPANY_SERVICE_COST scsc WITH(NOLOCK)
	ON	scsc.ServiceID = s.ServiceID
	WHERE (@ServiceCompanyID is NULL OR scsc.ServiceCompanyID = @ServiceCompanyID)
	ORDER BY scsc.ServiceCompanyID, st.ServiceTypeID, s.ServiceID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICE_COSTS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICE_COSTS] AS' 
END
GO
--Configuração de custos para os serviços prestados pela companhia - Alterar os já existentes ou  apagar (adiconar????)
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICE_COSTS]
	@ServiceCompanyID int,
	@CostDate int = NULL,
	@ServiceTypeID int = NULL,
	@ServiceID int = NULL
AS
BEGIN
	SELECT 
		scsc.ServiceCompanyID,
		st.ServiceTypeID, --Tipo de Serviço ID
		st.[ServiceTypeDescription] ServiceTypeDesc, --Tipo de Serviço
		s.ServiceID,
		s.ServiceDescription ServiceDesc, --Descrição do Serviço
		s.ServiceCode, --Código do Serviço
		scsc.CostDate, --Data de efectivação
		scsc.ServiceCost, --Custo do Serviço
		scsc.Formula, --Formula de Cálculo
		s.MatchCode --Critério de enquadramento
	FROM dbo.RD_SERVICE_TYPE st WITH(NOLOCK)
	INNER JOIN
		dbo.RD_SERVICE s WITH(NOLOCK)
	ON	s.ServiceTypeID = st.ServiceTypeID
	INNER JOIN
		dbo.RD_SERVICE_COMPANY_SERVICE_COST scsc WITH(NOLOCK)
	ON	scsc.ServiceID = s.ServiceID
	WHERE scsc.ServiceCompanyID = @ServiceCompanyID
		AND st.ServiceTypeID = @ServiceTypeID
		AND (@ServiceTypeID is NULL OR st.ServiceTypeID = @ServiceTypeID)
		AND (@ServiceID is NULL OR s.ServiceID = @ServiceID)
		AND (@CostDate is NULL OR scsc.CostDate = @CostDate)
	ORDER BY st.ServiceTypeID ASC, scsc.CostDate DESC, s.ServiceID ASC
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_SERVICE_COMPANY_SERVICE_COST]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_SERVICE_COMPANY_SERVICE_COST] AS' 
END
GO
--Configuração de custos para os serviços prestados pela companhia - Alterar os já existentes ou  apagar (adiconar????)
ALTER  PROCEDURE [dbo].[RD_UX_SET_SERVICE_COMPANY_SERVICE_COST]
	@ServiceCompanyID int,
	@ServiceID int,
	@CostDate int,
	@ServiceCost float,
	@Formula varchar(100)
AS
BEGIN
	IF (NOT EXISTS (SELECT TOP 1 1 
		FROM [dbo].[RD_SERVICE_COMPANY_SERVICE_COST] WITH(NOLOCK)
		WHERE ServiceCompanyID = @ServiceCompanyID
			AND ServiceID = @ServiceID
			AND CostDate = @CostDate))
	BEGIN
		INSERT INTO [dbo].[RD_SERVICE_COMPANY_SERVICE_COST](ServiceCompanyID, ServiceID, CostDate, ServiceCost, Formula)
		SELECT @ServiceCompanyID, @ServiceID, @CostDate, @ServiceCost, @Formula
	END
	ELSE
	BEGIN
		UPDATE RD_SERVICE_COMPANY_SERVICE_COST
		SET  ServiceCost = @ServiceCost,
			Formula = @Formula
		WHERE ServiceCompanyID = @ServiceCompanyID
			AND ServiceID = @ServiceID
			AND CostDate = @CostDate
	END
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICES] AS' 
END
GO
--Configuração de restrições (listar/alterar)
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICES]
	@ServiceTypeID int = NULL
AS
BEGIN
	SELECT
		s.ServiceTypeID, 
		st.ServiceTypeDescription ServiceTypeDesc,
		s.ServiceID, 
		s.ServiceCode, 
		s.ServiceDescription ServiceDesc, 
		s.MatchCode
	FROM
		RD_SERVICE s WITH(NOLOCK)
	INNER JOIN
		RD_SERVICE_TYPE st WITH(NOLOCK)
	ON	st.ServiceTypeID = s.ServiceTypeID
	WHERE (@ServiceTypeID is NULL OR s.ServiceTypeID = @ServiceTypeID)
	ORDER BY s.ServiceTypeID, s.ServiceID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_SERVICE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_SERVICE] AS' 
END
GO
--Configuração de restrições (listar/alterar)
ALTER  PROCEDURE [dbo].[RD_UX_SET_SERVICE]
	@ServiceID int = NULL,
	@ServiceCode varchar(25) = NULL,
	@ServiceTypeID int = NULL,
	@ServiceDesc varchar(256),
	@MatchCode varchar(50)
AS
BEGIN
	IF (@ServiceID is NULL OR 
		NOT EXISTS (SELECT TOP 1 1 
					FROM [dbo].[RD_SERVICE] WITH(NOLOCK)
					WHERE ServiceTypeID = @ServiceTypeID AND ServiceCode = @ServiceCode))
	BEGIN
		INSERT INTO [dbo].[RD_SERVICE](ServiceID, ServiceCode, ServiceTypeID, ServiceDescription, MatchCode)
		SELECT ISNULL(MAX(ServiceID),0) + 1, @ServiceCode, @ServiceTypeID, @ServiceDesc, @MatchCode
		FROM [dbo].[RD_SERVICE]

		SELECT @ServiceID = ServiceID
		FROM [dbo].[RD_SERVICE] WITH(NOLOCK)
		WHERE ServiceTypeID = @ServiceTypeID AND ServiceCode = @ServiceCode)
	END
	ELSE
	BEGIN
		UPDATE [dbo].[RD_SERVICE]
		SET [ServiceDescription] = @ServiceDesc,
			[MatchCode] = @MatchCode
		WHERE ServiceID = @ServiceID
	END
	RETURN @ServiceID
END
GO