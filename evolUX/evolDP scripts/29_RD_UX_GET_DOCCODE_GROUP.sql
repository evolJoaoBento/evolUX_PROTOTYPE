IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_DOCCODE_GROUP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_DOCCODE_GROUP] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_DOCCODE_GROUP]
AS
BEGIN
	SET NOCOUNT ON
	SELECT	d.DocLayout,
			d.DocType,
			(SELECT TOP 1 dc.[Description]
					FROM RD_DOCCODE dc WITH(NOLOCK)
					WHERE dc.DocLayout = d.DocLayout AND dc.DocType = d.DocType
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
	@DocType varchar(8)
AS
BEGIN
	SELECT	d.DocCodeID,
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
