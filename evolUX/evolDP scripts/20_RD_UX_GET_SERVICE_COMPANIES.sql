IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_COMPANIES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANIES] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANIES]
AS
BEGIN
	SELECT c.CompanyID,
		c.CompanyCode,
		c.CompanyName,
		c.CompanyAddress,
		c.CompanyPostalCode,
		c.CompanyPostalCodeDescription,
		c.CompanyCountry
	FROM
		(SELECT DISTINCT ServiceCompanyID 
		FROM RD_SERVICE_COMPANY_RESTRICTION WITH(NOLOCK)) scr
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK) 
	ON	scr.ServiceCompanyID = c.CompanyID
	ORDER BY c.CompanyID
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_COMPANY_RESTRICTIONS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_RESTRICTIONS] AS' 
END
GO
--Configuração de restrições (listar/alterar)
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_RESTRICTIONS]
	@ServiceCompanyID int
AS
BEGIN
	SELECT
		c.CompanyCode ServiceCompanyCode,
		mt.MaterialTypeID,
		mt.MaterialTypeDescription,
		scr.MaterialPosition, --Nº Máximo de Bandejas/Estações disponíveis
		scr.FileSheetsCutoffLevel,  --Nível Máximo de Folhas por Ficheiro, NULL ==> Não Aplicável
		CASE WHEN mt.MaterialTypeCode = 'STATION' THEN CAST(scr.RestrictionMode as varchar) ELSE 'NA' END RestrictionMode --Ação em caso de exceder o nº máximo de estações, '0' ==> 'Impede Produção do objecto postal', 1 ==> 'Obriga a envelopagem manual do objecto postal'
	FROM
		RD_SERVICE_COMPANY_RESTRICTION scr WITH(NOLOCK)
	INNER JOIN
		RD_MATERIAL_TYPE mt WITH(NOLOCK)
	ON	scr.MaterialTypeID = mt.MaterialTypeID
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK)
	ON	 scr.ServiceCompanyID = c.CompanyID
	WHERE scr.ServiceCompanyID = @ServiceCompanyID
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICE_TYPES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICE_TYPES] AS' 
END
GO
-- Tipos de serviços prestados pela companhia
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICE_TYPES]
	@ServiceCompanyID int
AS
BEGIN
	SELECT DISTINCT 
		c.CompanyCode ServiceCompanyCode,
		st.ServiceTypeID,
		st.ServiceTypeCode,
		st.ServiceTypeDescription
	FROM dbo.RD_SERVICE_TYPE st WITH(NOLOCK)
	INNER JOIN
		dbo.RD_SERVICE s WITH(NOLOCK)
	ON	s.ServiceTypeID = st.ServiceTypeID
	INNER JOIN
		dbo.RD_SERVICE_COMPANY_SERVICE_COST scsc WITH(NOLOCK)
	ON	scsc.ServiceID = s.ServiceID
	INNER JOIN
		dbo.RD_COMPANY c WITH(NOLOCK)
	ON	c.CompanyID = scsc.ServiceCompanyID
	WHERE scsc.ServiceCompanyID = @ServiceCompanyID
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
	@ServiceTypeID int
AS
BEGIN
	SELECT 
		c.CompanyCode ServiceCompanyCode,
		s.ServiceID,
		s.ServiceDescription, --Descrição do Serviço
		s.ServiceCode, --Código do Serviço
		scsc.CostDate, --Data de efectivação
		scsc.ServiceCost, --Custo do Serviço
		scsc.Formula, --Formula de Cálculo
		st.ServiceTypeCode, --Código do Tipo de Serviço
		st.ServiceTypeDescription, --Tipo de Serviço
		s.MatchCode --Critério de enquadramento
	FROM dbo.RD_SERVICE_TYPE st WITH(NOLOCK)
	INNER JOIN
		dbo.RD_SERVICE s WITH(NOLOCK)
	ON	s.ServiceTypeID = st.ServiceTypeID
	INNER JOIN
		dbo.RD_SERVICE_COMPANY_SERVICE_COST scsc WITH(NOLOCK)
	ON	scsc.ServiceID = s.ServiceID
	INNER JOIN
		dbo.RD_COMPANY c WITH(NOLOCK)
	ON	c.CompanyID = scsc.ServiceCompanyID
	WHERE scsc.ServiceCompanyID = @ServiceCompanyID
		AND st.ServiceTypeID = @ServiceTypeID
	ORDER BY scsc.CostDate DESC, st.ServiceTypeCode ASC, s.ServiceCode ASC
END
GO
