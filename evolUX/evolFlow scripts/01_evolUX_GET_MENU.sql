IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'Language' AND OBJECT_ID = OBJECT_ID(N'USERS'))
BEGIN
	ALTER TABLE dbo.ACTIONS
	ADD [Language] varchar(50) NULL DEFAULT (0)
END
GO
IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'LocalizationKey' AND OBJECT_ID = OBJECT_ID(N'ACTIONS'))
BEGIN
	ALTER TABLE dbo.ACTIONS
	ADD [LocalizationKey] varchar(255) NULL 
END
GO
IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'DefaultOrder' AND OBJECT_ID = OBJECT_ID(N'ACTIONS'))
BEGIN
	ALTER TABLE dbo.ACTIONS
	ADD [DefaultOrder] int NULL 
END
GO
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_GET_MENU]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[evolUX_GET_MENU] AS'
END
GO
ALTER  PROCEDURE [dbo].[evolUX_GET_MENU]
	@ProfileList IDList READONLY
AS
BEGIN
	SET NOCOUNT ON
	SELECT DISTINCT x.[ActionIDLevel1], x.[DescriptionLevel1], x.[LocalizationKeyLevel1], x.ActionIDLevel2, x.[DescriptionLevel2], x.[LocalizationKeyLevel2], x.[URLLevel2], z.[ActionID] as [ActionIDLevel3], z.[Description] as [DescriptionLevel3], z.[LocalizationKey] as [LocalizationKeyLevel3], z.[URL] as [URLLevel3], x.ActionOrderLevel1, x.ActionOrderLevel2, ISNULL(z.ActionOrderLevel3, x.ActionOrderLevel2) [ActionOrderLevel3] 
	FROM 
		(SELECT pa.ActionID as [ActionIDLevel1], pa.[Description] as [DescriptionLevel1], ISNULL(pa.[LocalizationKey],pa.[Description]) as [LocalizationKeyLevel1], a.ActionID as [ActionIDLevel2], a.[Description] as [DescriptionLevel2], ISNULL(a.[LocalizationKey],a.[Description]) as [LocalizationKeyLevel2], CASE WHEN ISNULL(a.evolGUI_TypeID, 0) = 1 THEN a.[URL] END as [URLLevel2], pa.DefaultOrder ActionOrderLevel1, MAX(ISNULL(p.ActionOrder, a.DefaultOrder)) ActionOrderLevel2 
		FROM
			ACTIONS a WITH(NOLOCK)
		INNER JOIN
			ACTIONS pa WITH(NOLOCK)
		ON a.ParActionID = pa.ActionID
		INNER JOIN
			[PERMISSIONS] p WITH(NOLOCK)
		ON p.ActionID = a.ActionID
		WHERE 	a.ActionTypeID = 1   
			AND ISNULL(a.evolGUI_TypeID,0) >= 0
			AND ISNULL(pa.evolGUI_TypeID,0) >= 0
			AND p.ProfileID in (SELECT ID FROM @ProfileList)
			AND p.FlowType in (SELECT DISTINCT FlowType
								FROM BACKUP_JOBS WITH(NOLOCK)
								UNION
								SELECT FlowType
								FROM FLOW_TYPE WITH(NOLOCK)
								WHERE NOT EXISTS(SELECT TOP 1 1 FROM BACKUP_JOBS WITH(NOLOCK)))   
			AND pa.ActionTypeID = 0  
		GROUP BY pa.ActionID, pa.[Description], pa.[LocalizationKey], a.ActionID, a.[Description], a.[LocalizationKey], a.[URL], a.evolGUI_TypeID, pa.DefaultOrder) x  
	LEFT OUTER JOIN 
		(SELECT a.ActionID, a.[Description], ISNULL(a.[LocalizationKey], a.[Description]) as [LocalizationKey], CASE WHEN ISNULL(a.evolGUI_TypeID, 0) = 1 THEN a.[URL] END [URL], a.ParActionID, a.ActionTypeID, MAX(ISNULL(p.ActionOrder,a.DefaultOrder)) ActionOrderLevel3
		FROM 
			ACTIONS a WITH(NOLOCK)
		INNER JOIN
			[PERMISSIONS] p WITH(NOLOCK)
		ON a.ActionID = p.ActionID
		WHERE  p.ProfileID in (SELECT ID FROM @ProfileList)
			AND ISNULL(a.evolGUI_TypeID,0) >= 0
		GROUP BY a.ActionID, a.[URL], a.[Description], a.[LocalizationKey], a.ParActionID, a.ActionTypeID, a.evolGUI_TypeID) z  
	ON x.ActionIDLevel2 = z.ParActionID  
		AND z.ActionTypeID = 1
	ORDER BY x.ActionOrderLevel1, x.ActionIDLevel1, x.ActionOrderLevel2, x.ActionIDLevel2, ISNULL(z.ActionOrderLevel3,x.ActionOrderLevel2) ASC
END
GO
UPDATE ACTIONS
SET LocalizationKey = 'ActionMenu' + CASE [Description] 
	WHEN 'Configura��es evolFlow' THEN 'EvolFlowConfig'
	WHEN 'Processamento' THEN 'Execution'
	WHEN 'Op��es de Utilizador' THEN 'UserSettings'
	WHEN 'Suporte' THEN 'Support'
	WHEN 'Erros' THEN 'Error'
	WHEN 'Gest�o de Tabelas <q style="FONT-VARIANT: normal;TEXT-TRANSFORM: none">evolDP</q>' THEN 'EvolDPConfig'
	WHEN 'Configura��es evolDP' THEN 'EvolDPConfig'	
	WHEN 'Relat�rios' THEN CASE WHEN ActionID = 50 THEN 'ClientReporting' ELSE 'Reporting' END
	WHEN 'Relat�rios evolDP' THEN 'Reporting'
	WHEN 'Finishing' THEN 'Finishing'
	WHEN 'Produ��o evolDP' THEN 'Finishing'
	WHEN 'Produ��o' THEN CASE WHEN ActionID = 30 THEN 'ClientPages' ELSE NULL END
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 0
GO
UPDATE ACTIONS
SET [DefaultOrder] = CASE [LocalizationKey] 
	WHEN 'ActionMenuEvolFlowConfig' THEN 200
	WHEN 'ActionMenuExecution' THEN 300
	WHEN 'ActionMenuUserSettings' THEN 100
	WHEN 'ActionMenuSupport' THEN 400
	WHEN 'ActionMenuClientPages' THEN 900
	WHEN 'ActionMenuClientReporting' THEN 910
	WHEN 'ActionMenuError' THEN 1500
	WHEN 'ActionMenuEvolDPConfig' THEN 1100
	WHEN 'ActionMenuReporting' THEN 1300
	WHEN 'ActionMenuFinishing' THEN 1400
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 0
GO
UPDATE ACTIONS
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Recursos' THEN 'Resources'
	WHEN 'Fluxos Configurados' THEN 'ConfiguredFlows'
	WHEN 'Modelo de Dados Activo' THEN 'ActiveDataModel'
	WHEN 'Registo Autom�tico de Jobs' THEN 'FlowsSchedulerRules'
	WHEN 'Registo Autom�tico de Jobs' THEN 'FlowsSchedulerRules'
	WHEN 'Registo Autom�tico de Jobs' THEN 'FlowsSchedulerRules'
	WHEN 'Registo Autom�tico de Jobs' THEN 'FlowsSchedulerRules'
	WHEN 'Acessos a P�ginas' THEN 'Permissions'
	WHEN 'Controlo de Jobs' THEN 'JobControl'
	WHEN 'Executar Fluxos' THEN 'RegistJob'
	WHEN 'Tarefas de Utilizador' THEN 'UserTasks'
	WHEN 'Gest�o de Erros' THEN 'ErrorManagement'
	WHEN 'Mudar Password' THEN 'ChangePassword'
	WHEN 'Utilizadores' THEN 'Users'
	WHEN 'Perfis' THEN 'Profiles'
	WHEN 'Informa��o de Suporte' THEN 'SupportInformation'
	WHEN 'P�ginas de Ajuda' THEN 'HelpPages'
	WHEN 'Marcar Ficheiros em Erro' THEN 'MarkFilesinError'
	WHEN 'Marcar Documentos em Erro' THEN 'MarkDocumentsinError'
	WHEN 'Marcar Intervalos de Documentos em Erro' THEN 'MarkDocumentsRangeinError'
	WHEN 'Tipos de Documento' THEN 'DocumentTypification'
	WHEN 'Gamas de Envelopes' THEN 'EnvelopeRange'
	WHEN 'Materiais' THEN 'Materials'
	WHEN 'Companhias de Servi�os' THEN 'ServiceCompanies'
	WHEN 'Servi�os por Companhia' THEN 'ServiceCompanyServices'
	WHEN 'Companhias de Expedi��o' THEN 'ExpeditionCompanies'
	WHEN 'C�digos Tratamento/Expedi��o' THEN 'ExpCodes'
	WHEN 'Vers�es de Projectos' THEN 'ProjectVersions'
	WHEN 'Par�metros Expurgos' THEN 'PurgeParameters'
	WHEN 'Tipos de Expedi��o' THEN 'ExpeditionTypes'
	WHEN 'Zonas de Expedi��o' THEN 'ExpeditionZones'
	WHEN 'Integra��es' THEN 'IntegrationReport'
	WHEN 'Reten��es' THEN 'RetentionReport'
	WHEN 'Produ��o Pendente' THEN 'PendingProductionReport'
	WHEN 'Produ��o' THEN 'ProductionReport'
	WHEN 'Recupera��es' THEN 'RecoverReport'
	WHEN 'Controlo de Produ��o / Consum�veis' THEN 'MaterialProductionRepor'
	WHEN 'Controlo de Factura��o' THEN 'BillingReport'
	WHEN 'Expedi��o' THEN 'ExpeditionReport'
	WHEN 'Estado de Produ��o / Imprimir' THEN 'ProductionStatus'
	WHEN 'Registos Pendentes' THEN 'PendingRegistries'
	WHEN 'Objecto Postal' THEN 'PostalObject'
	WHEN 'Impress�o Conclu�da' THEN 'ConcludedPrint'
	WHEN 'Envelopagem Conclu�da' THEN 'ConcludedFullfill'
	WHEN 'Recupera��es Parciais' THEN 'PartialRecover'
	WHEN 'Recupera��es Totais' THEN 'TotalRecover'
	WHEN 'Recupera��es Pendentes' THEN 'PendingRecover'
	WHEN 'Recupera��es: Detalhe de Registos (AR)' THEN 'RegistDetailRecover'
	WHEN 'Guias de Expedi��o' THEN 'ExpeditionReport'
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 1 AND parActionID in (SELECT ActionID FROM ACTIONS WHERE  ActionTypeID = 0)
GO
UPDATE ACTIONS
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Recursos de Processos' THEN 'ProcessResources'
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 1 AND parActionID = (SELECT ActionID FROM ACTIONS WHERE LocalizationKey = 'ActionResources')
GO
UPDATE ACTIONS
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Produ��o' THEN 'ProductionFlows'
	WHEN 'Backups' THEN 'BackupFlows'
	WHEN 'Expurgos' THEN 'PurgeFlows'
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 1 AND parActionID = (SELECT ActionID FROM ACTIONS WHERE LocalizationKey = 'ActionRegistJob')
GO
UPDATE ACTIONS
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Aprova��es' THEN 'UserAprovals'
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 1 AND parActionID = (SELECT ActionID FROM ACTIONS WHERE LocalizationKey = 'ActionUserTasks')
GO
UPDATE ACTIONS
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Adicionar' THEN 'AddUser'
	WHEN 'Apagar' THEN 'DeleteUser'
	WHEN 'Perfis' THEN 'UserProfiles'
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 1 AND parActionID = (SELECT ActionID FROM ACTIONS WHERE LocalizationKey = 'ActionUsers')
GO
UPDATE ACTIONS
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Acessos a P�ginas' THEN 'Permissions'
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 1 AND parActionID = (SELECT ActionID FROM ACTIONS WHERE LocalizationKey = 'ActionProfiles')
GO

