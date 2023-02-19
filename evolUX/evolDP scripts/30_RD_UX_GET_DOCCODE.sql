IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_DOCCODE_GROUP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_DOCCODE_GROUP] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_DOCCODE_GROUP]
AS
BEGIN
	SET NOCOUNT ON
	SELECT	DISTINCT d.DocLayout,
			d.DocType,
			d.PrintMatchCode,
			(SELECT TOP 1 dc.[Description]
					FROM RD_DOCCODE dc WITH(NOLOCK)
					WHERE dc.DocLayout = d.DocLayout AND dc.DocType = d.DocType AND dc.PrintMatchCode = d.PrintMatchCode
					ORDER BY dc.DocCodeID ASC
						--ISNULL(dc.ExceptionLevel1ID,0) ASC, 
						--ISNULL(dc.ExceptionLevel2ID,0) ASC,
						--ISNULL(dc.ExceptionLevel3ID,0) ASC
								) 
			as DocDescription
	FROM RD_DOCCODE d WITH(NOLOCK)
	ORDER BY d.DocLayout, d.DocType
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_DOCCODE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_DOCCODE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_DOCCODE]
	@DocLayout varchar(20),
	@DocType varchar(8),
	@NumRows int =  2147483647
AS
BEGIN
	SELECT	TOP(@NumRows) d.DocCodeID,
			d.DocLayout,
			d.DocType,
			[Description] as DocDescription,
			e1.ExceptionLevelID,
			e1.ExceptionCode,
			e1.ExceptionDescription,
			e2.ExceptionLevelID,
			e2.ExceptionCode,
			e2.ExceptionDescription,
			e3.ExceptionLevelID,
			e3.ExceptionCode,
			e3.ExceptionDescription
	FROM 	RD_DOCCODE d WITH(NOLOCK)
	LEFT OUTER JOIN
			RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
			ON	
			e1.ExceptionLevelID = d.ExceptionLevel1ID
	LEFT OUTER JOIN
			RDC_EXCEPTION_LEVEL2 e2 WITH(NOLOCK)
			ON	
			e2.ExceptionLevelID = d.ExceptionLevel2ID
	LEFT OUTER JOIN
			RDC_EXCEPTION_LEVEL3 e3 WITH(NOLOCK)
			ON	
			e3.ExceptionLevelID = d.ExceptionLevel3ID
	WHERE	d.DocLayout = @DocLayout
			AND		
			d.DocType = @DocType
	ORDER BY d.ExceptionLevel1ID, d.ExceptionLevel2ID, d.ExceptionLevel3ID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_ENVELOPE_MEDIA_GROUP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_ENVELOPE_MEDIA_GROUP] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_ENVELOPE_MEDIA_GROUP]
	@EnvMediaGroupID int = NULL
AS
BEGIN
	SET NOCOUNT ON
	SELECT  EnvMediaGroupID, [Description], [DefaultEnvMediaID]
	FROM RD_ENVELOPE_MEDIA_GROUP WITH(NOLOCK)
	WHERE @EnvMediaGroupID is NULL OR EnvMediaGroupID = @EnvMediaGroupID
	ORDER BY [Description]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPEDITION_TYPE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_TYPE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_TYPE]
	@ExpeditionType int = NULL
AS
BEGIN
	SET NOCOUNT ON
	SELECT ExpeditionType, [Priority], [Description]
	FROM RD_EXPEDITION_TYPE WITH(NOLOCK)
	WHERE @ExpeditionType is NULL OR ExpeditionType = @ExpeditionType
	ORDER BY [Priority]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_TASK]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASK] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASK]
	@ServiceTaskID int = NULL
AS
BEGIN
	SELECT ServiceTaskID, ServiceTaskCode, [Description] ServiceTaskDesc, StationExceededDesc, ComplementServiceTaskID, ExternalExpeditionMode
	FROM RD_SERVICE_TASK
	WHERE @ServiceTaskID is NULL OR ServiceTaskID = @ServiceTaskID
	ORDER BY ServiceTaskID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RDC_UX_GET_SUPORT_TYPE_CONFIG]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RDC_UX_GET_SUPORT_TYPE_CONFIG] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RDC_UX_GET_SUPORT_TYPE_CONFIG]
	@Option varchar(15)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @OptionJoin bit,
		@OptionStart varchar(10)

	IF (CHARINDEX('Hide',@Option,1) > 0)
	BEGIN
		SELECT @OptionStart = SUBSTRING(@Option,1,CHARINDEX('Hide',@Option,1)-1)

		SELECT TOP 1 @OptionJoin = CASE WHEN [FieldDescription] = 'true' THEN 1 ELSE 0 END
		FROM [evolDP_DESCRIPTION]
		WHERE FieldName = (@OptionStart + 'Join')

		IF (@OptionJoin = 1)
		BEGIN
			SET @Option = NULL
		END
	END

	CREATE TABLE #OPTION_LIST
	([ID] int NOT NULL,
	 [Code] varchar(20) NOT NULL,
	 [Description] varchar(256))


	IF (@Option = 'Finishing' OR @Option = 'Archive' OR @Option = 'ElectronicHide' OR @Option = 'EmailHide')
	BEGIN
		INSERT INTO #OPTION_LIST([ID], [Code], [Description])
		SELECT DISTINCT 
			CASE @Option 
				WHEN 'Finishing' THEN Finishing*1 
				WHEN 'Archive' THEN Archive*2 
				WHEN 'ElectronicHide' THEN ElectronicHide*16 
				WHEN 'EmailHide' THEN EmailHide*128 
			END,
			@Option + CASE @Option 
				WHEN 'Finishing' THEN CAST(Finishing as varchar) 
				WHEN 'Archive' THEN CAST(Archive as varchar) 
				WHEN 'ElectronicHide' THEN CAST(ElectronicHide as varchar) 
				WHEN 'EmailHide' THEN CAST(EmailHide as varchar) 
			END,
			CASE @Option 
				WHEN 'Finishing' THEN CAST(Finishing as varchar) 
				WHEN 'Archive' THEN CAST(Archive as varchar) 
				WHEN 'ElectronicHide' THEN CAST(ElectronicHide as varchar) 
				WHEN 'EmailHide' THEN CAST(EmailHide as varchar) 
			END
		FROM RDC_SUPORT_TYPE WITH(NOLOCK)
	END
	ELSE
	BEGIN
		IF (@Option = 'Email')
		BEGIN
			INSERT INTO #OPTION_LIST([ID], [Code], [Description])
			SELECT SuportTypeValue [ID], SuportStream [Code], SuportTypeDescription [Description]
			FROM RDC_SUPORT_TYPE_REFERENCE
			WHERE SuportTypeValue & 224 > 0
		END
		ELSE
		BEGIN
			IF (@Option = 'Electronic')
			BEGIN
				INSERT INTO #OPTION_LIST([ID], [Code], [Description])
				SELECT SuportTypeValue [ID], SuportStream [Code], SuportTypeDescription [Description]
				FROM RDC_SUPORT_TYPE_REFERENCE
				WHERE SuportTypeValue & 28 > 0
			END
			ELSE
			BEGIN
				INSERT INTO #OPTION_LIST([ID], [Code], [Description])
				SELECT SuportTypeValue [ID], SuportStream [Code], SuportTypeDescription [Description]
				FROM RDC_SUPORT_TYPE_REFERENCE
				WHERE SuportTypeValue is NULL
			END
		END
		IF ((SELECT COUNT(1) FROM #OPTION_LIST) > 0)
		BEGIN
			INSERT INTO #OPTION_LIST([ID], [Code], [Description])
			SELECT 0 [ID], @Option + '0' [Code], @Option [Description]
		END
	END

	SELECT [ID], [Code], [Description]
	FROM #OPTION_LIST

	DROP TABLE #OPTION_LIST
END
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_DOCCODE_CONFIG]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_DOCCODE_CONFIG] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_DOCCODE_CONFIG]
	@DocCodeID int,
	@StartDate int = NULL,
	@MaxDateFlag bit = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SuportTable IDDescList

	INSERT INTO @SuportTable ([ID], [Desc])
	SELECT SuportTypeValue [ID], SuportStream [Desc]
	FROM RDC_SUPORT_TYPE_REFERENCE

	DECLARE @ElectronicBits int,
			@EmailBits int
	SELECT @ElectronicBits = 4+8,
			@EmailBits = 32+64

	SELECT @ElectronicBits = @ElectronicBits + CASE WHEN UPPER(FieldDescription) = 'TRUE' THEN 16 ELSE 0 END
	FROM evolDP_DESCRIPTION WITH(NOLOCK)
	WHERE FieldName = 'ElectronicJoin'

	SELECT @EmailBits = @EmailBits + CASE WHEN UPPER(FieldDescription) = 'TRUE' THEN 128 ELSE 0 END
	FROM evolDP_DESCRIPTION WITH(NOLOCK)
	WHERE FieldName = 'EmailJoin'

	SELECT CAST((CASE WHEN CAST(CONVERT(varchar,CURRENT_TIMESTAMP,112) as int) > dc.StartDate THEN 0 ELSE 1 END) as bit) IsEditable,
	d.DocCodeID,
	d.[Description] DocDescription,
	d.PrintMatchCode,
	dc.StartDate,
	dc.AggrCompatibility,
	dc.EnvMediaID,
	evg.[Description] EnvMediaDesc,
	dc.ExpeditionType,
	dc.ExpCode,
	ce.CompanyID ExpCompanyID,
	ce.CompanyName ExpCompanyName,
	st.ServiceTaskID,
	st.ServiceTaskCode,
	st.[Description] ServiceTaskDesc,
	dc.SuportType,
	(dc.SuportType & 1) Finishing,
	(dc.SuportType & 2) Archive,
	(dc.SuportType & @ElectronicBits) Electronic,
	dbo.[GetFormat4SuportType](@ElectronicBits, dc.SuportType, ' /', @SuportTable) ElectronicDesc,
	(dc.SuportType & 16) ElectronicHide,
	(dc.SuportType & @EmailBits) EMail,
	dbo.[GetFormat4SuportType](@EmailBits, dc.SuportType, ' /', @SuportTable) EMailDesc,
	(dc.SuportType & 128) EMailHide,
	dc.[Priority], 
	--dc.AgingDays, --Descontinuado
	dc.CaducityDate,
	dc.MaxProdDate,
	dc.ProdMaxSheets,
	dc.ArchCaducityDate
	FROM
		RD_DOCCODE_CONFIG dc WITH(NOLOCK)
	INNER JOIN
		RD_DOCCODE d WITH(NOLOCK)
	ON d.DocCodeID = dc.DocCodeID
	INNER JOIN
		RD_ENVELOPE_MEDIA_GROUP evg WITH(NOLOCK)
	ON evg.[EnvMediaGroupID] = dc.EnvMediaID
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	ON est.ExpCode = dc.ExpCode
	INNER JOIN
		RD_COMPANY ce WITH(NOLOCK)
	ON ce.CompanyID = est.ExpCompanyID
	INNER JOIN
		RD_SERVICE_TASK st WITH(NOLOCK)
	ON st.ServiceTaskID = est.ServiceTaskID
	WHERE dc.DocCodeID = @DocCodeID
		AND 
		(
			dc.StartDate = @StartDate
		OR
			(@MaxDateFlag = 0 AND @StartDate is NULL)
		OR
			(@MaxDateFlag = 1 AND dc.StartDate = (SELECT MAX(StartDate) FROM RD_DOCCODE_CONFIG WHIT(NOLOCK) WHERE DocCodeID = dc.DocCodeID))
		)
	ORDER BY dc.StartDate DESC
END
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