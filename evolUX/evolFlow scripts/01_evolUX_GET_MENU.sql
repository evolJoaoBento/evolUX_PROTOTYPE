IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_PERMISSIONS]') AND type in (N'U'))
ALTER TABLE [dbo].[evolUX_PERMISSIONS] DROP CONSTRAINT [FK_evolUX_PERMISSIONS_evolUX_ACTIONS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_PERMISSIONS]') AND type in (N'U'))
ALTER TABLE [dbo].[evolUX_PERMISSIONS] DROP CONSTRAINT [DF_evolUX_PERMISSIONS_FlowType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_PERMISSIONS]') AND type in (N'U'))
DROP TABLE [dbo].[evolUX_PERMISSIONS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_ACTIONS]') AND type in (N'U'))
ALTER TABLE [dbo].[evolUX_ACTIONS] DROP CONSTRAINT [DF_evolUX_ACTIONS_evolGUI_TypeID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_ACTIONS]') AND type in (N'U'))
DROP TABLE [dbo].[evolUX_ACTIONS]
GO
CREATE TABLE [dbo].[evolUX_ACTIONS](
	[ActionID] [int] NOT NULL,
	[ActionTypeID] [int] NOT NULL,
	[LocalizationKey] [varchar](50) NULL,
	[Description] [varchar](255) NULL,
	[ParentActionID] [int] NULL,
	[DefaultOrder] [int] NULL,
	[HistoryFlag] [bit] NULL,
	[evolGUI_ActionID] [int] NULL,
	[evolGUI_TypeID] [int] NOT NULL CONSTRAINT DF_evolUX_ACTIONS_evolGUI_TypeID DEFAULT (0),
 CONSTRAINT [PK_evolUX_ACTIONS] PRIMARY KEY CLUSTERED 
(
	[ActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, Description, ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
SELECT ActionID, ActionTypeID, NULL, [Description], ParActionID, NULL, HistoryFlag, ActionID, evolGUI_TypeID
FROM ACTIONS
WHERE (ActionTypeID = 0
		OR (ActionTypeID = 1 AND parActionID in (SELECT ActionID FROM Actions WHERE ActionTypeID = 0))
		OR (ActionTypeID = 1 AND parActionID in (SELECT ActionID FROM Actions WHERE(ActionTypeID = 1 AND parActionID in (SELECT ActionID FROM Actions WHERE ActionTypeID = 0)))))
	AND [Description] not in ('Estados de Jobs','Estado de Jobs Activos')
GO
CREATE TABLE [dbo].[evolUX_PERMISSIONS](
	[ActionID] [int] NOT NULL,
	[ProfileID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
	[FlowID] [int] NULL,
	[TaskID] [int] NULL,
	[ActionOrder] [int] NULL,
	[FlowType] [int] NULL,
	[Mandatory] bit NOT NULL CONSTRAINT DF_evolUX_PERMISSIONS_Mandatory DEFAULT (0),
 CONSTRAINT [PK_evolUX_PERMISSIONS] PRIMARY KEY CLUSTERED 
(
	[ActionID] ASC,
	[ProfileID] ASC,
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[evolUX_PERMISSIONS] ADD  CONSTRAINT [DF_evolUX_PERMISSIONS_FlowType]  DEFAULT (0) FOR [FlowType]
GO
ALTER TABLE [dbo].[evolUX_PERMISSIONS]  WITH CHECK ADD  CONSTRAINT [FK_evolUX_PERMISSIONS_evolUX_ACTIONS] FOREIGN KEY([ActionID])
REFERENCES [dbo].[evolUX_ACTIONS] ([ActionID])
GO
IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'RefreshToken' AND OBJECT_ID = OBJECT_ID(N'USERS'))
BEGIN
	ALTER TABLE dbo.USERS
	ADD [RefreshToken] varchar(500) NULL
END
GO
IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'RefreshTokenExpiryTime' AND OBJECT_ID = OBJECT_ID(N'USERS'))
BEGIN
	ALTER TABLE dbo.USERS
	ADD [RefreshTokenExpiryTime] datetime NULL -- [datetime2](7)
END
GO
IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'Language' AND OBJECT_ID = OBJECT_ID(N'USERS'))
BEGIN
	ALTER TABLE dbo.USERS
	ADD [Language] varchar(10) NULL
END
GO
IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'Active' AND OBJECT_ID = OBJECT_ID(N'USERS'))
BEGIN
	ALTER TABLE dbo.USERS
	ADD Active bit NOT NULL CONSTRAINT DF_USERS_Active DEFAULT (1)
END
GO
UPDATE dbo.USERS
SET Active = 1
WHERE UserType <> 'OFF'
GO
UPDATE dbo.USERS
SET Active = 0
WHERE UserType = 'OFF'
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USERS_DEACTIVATE]') AND type in (N'TR'))
BEGIN
	DROP TRIGGER dbo.USERS_DEACTIVATE 
END
GO
-- =============================================
-- Author:		evolSolutions
-- Create date: 2023/01/10
-- Description:	Deactivate Users
-- =============================================
CREATE TRIGGER dbo.USERS_DEACTIVATE 
   ON  dbo.USERS 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	IF UPDATE(UserType)
	BEGIN
		UPDATE USERS
		SET Active = CASE UPPER(i.UserType) WHEN 'OFF' THEN 0 ELSE 1 END 
		FROM inserted i
		WHERE i.Active <> (CASE UPPER(i.UserType) WHEN 'OFF' THEN 0 ELSE 1 END)
	END
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
		(SELECT pa.ActionID as [ActionIDLevel1], pa.[Description] as [DescriptionLevel1], ISNULL(pa.[LocalizationKey], pa.[Description]) as [LocalizationKeyLevel1], a.ActionID as [ActionIDLevel2], a.[Description] as [DescriptionLevel2], ISNULL(a.[LocalizationKey], a.[Description]) as [LocalizationKeyLevel2], 
			CASE WHEN a.evolGUI_TypeID = 1 THEN (SELECT [URL] FROM ACTIONS WHERE ActionID = a.evolGUI_ActionID) ELSE NULL END as [URLLevel2], 
			pa.DefaultOrder ActionOrderLevel1, MAX(ISNULL(p.ActionOrder, a.DefaultOrder)) ActionOrderLevel2 
		FROM
			[evolUX_ACTIONS] a WITH(NOLOCK)
		INNER JOIN
			[evolUX_ACTIONS] pa WITH(NOLOCK)
		ON a.ParentActionID = pa.ActionID
		INNER JOIN
			[evolUX_PERMISSIONS] p WITH(NOLOCK)
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
		GROUP BY pa.ActionID, pa.[Description], pa.[LocalizationKey], a.ActionID, a.[Description], a.[LocalizationKey], a.evolGUI_TypeID, a.evolGUI_ActionID, pa.DefaultOrder) x  
	LEFT OUTER JOIN 
		(SELECT a.ActionID, a.[Description], ISNULL(a.[LocalizationKey], a.[Description]) as [LocalizationKey], 
			CASE WHEN a.evolGUI_TypeID = 1 THEN (SELECT [URL] FROM ACTIONS WHERE ActionID = a.evolGUI_ActionID) ELSE NULL END [URL], a.ParentActionID, a.ActionTypeID, MAX(ISNULL(p.ActionOrder,a.DefaultOrder)) ActionOrderLevel3
		FROM 
			[evolUX_ACTIONS] a WITH(NOLOCK)
		INNER JOIN
			[evolUX_PERMISSIONS] p WITH(NOLOCK)
		ON a.ActionID = p.ActionID
		WHERE  p.ProfileID in (SELECT ID FROM @ProfileList)
			AND ISNULL(a.evolGUI_TypeID,0) >= 0
		GROUP BY a.ActionID, a.evolGUI_TypeID, a.evolGUI_ActionID, 
			a.[Description], a.[LocalizationKey], a.ParentActionID, a.ActionTypeID, a.evolGUI_TypeID) z  
	ON x.ActionIDLevel2 = z.ParentActionID  
		AND z.ActionTypeID = 1
	ORDER BY x.ActionOrderLevel1, x.ActionIDLevel1, x.ActionOrderLevel2, x.ActionIDLevel2, ISNULL(z.ActionOrderLevel3,x.ActionOrderLevel2) ASC
END
GO
UPDATE [evolUX_ACTIONS]
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
	ELSE LocalizationKey END
FROM [evolUX_ACTIONS]
WHERE ActionTypeID = 0
GO
UPDATE [evolUX_ACTIONS]
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
	ELSE LocalizationKey END
FROM [evolUX_ACTIONS]
WHERE ActionTypeID = 0
GO
UPDATE [evolUX_ACTIONS]
SET LocalizationKey = 'Action' + CASE RTRIM(LTRIM([Description]))
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
	WHEN 'Mudar Password' THEN 'UserChangePassword'
	WHEN 'O Meu Perfil' THEN 'UserProfile'
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
	ELSE LocalizationKey END
FROM [evolUX_ACTIONS]
WHERE ActionTypeID = 1 AND ParentActionID in (SELECT ActionID FROM [evolUX_ACTIONS] WHERE  ActionTypeID = 0)
GO
UPDATE [evolUX_ACTIONS]
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Recursos de Processos' THEN 'ProcessResources'
	ELSE NULL END
FROM [evolUX_ACTIONS]
WHERE ActionTypeID = 1 AND ParentActionID = (SELECT ActionID FROM [evolUX_ACTIONS] WHERE LocalizationKey = 'ActionResources')
GO
UPDATE [evolUX_ACTIONS]
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Produção' THEN 'ProductionFlows'
	WHEN 'Backups' THEN 'BackupFlows'
	WHEN 'Expurgos' THEN 'PurgeFlows'
	ELSE LocalizationKey END
FROM [evolUX_ACTIONS]
WHERE ActionTypeID = 1 AND ParentActionID = (SELECT ActionID FROM [evolUX_ACTIONS] WHERE LocalizationKey = 'ActionRegistJob')
GO
UPDATE [evolUX_ACTIONS]
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Aprovações' THEN 'UserAprovals'
	ELSE LocalizationKey END
FROM [evolUX_ACTIONS]
WHERE ActionTypeID = 1 AND ParentActionID = (SELECT ActionID FROM [evolUX_ACTIONS] WHERE LocalizationKey = 'ActionUserTasks')
GO
UPDATE [evolUX_ACTIONS]
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Adicionar' THEN 'AddUser'
	WHEN 'Adicionar um Utilizador' THEN 'AddUser'
	WHEN 'Apagar' THEN 'DeleteUser'
	WHEN 'Apagar um Utilizador' THEN 'DeleteUser'
	WHEN 'Perfis' THEN 'UserProfiles'
	ELSE LocalizationKey END
FROM [evolUX_ACTIONS]
WHERE ActionTypeID = 1 AND ParentActionID = (SELECT ActionID FROM [evolUX_ACTIONS] WHERE LocalizationKey = 'ActionUsers')
GO
UPDATE [evolUX_ACTIONS]
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Adicionar um Utilizador' THEN 'AddUser'
	WHEN 'Apagar um Utilizador' THEN 'DeleteUser'
	WHEN 'O Meu Perfil' THEN 'UserProfiles'
	WHEN 'Meu Perfil' THEN 'UserProfiles'
	ELSE LocalizationKey END
FROM [evolUX_ACTIONS]
WHERE ActionTypeID = 1 AND ParentActionID = (SELECT ActionID FROM [evolUX_ACTIONS] WHERE LocalizationKey = 'ActionMenuEvolFlowConfig')
GO
UPDATE [evolUX_ACTIONS]
SET LocalizationKey = 'Action' + CASE [Description] 
	WHEN 'Consulta' THEN 'ProfilesList'
	WHEN 'Acessos a Páginas' THEN 'Permissions'
	ELSE LocalizationKey END
FROM [evolUX_ACTIONS]
WHERE ActionTypeID = 1 AND ParentActionID = (SELECT ActionID FROM [evolUX_ACTIONS] WHERE LocalizationKey = 'ActionProfiles')
GO
INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
SELECT ea.ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType
FROM [PERMISSIONS] p
INNER JOIN
	[ACTIONS] a
ON a.ActionID = p.ActionID
INNER JOIN
	[evolUX_ACTIONS] ea
ON ea.evolGUI_ActionID = a.ActionID
GO
INSERT INTO [evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
SELECT ea.ActionID, p.ProfileID, 
	(SELECT ISNULL(MAX(PermissionID),0) + 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = ea.ActionID AND ProfileID = p.ProfileID),
	NULL, NULL, NULL, 0
FROM PROFILES p
INNER JOIN
	[evolUX_ACTIONS] ea
ON p.NrChildren = 0
	AND ea.LocalizationKey = 'ActionUserProfiles'
GO
INSERT INTO [evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
SELECT ea1.ActionID, p.ProfileID, 
	(SELECT ISNULL(MAX(PermissionID),0) + 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = ea1.ActionID AND ProfileID = p.ProfileID),
	NULL, NULL, NULL, 0
FROM PROFILES p
INNER JOIN
	[evolUX_ACTIONS] ea1
ON ea1.LocalizationKey = 'ActionProfilesList'
	AND p.ProfileID in (SELECT pm.ProfileID
							FROM [PERMISSIONS] pm
							INNER JOIN
								ACTIONS a
							ON a.ActionID = pm.ActionID
							INNER JOIN
								[evolUX_ACTIONS] ea
							ON ea.evolGUI_ActionID = a.ActionID
							WHERE ea.LocalizationKey = 'ActionProfiles')
GO