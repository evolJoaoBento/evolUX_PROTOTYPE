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
	WHEN 'Configurações evolFlow' THEN 'EvolFlowConfig'
	WHEN 'Processamento' THEN 'Execution'
	WHEN 'Opções de Utilizador' THEN 'UserSettings'
	WHEN 'Suporte' THEN 'Support'
	WHEN 'Erros' THEN 'Error'
	WHEN 'Gestão de Tabelas <q style="FONT-VARIANT: normal;TEXT-TRANSFORM: none">evolDP</q>' THEN 'EvolDPConfig'
	WHEN 'Configurações evolDP' THEN 'EvolDPConfig'	
	WHEN 'Relatórios' THEN CASE WHEN ActionID = 50 THEN 'ClientReporting' ELSE 'Reporting' END
	WHEN 'Relatórios evolDP' THEN 'Reporting'
	WHEN 'Finishing' THEN 'Finishing'
	WHEN 'Produção evolDP' THEN 'Finishing'
	WHEN 'Produção' THEN CASE WHEN ActionID = 30 THEN 'ClientPages' ELSE NULL END
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
	WHEN 'Registo Automático de Jobs' THEN 'FlowsSchedulerRules'
	WHEN 'Registo Automático de Jobs' THEN 'FlowsSchedulerRules'
	WHEN 'Registo Automático de Jobs' THEN 'FlowsSchedulerRules'
	WHEN 'Registo Automático de Jobs' THEN 'FlowsSchedulerRules'
	WHEN 'Acessos a Páginas' THEN 'Permissions'
	WHEN 'Controlo de Jobs' THEN 'JobControl'
	WHEN 'Executar Fluxos' THEN 'RegistJob'
	WHEN 'Tarefas de Utilizador' THEN 'UserTasks'
	WHEN 'Gestão de Erros' THEN 'ErrorManagement'
	WHEN 'Mudar Password' THEN 'ChangePassword'
	WHEN 'Utilizadores' THEN 'Users'
	WHEN 'Perfis' THEN 'Profiles'
	WHEN 'Informação de Suporte' THEN 'SupportInformation'
	WHEN 'Páginas de Ajuda' THEN 'HelpPages'
	WHEN 'Marcar Ficheiros em Erro' THEN 'MarkFilesinError'
	WHEN 'Marcar Documentos em Erro' THEN 'MarkDocumentsinError'
	WHEN 'Marcar Intervalos de Documentos em Erro' THEN 'MarkDocumentsRangeinError'
	WHEN 'Tipos de Documento' THEN 'DocumentTypification'
	WHEN 'Gamas de Envelopes' THEN 'EnvelopeRange'
	WHEN 'Materiais' THEN 'Materials'
	WHEN 'Companhias de Serviços' THEN 'ServiceCompanies'
	WHEN 'Serviços por Companhia' THEN 'ServiceCompanyServices'
	WHEN 'Companhias de Expedição' THEN 'ExpeditionCompanies'
	WHEN 'Códigos Tratamento/Expedição' THEN 'ExpCodes'
	WHEN 'Versões de Projectos' THEN 'ProjectVersions'
	WHEN 'Parâmetros Expurgos' THEN 'PurgeParameters'
	WHEN 'Tipos de Expedição' THEN 'ExpeditionTypes'
	WHEN 'Zonas de Expedição' THEN 'ExpeditionZones'
	WHEN 'Integrações' THEN 'IntegrationReport'
	WHEN 'Retenções' THEN 'RetentionReport'
	WHEN 'Produção Pendente' THEN 'PendingProductionReport'
	WHEN 'Produção' THEN 'ProductionReport'
	WHEN 'Recuperações' THEN 'RecoverReport'
	WHEN 'Controlo de Produção / Consumíveis' THEN 'MaterialProductionRepor'
	WHEN 'Controlo de Facturação' THEN 'BillingReport'
	WHEN 'Expedição' THEN 'ExpeditionReport'
	WHEN 'Estado de Produção / Imprimir' THEN 'ProductionStatus'
	WHEN 'Registos Pendentes' THEN 'PendingRegistries'
	WHEN 'Objecto Postal' THEN 'PostalObject'
	WHEN 'Impressão Concluída' THEN 'ConcludedPrint'
	WHEN 'Envelopagem Concluída' THEN 'ConcludedFullfill'
	WHEN 'Recuperações Parciais' THEN 'PartialRecover'
	WHEN 'Recuperações Totais' THEN 'TotalRecover'
	WHEN 'Recuperações Pendentes' THEN 'PendingRecover'
	WHEN 'Recuperações: Detalhe de Registos (AR)' THEN 'RegistDetailRecover'
	WHEN 'Guias de Expedição' THEN 'ExpeditionReport'
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
	WHEN 'Produção' THEN 'ProductionFlows'
	WHEN 'Backups' THEN 'BackupFlows'
	WHEN 'Expurgos' THEN 'PurgeFlows'
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 1 AND parActionID = (SELECT ActionID FROM ACTIONS WHERE LocalizationKey = 'ActionRegistJob')
GO
UPDATE ACTIONS
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Aprovações' THEN 'UserAprovals'
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
	WHEN 'Acessos a Páginas' THEN 'Permissions'
	ELSE NULL END
FROM ACTIONS
WHERE ActionTypeID = 1 AND parActionID = (SELECT ActionID FROM ACTIONS WHERE LocalizationKey = 'ActionProfiles')
GO

