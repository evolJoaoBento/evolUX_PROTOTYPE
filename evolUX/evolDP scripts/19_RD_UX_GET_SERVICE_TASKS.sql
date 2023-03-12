IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_TASKS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASKS] AS' 
END
GO

ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASKS]
AS
BEGIN
	SELECT ServiceTaskID, ServiceTaskCode, [Description] ServiceTaskDesc
	FROM RD_SERVICE_TASK
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
		st.ServiceTypeDescription --Descrição do Tipo de Serviço
	FROM
		RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
	INNER JOIN
		RD_SERVICE_TYPE st
	ON	st.ServiceTypeID = stst.ServiceTypeID
	WHERE stst.ServiceTaskID = @ServiceTaskID
	ORDER BY st.ServiceTypeCode
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCOMPANY_SERVICE_TASKS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_SERVICE_TASKS] AS' 
END
GO
-- Listar Tipo Tratamento/Expedição (ExpCode) - Alterar/apagar/adiconar?
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCOMPANY_SERVICE_TASKS]
AS
BEGIN
	SELECT e.ExpCode, --Código Tratamento/Expedição
		e.[Description] ExpCodeDesc,
		c.CompanyCode ExpCompanyCode, --Companhia de Expedição
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
	ORDER BY est.ExpCode
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPCODE_EXPCENTER_SELECTIONS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPCODE_EXPCENTER_SELECTIONS] AS' 
END
GO
--Carateristicas do Tipo Tratamento/expedição
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPCODE_EXPCENTER_SELECTIONS]
	@ExpCode varchar(10)
AS
BEGIN
	SELECT 
		eee.ExpCode,
		eee.ExpCenterCode,
		eee.ServiceCompanyID,
		c.CompanyCode ServiceCompanyCode,
		eee.ExpeditionZone,
		ez.[Description] ExpeditionZoneDesc
	FROM
		RD_EXPEDITION_EXPCENTER_EXPZONE eee WITH(NOLOCK)
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK)
	ON	eee.ServiceCompanyID = c.CompanyID
	INNER JOIN
		RD_EXPEDITION_ZONE ez WITH(NOLOCK)
	ON	eee.ExpeditionZone = ez.ExpeditionZone
	WHERE eee.ExpCode = @ExpCode
	ORDER BY eee.ExpCenterCode
END
GO





