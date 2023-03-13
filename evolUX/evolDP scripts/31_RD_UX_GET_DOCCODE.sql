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
	@DocCodeID int = NULL,
	@DocLayout varchar(20) = NULL,
	@DocType varchar(8) = NULL,
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
	WHERE	(@DocCodeID is NULL AND d.DocLayout = @DocLayout AND d.DocType = @DocType)
			OR d.DocCodeID = @DocCodeID
	ORDER BY d.ExceptionLevel1ID, d.ExceptionLevel2ID, d.ExceptionLevel3ID
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RDC_UX_GET_SUPORT_TYPE_CONFIG]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RDC_UX_GET_SUPORT_TYPE_CONFIG] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RDC_UX_GET_SUPORT_TYPE_CONFIG]
	@Option varchar(15) = NULL
AS
BEGIN
	SET NOCOUNT ON

	CREATE TABLE #OPTION_LIST
	([ID] int NOT NULL,
	 [Code] varchar(20) NOT NULL,
	 [GroupCode] varchar(20) NOT NULL)

	INSERT INTO #OPTION_LIST([ID], [Code], [GroupCode])
	SELECT [ID], [Code], [GroupCode]
	FROM RDC_UX_SUPORT_TYPE_OPTIONS
	WHERE (@Option is NULL OR [GroupCode] = @Option)

	INSERT INTO #OPTION_LIST([ID], [Code], [GroupCode])
	SELECT 0 [ID], o.[GroupCode] + '0', o.[GroupCode]
	FROM #OPTION_LIST o
	WHERE NOT EXISTS (SELECT TOP 1 1 FROM #OPTION_LIST WHERE [GroupCode] = o.[GroupCode] AND ID = 0)
	GROUP BY [GroupCode]

	SELECT [ID], [Code], [GroupCode]
	FROM #OPTION_LIST

	DROP TABLE #OPTION_LIST
END
GO