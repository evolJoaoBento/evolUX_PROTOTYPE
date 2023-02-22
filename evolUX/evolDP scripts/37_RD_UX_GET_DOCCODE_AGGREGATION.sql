IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_DOCCODE_AGGREGATION]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_DOCCODE_AGGREGATION] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_DOCCODE_AGGREGATION]
	@DocCodeID int
AS
BEGIN
	SELECT	d.DocCodeID,
			d.DocLayout,
			d.DocType,
			d.[Description] as DocDescription,
			CAST(CASE WHEN MAX(ISNULL(a.RefDocCodeID,0)) = 0 THEN 0 ELSE 1 END as bit) Compatible,
			e1.ExceptionLevelID,
			e1.ExceptionCode,
			e1.ExceptionDescription,
			e2.ExceptionLevelID,
			e2.ExceptionCode,
			e2.ExceptionDescription,
			e3.ExceptionLevelID,
			e3.ExceptionCode,
			e3.ExceptionDescription
	FROM
		RD_DOCCODE d WITH(NOLOCK)
	INNER JOIN
		RD_DOCCODE_CONFIG dc WITH(NOLOCK)
	ON	dc.DocCodeID = d.DocCodeID
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
	LEFT OUTER JOIN
		RD_DOCCODE x WITH(NOLOCK)
	ON x.DocCodeID = @DocCodeID
	LEFT OUTER JOIN
		RD_DOCCODE_AGGREGATION_COMPATIBILITY a WITH(NOLOCK)
	ON	a.AggDocCodeID = d.DocCodeID
		AND a.RefDocCodeID = x.DocCodeID
	WHERE (dc.StartDate > CAST(CONVERT(varchar(8),CURRENT_TIMESTAMP,112) as int)
		OR dc.StartDate = (SELECT MAX(StartDate) FROM RD_DOCCODE_CONFIG WITH(NOLOCK) WHERE DocCodeID = dc.DocCodeID))
		AND (dc.AggrCompatibility in (1,2)
			OR
			(dc.AggrCompatibility = 3 AND d.DocLayout = x.DocLayout AND d.DocType = x.DocType))
	GROUP BY d.DocCodeID,
			d.DocLayout,
			d.DocType,
			d.[Description],
			e1.ExceptionLevelID,
			e1.ExceptionCode,
			e1.ExceptionDescription,
			e2.ExceptionLevelID,
			e2.ExceptionCode,
			e2.ExceptionDescription,
			e3.ExceptionLevelID,
			e3.ExceptionCode,
			e3.ExceptionDescription
	ORDER BY d.DocLayout, d.DocType, e1.ExceptionLevelID, e2.ExceptionLevelID, e3.ExceptionLevelID
END
GO
