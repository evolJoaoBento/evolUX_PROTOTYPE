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
	AND [Description] not in ('Estados de Jobs','Estado de Jobs Activos','Adicionar Tipo de Documento','Adicionar Gamas de Envelopes')
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
DROP INDEX IF EXISTS [UX_RT_USERS_UserName] ON [dbo].[USERS]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_RT_USERS_UserName] ON [dbo].[USERS]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_GET_USER]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[evolUX_GET_USER] AS'
END
GO
ALTER  PROCEDURE [dbo].[evolUX_GET_USER]
	@UserName varchar(50) = NULL
AS
BEGIN
	SELECT UserID, UserName, RefreshToken, RefreshTokenExpiryTime, ISNULL([Language],'pt') [Language] 
	FROM [dbo].[USERS] WITH(NOLOCK)
	WHERE UserName = ISNULL(@UserName,UserName) AND Active = 1
END
GO
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_UPDATE_USER]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[evolUX_UPDATE_USER] AS'
END
GO
ALTER  PROCEDURE [dbo].[evolUX_UPDATE_USER]
	@UserID int = NULL,
	@UserName varchar(50) = NULL,
	@Password varchar(500) = NULL,
	@UserType char(3) = NULL,
	@Language varchar(10) = NULL
AS
BEGIN
	SET NOCOUNT ON
	IF (@UserID is NULL AND @UserName is NULL)
	BEGIN
		SELECT -1 ErrorID, 'MissingUserIdentification' Error
		RETURN
	END

	IF (@UserID is NULL)
	BEGIN
		SELECT @UserID = UserID
		FROM [dbo].[USERS] WITH(NOLOCK)
		WHERE UserName = @UserName
	END

	IF (@UserID is NULL)
	BEGIN
		SELECT -2 ErrorID, 'UserNotFound' Error
		RETURN
	END

	IF (@UserType is NOT NULL)
	BEGIN
		UPDATE [dbo].[USERS]
		SET [UserType] = @UserType
		WHERE UserID = @UserID
	END

	UPDATE [dbo].[USERS]
	SET [Password] = ISNULL(@Password, [Password]),
		[Language] = ISNULL(@Language, [Language])
	WHERE UserID = @UserID AND Active = 1

	IF (@@ROWCOUNT > 0)
	BEGIN
		SELECT 0 ErrorID, 'Success' Error
	END
	ELSE
	BEGIN
		IF (EXISTS (SELECT TOP 1 1 FROM [dbo].[USERS] WHERE UserID = @UserID AND Active = 0))
		BEGIN
			SELECT -3 ErrorID, 'UserInactive' Error
		END
		ELSE
		BEGIN
			SELECT -4 ErrorID, 'NotSuccess' Error
		END
	END
END
GO
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_GET_USER_PROFILES]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[evolUX_GET_USER_PROFILES] AS'
END
GO
ALTER  PROCEDURE [dbo].[evolUX_GET_USER_PROFILES]
	@UserName varchar(50) = NULL
AS
BEGIN
	SELECT p.ProfileID, p.ParentProfileID, p.NrChildren, p.CompanyServer, p.[Description]
	FROM
		[dbo].[USERS] u WITH(NOLOCK)
	INNER JOIN
		[dbo].[USER_PROFILES] up WITH(NOLOCK)
	ON u.UserID = up.UserID
	INNER JOIN
		[dbo].[PROFILES] p WITH(NOLOCK)
	ON up.ProfileID = p.ProfileID
	WHERE u.UserName = @UserName AND u.Active = 1
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
			pa.DefaultOrder ActionOrderLevel1, MAX(ISNULL(a.DefaultOrder, p.ActionOrder)) ActionOrderLevel2 
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
			CASE WHEN a.evolGUI_TypeID = 1 THEN (SELECT [URL] FROM ACTIONS WHERE ActionID = a.evolGUI_ActionID) ELSE NULL END [URL], a.ParentActionID, a.ActionTypeID, MAX(ISNULL(a.DefaultOrder, p.ActionOrder)) ActionOrderLevel3
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
	WHEN 'Materiais' THEN 'Consumables'
	WHEN 'Companhias de Serviços' THEN 'ServiceCompanies'
	WHEN 'Serviços por Companhia' THEN 'ServicesProvided'
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
	WHEN 'Registos Pendentes' THEN 'PendingRegist'
	WHEN 'Objecto Postal' THEN 'PostalObject'
	WHEN 'Impressão Concluída' THEN 'ConcludedPrint'
	WHEN 'Envelopagem Concluída' THEN 'ConcludedFulfilment'
	WHEN 'Recuperações Parciais' THEN 'PartialRecover'
	WHEN 'Recuperações Totais' THEN 'TotalRecover'
	WHEN 'Recuperações Pendentes' THEN 'PendingRecover'
	WHEN 'Recuperações: Detalhe de Registos (AR)' THEN 'RegistDetailRecover'
	WHEN 'Guias de Expedição' THEN 'Expedition'
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
------------------------------------------------------------------
-- DOCUMENT
------------------------------------------------------------------
GO
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

SELECT @ParentLocalizationKey = 'ActionDocumentTypification'

UPDATE evolUX_Actions
SET DefaultOrder = 10
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int, [Description] varchar(255), evolGUIActionID int)

INSERT INTO #ChildActions
SELECT 'ActionDocCompanies', 10, 'Configurar Companhias de Documentos', evolGUI_ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

INSERT INTO #ChildActions
SELECT 'ActionDocCode', 20, 'Configurar Tipos de Documentos', evolGUI_ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

INSERT INTO #ChildActions
SELECT 'ActionExceptionLevels', 30, 'Configurar Exceções', NULL

INSERT INTO #ChildActions
SELECT 'ActionProjectVersions', 60, 'Versões de Projectos', ActionID
FROM ACTIONS
WHERE  [Description] like 'Vers_es de Projectos'

INSERT INTO #ChildActions
SELECT 'ActionPurgeParameters', 60, 'Parâmetros Expurgos', ActionID
FROM ACTIONS
WHERE  [Description] like 'Par_metros Expurgos'

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 1, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 1, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions
-----
SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExceptionLevels'

INSERT INTO #ChildActions
SELECT 'ActionExceptionLevel1ID', 10, 'Configurar Exceção Nível 1', ActionID
FROM ACTIONS
WHERE  [Description] like 'Configurar @PARAMETERS/ACTION/EXCEPTION/%'

INSERT INTO #ChildActions
SELECT 'ActionExceptionLevel2ID', 20, 'Configurar Exceção Nível 2', ActionID
FROM ACTIONS
WHERE  [Description] like 'Configurar @PARAMETERS/ACTION/EXCEPTION/%'

INSERT INTO #ChildActions
SELECT 'ActionExceptionLevel3ID', 30, 'Configurar Exceção Nível 3', ActionID
FROM ACTIONS
WHERE  [Description] like 'Configurar @PARAMETERS/ACTION/EXCEPTION/%'

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 1, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 1, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions
----

INSERT INTO #ChildActions
SELECT 'AddExceptionLevel', 0, 'Adicionar/Alterar Exceção', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar @PARAMETERS/ACTION/EXCEPTION/%'
	OR 
	  [Description] like 'Submeter @PARAMETERS/ACTION/EXCEPTION/%'
	OR 
	  [Description] like 'Adicionar @PARAMETERS/ACTION/EXCEPTION/%'

INSERT INTO #ChildActions
SELECT 'DeleteExceptionLevel', 0, 'Apagar Exceção', ActionID
FROM ACTIONS
WHERE [Description] like 'Apagar @PARAMETERS/ACTION/EXCEPTION/%'

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExceptionLevel1ID'


WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions

INSERT INTO #ChildActions
SELECT 'AddConstantParameter', 0, 'Adicionar/Alterar Parâmtero', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar Par_metro de Expurgo'
	OR 
	  [Description] like 'Submissão do Par_metro de Expurgo'
	OR 
	  [Description] like 'Adicionar Par_metro de Expurgo'

INSERT INTO #ChildActions
SELECT 'DeleteConstantParameter', 0, 'Apagar Parâmetro', NULL


DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionPurgeParameters'

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor
DELETE #ChildActions
DROP TABLE #ChildActions
GO
-----------
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int, [Description] varchar(255), evolGUIActionID int)

INSERT INTO #ChildActions
SELECT 'AddDocCode', 0, 'Adicionar/Alterar Tipo de Documento', ActionID
FROM ACTIONS
WHERE [Description] like 'Adicionar Tipo de Documento'
	OR 
	  [Description] like 'Alterar Tipo de Documento'
	OR 
	  [Description] like 'Adicionar Configura__o para Tipo de Documento'
	OR 
	  [Description] like 'Alterar Compatibilidades para Tipo de Documento'
	OR 
	  [Description] like 'Alteração de Compatibilidades para Tipo de Documento'

INSERT INTO #ChildActions
SELECT 'DeleteDocCode', 0, 'Apagar Tipo de Documento', ActionID
FROM ACTIONS
WHERE [Description] like 'Apagar Tipo de Documento'
	OR 
	  [Description] like 'Apagar Configura__o de Tipo de Documento'


INSERT INTO #ChildActions
SELECT 'ExportDocCode', 0, 'Exportar Configuração do Tipo de Documento ', evolGUIActionID
FROM #ChildActions
WHERE LocalizationKey = 'AddDocCode'

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionDocCode'


WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DROP TABLE #ChildActions
GO
----------------------------------------------------------------
-- MATERIALS
----------------------------------------------------------------
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

SELECT @ParentLocalizationKey = 'ActionConsumables'

UPDATE evolUX_Actions
SET DefaultOrder = 20
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int, [Description] varchar(255), evolGUIActionID int)

INSERT INTO #ChildActions
SELECT 'ActionMaterialType', 10, 'Configurar Tipos de Materiais', ActionID
FROM ACTIONS
WHERE  [Description] like 'Adicionar Material'

INSERT INTO #ChildActions
SELECT 'ActionMaterialManagement', 20, 'Configurar Materiais', ActionID
FROM ACTIONS
WHERE  [Description] in ('Lista de Materiais','Materiais')

INSERT INTO #ChildActions
SELECT 'ActionEnvelopeRange', 30, 'Configurar Gamas de Envelopes', ActionID
FROM ACTIONS
WHERE  [Description] in ('Gamas de Envelopes','Grupos de Gamas de Envelopes')

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 1, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 1, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions

INSERT INTO #ChildActions
SELECT 'AddMaterial', 0, 'Adicionar/Alterar Materiais', ActionID
FROM ACTIONS
WHERE [Description] in ('Alterar Grupo de Materiais', 'Adicionar Grupo de Materiais','Adicionar Material')
OR [Description] like 'Alterar Caracter_sticas do Material'

INSERT INTO #ChildActions
SELECT 'DeleteMaterial', 0, 'Apagar Materiais', NULL

INSERT INTO #ChildActions
SELECT DISTINCT 'ExportMaterial', 0, 'Exportar Configuração do Material ', evolGUIActionID
FROM #ChildActions
WHERE LocalizationKey = 'AddMaterial'

INSERT INTO #ChildActions
SELECT 'AddMaterialType', 0, 'Adicionar/Alterar Tipo de Materiais', ActionID
FROM ACTIONS
WHERE [Description] in ('Alterar Grupo de Materiais', 'Adicionar Grupo de Materiais','Adicionar Material')
OR [Description] like 'Alterar Caracter_sticas do Material'

INSERT INTO #ChildActions
SELECT 'DeleteMaterialType', 0, 'Apagar Tipo de Materiais', NULL

INSERT INTO #ChildActions
SELECT DISTINCT 'ExportMaterialType', 0, 'Exportar Configuração do Material ', evolGUIActionID
FROM #ChildActions
WHERE LocalizationKey = 'AddMaterialType'

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionMaterialManagement'

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions

INSERT INTO #ChildActions
SELECT 'AddEnvelopeRange', 0, 'Adicionar/Alterar Gama de envelopes', ActionID
FROM ACTIONS
WHERE [Description] in ('Alterar envelope em Gama de envelopes','Adicionar Gamas de Envelopes',
'Alterar Grupo de Gamas de Envelopes','Alterar ou Apagar Grupo de Gama de Envelopes','Alterar Grupo de Gama de Envelopes','Adicionar Grupo de Gama de Envelopes')
OR [Description] like 'Alterar Gama de Envelopes para Companhia de Expedi__o'
OR [Description] like 'Alterar Contratos de Expedi__o para Gama de Envelopes'
OR [Description] like 'Alterar Contrato de Expedi__o para Gama de Envelopes'
OR [Description] like 'Adicionar Excep__o de Gama de Envelopes para Companhia de Expedi__o'


INSERT INTO #ChildActions
SELECT 'DeleteEnvelopeRange', 0, 'Apagar Gama de Envelopes', ActionID
FROM ACTIONS
WHERE [Description] in ('Apagar Gama de Envelopes','Apagar Grupo de Gama de Envelopes')
OR [Description] like 'Apagar Excep__o de Gama de Envelopes para Companhia de Expedi__o'

INSERT INTO #ChildActions
SELECT DISTINCT 'ExportEnvelopeRange', 0, 'Exportar Configuração da Gama de Envelopes', evolGUIActionID
FROM #ChildActions
WHERE LocalizationKey = 'AddEnvelopeRange'


DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionEnvelopeRange'

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions
DROP TABLE #ChildActions
GO
----------------------------------------------------------------
-- EXPEDITION CONFIG
----------------------------------------------------------------
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

SELECT @ParentLocalizationKey = 'ActionExpeditionConfig'

	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @ParentLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, ActionTypeID, @ParentLocalizationKey, 'Configurações de Expedição', ParentActionID, DefaultOrder, 0, NULL, 0
		FROM [evolUX_ACTIONS]
		WHERE LocalizationKey = 'ActionExpeditionTypes'
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 1, [Description] = 'Configurações de Expedição', ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
FROM [evolUX_PERMISSIONS] p
INNER JOIN
	[evolUX_ACTIONS] u
ON u.ActionID = p.ActionID
WHERE u.LocalizationKey = 'ActionExpeditionTypes'
AND NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

SELECT @ParentActionID = @ActionID

UPDATE evolUX_ACTIONS
SET ParentActionID = @ParentActionID, ActionTypeID = 1, DefaultOrder = 10 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionTypes'

UPDATE evolUX_ACTIONS
SET ParentActionID = @ParentActionID, ActionTypeID = 1, DefaultOrder = 20 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionZones'

UPDATE evolUX_ACTIONS
SET ParentActionID = @ParentActionID, ActionTypeID = 1, DefaultOrder = 30 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionCompanies'

SELECT @ParentActionID = ActionID 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionCompanies'


CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int, [Description] varchar(255), evolGUIActionID int)

INSERT INTO #ChildActions
SELECT 'ActionExpCodes', 10, [Description], evolGUI_ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpCodes'

INSERT INTO #ChildActions
SELECT 'ActionExpeditionContracts', 20, 'Configurar Contratos de Expedição', ActionID
FROM ACTIONS
WHERE  [Description] like 'Contratos de Expedi__o'

INSERT INTO #ChildActions
SELECT 'ActionExpRegistRange', 30, 'Configurar Faixa de Registos para Expedição', ActionID
FROM ACTIONS
WHERE  [Description] like 'Alterar Faixa de Registos para Expedic__'

INSERT INTO #ChildActions
SELECT 'AddExpeditionType', 0, 'Alterar Tipo de Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Alter Configura__es para Tipo de Expedi__o'
	OR [Description] like 'Alteração de Configura__es para Tipo de Expedi__o'

INSERT INTO #ChildActions
SELECT 'AddExpCompany', 0, 'Alterar Companhia de Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar Características da Companhia de Expedição'

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = @ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionCompanies'

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions

INSERT INTO #ChildActions
SELECT 'AddExpeditionContract', 0, 'Adicionar/Alterar Contrato de Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar Contrato de Expedi__o'
	OR [Description] like 'Adicionar Contrato de Expedi__o'

INSERT INTO #ChildActions
SELECT 'DeleteExpeditionContract', 0, 'Apagar Contrato de Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Apagar Contrato de Expedi__o'

INSERT INTO #ChildActions
SELECT DISTINCT 'ExportExpeditionContract', 0, 'Exportar Contrato de Expedição', evolGUIActionID
FROM #ChildActions
WHERE LocalizationKey = 'AddExpeditionContract'

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionContracts'

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions

INSERT INTO #ChildActions
SELECT 'AddExpRegistRange', 0, 'Adicionar/Alterar Faixa de Registos', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar Faixa de Registos para Expedi__o'
	OR [Description] like 'Altera__o da Faixa de Registos para Expedi__o'
	OR [Description] like 'Adicionar Faixa de Registos para Expedi__o'
	OR [Description] like 'Adi__o de Faixa de Registos para Expedi__o'

INSERT INTO #ChildActions
SELECT DISTINCT 'ExportExpRegistRange', 0, 'Exportar Configuração da Faixa de Registos ', evolGUIActionID
FROM #ChildActions
WHERE LocalizationKey = 'AddExpRegistRange'


DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = @ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionEnvelopeRange'

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions
DROP TABLE #ChildActions
GO
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

SELECT @ParentLocalizationKey = 'ActionExpeditionConfig', @DefaultOrder = 40, @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionMenuEvolDPConfig'

	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @ParentLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, ActionTypeID, @ParentLocalizationKey, 'Configurações de Expedição', ParentActionID, DefaultOrder, 0, NULL, 0
		FROM [evolUX_ACTIONS]
		WHERE LocalizationKey = 'ActionExpeditionTypes'
	END
	ELSE

	UPDATE [evolUX_ACTIONS]
	SET ActionTypeID = 1, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
	WHERE LocalizationKey = @ParentLocalizationKey

INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
FROM [evolUX_PERMISSIONS] p
INNER JOIN
	[evolUX_ACTIONS] u
ON u.ActionID = p.ActionID
WHERE u.LocalizationKey = 'ActionExpeditionTypes'
AND NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

SELECT @ParentActionID = @ActionID

UPDATE evolUX_ACTIONS
SET ParentActionID = @ParentActionID, ActionTypeID = 1, DefaultOrder = 10 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionTypes'

UPDATE evolUX_ACTIONS
SET ParentActionID = @ParentActionID, ActionTypeID = 1, DefaultOrder = 20 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionZones'

UPDATE evolUX_ACTIONS
SET ParentActionID = @ParentActionID, ActionTypeID = 1, DefaultOrder = 30 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionCompanies'

SELECT @ParentActionID = ActionID 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpeditionCompanies'


CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int, [Description] varchar(255), evolGUIActionID int)

INSERT INTO #ChildActions
SELECT 'ActionExpeditionContracts', 20, 'Configurar Contratos de Expedição', ActionID
FROM ACTIONS
WHERE  [Description] like 'Contratos de Expedi__o'

INSERT INTO #ChildActions
SELECT 'ActionExpRegistRange', 20, 'Configurar Faixa de Registos para Expedição', ActionID
FROM ACTIONS
WHERE  [Description] like 'Alterar Faixa de Registos para Expedic__'

INSERT INTO #ChildActions
SELECT 'AddExpeditionType', 0, 'Alterar Companhia de Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Alter Configura__es para Tipo de Expedi__o'
	OR [Description] like 'Alteração de Configura__es para Tipo de Expedi__o'

INSERT INTO #ChildActions
SELECT 'AddExpCompany', 0, 'Alterar Companhia de Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar Características da Companhia de Expedição'

INSERT INTO #ChildActions
SELECT 'AddExpeditionContract', 0, 'Adicionar/Alterar Contrato de Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar Contrato de Expedi__o'
	OR [Description] like 'Adicionar Contrato de Expedi__o'

INSERT INTO #ChildActions
SELECT 'DeleteExpeditionContract', 0, 'Apagar Contrato de Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Apagar Contrato de Expedi__o'

INSERT INTO #ChildActions
SELECT DISTINCT 'ExportExpeditionContract', 0, 'Exportar Contrato de Expedição', evolGUIActionID
FROM #ChildActions
WHERE LocalizationKey = 'AddExpeditionContract'

INSERT INTO #ChildActions
SELECT 'AddExpRegistRange', 0, 'Adicionar/Alterar Faixa de Registos', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar Faixa de Registos para Expedi__o'
	OR [Description] like 'Altera__o da Faixa de Registos para Expedi__o'
	OR [Description] like 'Adicionar Faixa de Registos para Expedi__o'
	OR [Description] like 'Adi__o de Faixa de Registos para Expedi__o'

INSERT INTO #ChildActions
SELECT DISTINCT 'ExportExpRegistRange', 0, 'Exportar Configuração da Faixa de Registos ', evolGUIActionID
FROM #ChildActions
WHERE LocalizationKey = 'AddExpRegistRange'


DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, NULL, @DefaultOrder, 0, NULL, 0
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions
DROP TABLE #ChildActions
GO
--------------------------------------
-- SERVICES
--------------------------------------
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

SELECT @ParentLocalizationKey = 'ActionMenuEvolDPConfig', @NewLocalizationKey = 'ActionServiceProvision', 
	@NewDescription = 'Configuração de Serviços', @DefaultOrder = 30

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 1, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 1, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
FROM [evolUX_PERMISSIONS] p
INNER JOIN
	[evolUX_ACTIONS] u
ON u.ActionID = p.ActionID
WHERE u.LocalizationKey = 'ActionServicesProvided'
AND NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

SELECT @ParentActionID = @ActionID

UPDATE evolUX_ACTIONS
SET ParentActionID = @ParentActionID, ActionTypeID = 1, DefaultOrder = 10 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionServicesProvided'

UPDATE evolUX_ACTIONS
SET ParentActionID = @ParentActionID, ActionTypeID = 1, DefaultOrder = 30 
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionServiceCompanies'

SELECT @NewLocalizationKey = 'ActionServiceWorkFlow', @NewDescription = 'Configuração de Tipos de Tratamento', @DefaultOrder = 20

SET @ActionID = NULL
SELECT @ActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 100
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 100
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 1, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 1, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
FROM [PERMISSIONS] p
INNER JOIN
	ACTIONS c
ON p.ActionID = c.ActionID
WHERE c.[Description] like 'C_digos Tratamento%Expedi__o'
	AND NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)


CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int, [Description] varchar(255), evolGUIActionID int)

INSERT INTO #ChildActions
SELECT 'ActionExpCodes', 10, [Description], evolGUI_ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpCodes'

INSERT INTO #ChildActions
SELECT 'AddServiceTask', 20, 'Configurar Tipos Tratamento', ActionID
FROM ACTIONS
WHERE  [Description] like 'Associar%Tipo de Tratamento'
	OR
	[Description] like 'Adicionar%Tipo de Tratamento'
	OR
	[Description] like 'Alterar%Tipo de Tratamento'

INSERT INTO #ChildActions
SELECT 'DeleteServiceTask', 30, 'Apagar Tipo de Tratamento', ActionID
FROM ACTIONS
WHERE  [Description] like 'Apagar%Tipo de Tratamento%'

INSERT INTO #ChildActions
SELECT 'AddServiceCompany', 0, 'Adicionar/Alterar Companhia de Prestação de Serviços', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar Limites da Companhia de Servi_os'

INSERT INTO #ChildActions
SELECT 'AddServiceCompanyExpCodes', 0, 'Adicionar/Alterar escalões de envelopagem da Companhia de Prestação de Serviços', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar custos dos Servi_os para Companhia de Servi_os'

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionServiceCompanies'

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions

INSERT INTO #ChildActions
SELECT 'AddExpCode', 0, 'Adicionar/Alterar Código Tratamento/Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Alterar Companhia de Servi_os para C_digo Tratamento_Expedi__o'

INSERT INTO #ChildActions
SELECT 'DeleteExpCode', 0, 'Apagar Código Tratamento/Expedição', ActionID
FROM ACTIONS
WHERE [Description] like 'Apagar C_digo de Tratamento%Expedi__o'

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionExpCodes'

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DELETE #ChildActions

DROP TABLE #ChildActions
GO
--------------------------------------
-- FINISHING
--------------------------------------
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int)

INSERT INTO #ChildActions
SELECT 'ActionPartialRecover', 10
INSERT INTO #ChildActions
SELECT 'ActionTotalRecover', 20
INSERT INTO #ChildActions
SELECT 'ActionPendingRecover', 30
INSERT INTO #ChildActions
SELECT 'ActionRegistDetailRecover', 40
--------
SELECT @NewLocalizationKey = 'ActionRecovers', @NewDescription = 'Recuperações', @ParentLocalizationKey = 'ActionMenuFinishing'

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

SELECT @DefaultOrder = MIN(u.DefaultOrder)
FROM evolUX_ACTIONS u
INNER JOIN
	#ChildActions c
ON u.LocalizationKey = c.LocalizationKey

SET @ActionID = NULL
SELECT @ActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @NewLocalizationKey

IF (@ActionID is NULL)
BEGIN
	SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
	FROM ACTIONS
	WHERE ActionID < 10000

	WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
		OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
	BEGIN
		SET @ActionID = @ActionID + 10
	END

	INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
	SELECT @ActionID, 1, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
END
ELSE
BEGIN
	UPDATE [evolUX_ACTIONS]
	SET ActionTypeID = 1, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
	WHERE ActionID = @ActionID
END

INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
FROM [PERMISSIONS] p
INNER JOIN
	[evolUX_ACTIONS] u
ON u.ActionID = p.ActionID
INNER JOIN
	#ChildActions c
ON u.LocalizationKey = c.LocalizationKey
WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)
---------
UPDATE evolUX_ACTIONS
SET ParentActionID = @ActionID, DefaultOrder = c.DefaultOrder
FROM evolUX_ACTIONS u
INNER JOIN
	#ChildActions c
ON u.LocalizationKey = c.LocalizationKey
DROP TABLE #ChildActions

GO
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int)

INSERT INTO #ChildActions
SELECT 'ActionConcludedFulfilment', 30
INSERT INTO #ChildActions
SELECT 'ActionConcludedPrint', 20
INSERT INTO #ChildActions
SELECT 'ActionPendingRegist', 10

SELECT @NewLocalizationKey = 'ActionConcludedRegist', @NewDescription = 'Impressão/Envelopagem', @ParentLocalizationKey = 'ActionMenuFinishing'

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

SELECT @DefaultOrder = MIN(u.DefaultOrder)
FROM evolUX_ACTIONS u
INNER JOIN
	#ChildActions c
ON u.LocalizationKey = c.LocalizationKey

SET @ActionID = NULL
SELECT @ActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 1, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 1, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END
INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
FROM [PERMISSIONS] p
INNER JOIN
	[evolUX_ACTIONS] u
ON u.ActionID = p.ActionID
INNER JOIN
	#ChildActions c
ON u.LocalizationKey = c.LocalizationKey
WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

UPDATE evolUX_ACTIONS
SET ParentActionID = @ActionID, DefaultOrder = c.DefaultOrder
FROM evolUX_ACTIONS u
INNER JOIN
	#ChildActions c
ON u.LocalizationKey = c.LocalizationKey

DROP TABLE #ChildActions
GO
DECLARE @DefaultOrder int

SET @DefaultOrder = 10

UPDATE evolUX_ACTIONS
SET DefaultOrder = @DefaultOrder
WHERE ParentActionID = (SELECT ActionID FROM evolUX_ACTIONS WHERE LocalizationKey = 'ActionMenuFinishing')
	AND LocalizationKey = 'ActionProductionStatus'

SET @DefaultOrder = @DefaultOrder + 10
UPDATE evolUX_ACTIONS
SET DefaultOrder = @DefaultOrder
WHERE ParentActionID = (SELECT ActionID FROM evolUX_ACTIONS WHERE LocalizationKey = 'ActionMenuFinishing')
	AND LocalizationKey = 'ActionConcludedRegist'

SET @DefaultOrder = @DefaultOrder + 10
UPDATE evolUX_ACTIONS
SET DefaultOrder = @DefaultOrder
WHERE ParentActionID = (SELECT ActionID FROM evolUX_ACTIONS WHERE LocalizationKey = 'ActionMenuFinishing')
	AND LocalizationKey = 'ActionRecovers'

SET @DefaultOrder = @DefaultOrder + 10
UPDATE evolUX_ACTIONS
SET DefaultOrder = @DefaultOrder
WHERE ParentActionID = (SELECT ActionID FROM evolUX_ACTIONS WHERE LocalizationKey = 'ActionMenuFinishing')
	AND LocalizationKey = 'ActionPostalObject'

SET @DefaultOrder = @DefaultOrder + 30
UPDATE evolUX_ACTIONS
SET DefaultOrder = @DefaultOrder
WHERE ParentActionID = (SELECT ActionID FROM evolUX_ACTIONS WHERE LocalizationKey = 'ActionMenuFinishing')
	AND LocalizationKey = 'ActionExpedition'
GO
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int, [Description] varchar(255), evolGUIActionID int)

INSERT INTO #ChildActions
SELECT DISTINCT 'ActionPendingExpeditionFiles', 10, 'Expedição Pendente', ActionID
FROM ACTIONS
WHERE [Description] like 'Gerar Guias de Expedi__o'

INSERT INTO #ChildActions
SELECT DISTINCT 'ActionExpeditionReportList', 20, 'Listagem de Guias', ActionID
FROM ACTIONS
WHERE [Description] like 'Guias de Expedi__o' AND [URL] like 'ViewExpReport000.asp%'


SELECT @ParentLocalizationKey = 'ActionExpedition'

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = @ParentLocalizationKey

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 100
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 100
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 1, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 1, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END

	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DROP TABLE #ChildActions
GO
DECLARE @ActionID int,
	@NewLocalizationKey varchar(50),
	@ParentLocalizationKey varchar(50),
	@NewDescription varchar(255),
	@ParentActionID int,
	@DefaultOrder int

CREATE TABLE #ChildActions(LocalizationKey varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS, DefaultOrder int, [Description] varchar(255), evolGUIActionID int)

INSERT INTO #ChildActions
SELECT DISTINCT 'IgnoreFilePrinterSpecs', 0, 'Ignorar Restrições de impressão para ficheiros', ActionID
FROM ACTIONS
WHERE [Description] like 'Bot_o para mostrar todas as impressoras'

INSERT INTO #ChildActions
SELECT DISTINCT 'PrintFile', 0, 'Imprimir Ficheiro', ActionID
FROM ACTIONS
WHERE [Description] like 'Imprimir Ficheiro' 

DECLARE tCursor CURSOR LOCAL FOR
SELECT LocalizationKey, DefaultOrder, [Description]
FROM #ChildActions
ORDER BY DefaultOrder ASC

SELECT @ParentActionID = ActionID
FROM evolUX_ACTIONS
WHERE LocalizationKey = 'ActionProductionStatus'

OPEN tCursor
FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @ActionID = NULL
	SELECT @ActionID = ActionID
	FROM evolUX_ACTIONS
	WHERE LocalizationKey = @NewLocalizationKey

	IF (@ActionID is NULL)
	BEGIN
		SELECT @ActionID = (MAX(ActionID) / 100)*100 + 10
		FROM ACTIONS
		WHERE ActionID < 10000

		WHILE (EXISTS(SELECT TOP 1 1 FROM ACTIONS WHERE ActionID = @ActionID)
			OR EXISTS(SELECT TOP 1 1 FROM evolUX_ACTIONS WHERE ActionID = @ActionID))
		BEGIN
			SET @ActionID = @ActionID + 10
		END

		INSERT INTO [evolUX_ACTIONS](ActionID, ActionTypeID, LocalizationKey, [Description], ParentActionID, DefaultOrder, HistoryFlag, evolGUI_ActionID, evolGUI_TypeID)
		SELECT @ActionID, 3, @NewLocalizationKey, @NewDescription, @ParentActionID, @DefaultOrder, 0, NULL, 0
	END
	ELSE
	BEGIN
		UPDATE [evolUX_ACTIONS]
		SET ActionTypeID = 3, [Description] = @NewDescription, ParentActionID = @ParentActionID, DefaultOrder = @DefaultOrder
		WHERE ActionID = @ActionID
	END
	INSERT INTO [dbo].[evolUX_PERMISSIONS](ActionID, ProfileID, PermissionID, FlowID, TaskID, ActionOrder, FlowType)
	SELECT DISTINCT @ActionID, p.ProfileID, 1, NULL, NULL, NULL, 0
	FROM #ChildActions c
	INNER JOIN
		[PERMISSIONS] p
	ON	c.evolGUIActionID = p.ActionID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.LocalizationKey = c.LocalizationKey
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolUX_PERMISSIONS] WHERE ActionID = @ActionID AND ProfileID = p.ProfileID)

	FETCH NEXT FROM tCursor INTO @NewLocalizationKey, @DefaultOrder, @NewDescription
END
CLOSE tCursor
DEALLOCATE tCursor

DROP TABLE #ChildActions
GO
