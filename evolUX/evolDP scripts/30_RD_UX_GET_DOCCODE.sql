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
	SELECT ServiceTaskID, ServiceTaskCode, [Description], StationExceededDesc, ComplementServiceTaskID, ExternalExpeditionMode
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

	IF (@Option = 'Finishing' OR @Option = 'Archive' OR @Option = 'ElectronicHide' OR @Option = 'EmailHide')
	BEGIN
		SELECT DISTINCT 
			CASE @Option 
				WHEN 'Finishing' THEN Finishing*1 
				WHEN 'Archive' THEN Archive*2 
				WHEN 'ElectronicHide' THEN ElectronicHide*16 
				WHEN 'EmailHide' THEN EmailHide*128 
			END [ID],
			CASE @Option 
				WHEN 'Finishing' THEN CAST(Finishing as varchar) 
				WHEN 'Archive' THEN CAST(Archive as varchar) 
				WHEN 'ElectronicHide' THEN CAST(ElectronicHide as varchar) 
				WHEN 'EmailHide' THEN CAST(EmailHide as varchar) 
			END [Description]
		FROM RDC_SUPORT_TYPE WITH(NOLOCK)
	END
	ELSE
	BEGIN
		IF (@Option = 'Email')
		BEGIN
			SELECT SuportTypeValue [ID], SuportStream [Description]
			FROM RDC_SUPORT_TYPE_REFERENCE
			WHERE SuportTypeValue & 224 > 0
		END
		ELSE
		BEGIN
			IF (@Option = 'Electronic')
			BEGIN
				SELECT SuportTypeValue [ID], SuportStream [Description]
				FROM RDC_SUPORT_TYPE_REFERENCE
				WHERE SuportTypeValue & 28 > 0
			END
			ELSE
			BEGIN
				SELECT SuportTypeValue [ID], SuportStream [Description]
				FROM RDC_SUPORT_TYPE_REFERENCE
				WHERE SuportTypeValue is NULL
			END
		END
	END
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
	s.Finishing * 1,
	s.Archive * 2,
	s.Electronic ElectronicDesc,
	s.ElectronicHide,
	s.EMail EMailDesc,
	s.EMailHide,
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
	LEFT OUTER JOIN
		RDC_SUPORT_TYPE s WITH(NOLOCK)
	ON s.SuportType = dc.SuportType		
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