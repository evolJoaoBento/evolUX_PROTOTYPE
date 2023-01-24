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
--Configura��o de restri��es (listar/alterar)
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_RESTRICTIONS]
	@ServiceCompanyID int
AS
BEGIN
	SELECT
		c.CompanyCode ServiceCompanyCode,
		mt.MaterialTypeID,
		mt.MaterialTypeDescription,
		scr.MaterialPosition, --N� M�ximo de Bandejas/Esta��es dispon�veis
		scr.FileSheetsCutoffLevel,  --N�vel M�ximo de Folhas por Ficheiro, NULL ==> N�o Aplic�vel
		CASE WHEN mt.MaterialTypeCode = 'STATION' THEN CAST(scr.RestrictionMode as varchar) ELSE 'NA' END RestrictionMode --A��o em caso de exceder o n� m�ximo de esta��es, '0' ==> 'Impede Produ��o do objecto postal', 1 ==> 'Obriga a envelopagem manual do objecto postal'
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
-- Tipos de servi�os prestados pela companhia
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
--Configura��o de custos para os servi�os prestados pela companhia - Alterar os j� existentes ou  apagar (adiconar????)
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_COMPANY_SERVICE_COSTS]
	@ServiceCompanyID int,
	@ServiceTypeID int
AS
BEGIN
	SELECT 
		c.CompanyCode ServiceCompanyCode,
		s.ServiceID,
		s.ServiceDescription, --Descri��o do Servi�o
		s.ServiceCode, --C�digo do Servi�o
		scsc.CostDate, --Data de efectiva��o
		scsc.ServiceCost, --Custo do Servi�o
		scsc.Formula, --Formula de C�lculo
		st.ServiceTypeCode, --C�digo do Tipo de Servi�o
		st.ServiceTypeDescription, --Tipo de Servi�o
		s.MatchCode --Crit�rio de enquadramento
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
