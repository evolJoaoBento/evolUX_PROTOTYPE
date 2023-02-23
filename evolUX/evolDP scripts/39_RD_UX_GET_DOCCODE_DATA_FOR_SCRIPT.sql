IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_DOCCODE_DATA_FOR_SCRIPT]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_DOCCODE_DATA_FOR_SCRIPT] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_DOCCODE_DATA_FOR_SCRIPT]
	@DocCodeID int,
	@StartDate int
AS
	SET NOCOUNT ON

	--DOCCODE
	SELECT d.DocLayout, d.DocType, e1.ExceptionCode ExceptionLevel1Code, 
		e2.ExceptionCode ExceptionLevel2Code, e3.ExceptionCode ExceptionLevel3Code, d.[Description] DocDescription, d.PrintMatchCode,
		dc.AggrCompatibility, emg.[Description]  EnvMediaDesc, dc.ExpeditionType, dc.ExpCode, dc.SuportType, [Priority], 
		dc.CaducityDate, dc.MaxProdDate, dc.ProdMaxSheets, dc.ArchCaducityDate
	INTO #DOCCODE_CONFIG
	FROM RD_DOCCODE d WITH(NOLOCK)
	INNER JOIN
		RD_DOCCODE_CONFIG dc WITH(NOLOCK)
	ON	dc.DocCodeID = d.DocCodeID
	INNER JOIN
		RD_ENVELOPE_MEDIA_GROUP emg WITH(NOLOCK)
	ON emg.[EnvMediaGroupID] = dc.EnvMediaID
	LEFT OUTER JOIN
		RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
	ON e1.[ExceptionLevelID] = d.[ExceptionLevel1ID]
	LEFT OUTER JOIN
		RDC_EXCEPTION_LEVEL1 e2 WITH(NOLOCK)
	ON e2.[ExceptionLevelID] = d.[ExceptionLevel2ID]
	LEFT OUTER JOIN
		RDC_EXCEPTION_LEVEL1 e3 WITH(NOLOCK)
	ON e3.[ExceptionLevelID] = d.[ExceptionLevel3ID]
	WHERE d.DocCodeID = @DocCodeID AND dc.StartDate = @StartDate
	
	--EXCEPTION_LEVEL1
	SELECT CAST(1 as int) [Level], e.ExceptionCode, e.ExceptionDescription
	INTO #EXCEPTION_LEVEL
	FROM RDC_EXCEPTION_LEVEL1 e WITH(NOLOCK)
	INNER JOIN
		RD_DOCCODE d WITH(NOLOCK)
	ON e.[ExceptionLevelID] = d.[ExceptionLevel1ID]
	WHERE DocCodeID = @DocCodeID

	--EXCEPTION_LEVEL2
	INSERT INTO #EXCEPTION_LEVEL([Level], ExceptionCode, ExceptionDescription)
	SELECT CAST(2 as int) [Level], e.ExceptionCode, e.ExceptionDescription
	FROM RDC_EXCEPTION_LEVEL2 e WITH(NOLOCK)
	INNER JOIN
		RD_DOCCODE d WITH(NOLOCK)
	ON e.[ExceptionLevelID] = d.[ExceptionLevel2ID]
	WHERE DocCodeID = @DocCodeID

	--EXCEPTION_LEVEL3
	INSERT INTO #EXCEPTION_LEVEL([Level], ExceptionCode, ExceptionDescription)
	SELECT CAST(3 as int) [Level], e.ExceptionCode, e.ExceptionDescription
	FROM RDC_EXCEPTION_LEVEL3 e WITH(NOLOCK)
	INNER JOIN
		RD_DOCCODE d WITH(NOLOCK)
	ON e.[ExceptionLevelID] = d.[ExceptionLevel3ID]
	WHERE DocCodeID = @DocCodeID


	--DOCCODE_AGGREGATION_COMPATIBILITY
	SELECT DocLayout, DocType, ExceptionLevel1Code, ExceptionLevel2Code, ExceptionLevel3Code
	INTO #DOCCODE_AGGREGATION_COMPATIBILITY
	FROM #DOCCODE_CONFIG

	DELETE #DOCCODE_AGGREGATION_COMPATIBILITY

	IF ((SELECT AggrCompatibility FROM #DOCCODE_CONFIG) = 2)
	BEGIN
		--EXCEPTION_LEVEL1
		INSERT INTO #EXCEPTION_LEVEL([Level], ExceptionCode, ExceptionDescription)
		SELECT DISTINCT 1, e.ExceptionCode, e.ExceptionDescription
		FROM 
			RDC_EXCEPTION_LEVEL1 e WITH(NOLOCK)
		INNER JOIN
			RD_DOCCODE d WITH(NOLOCK)
		ON d.[ExceptionLevel1ID] = e.[ExceptionLevelID]
		INNER JOIN
			RD_DOCCODE_AGGREGATION_COMPATIBILITY da
		ON da.[RefDocCodeID] = @DocCodeID
			AND da.[AggDocCodeID] = d.DocCodeID
		WHERE NOT EXISTS (SELECT TOP 1 1 FROM #EXCEPTION_LEVEL WHERE [Level] = 1 AND ExceptionCode = e.ExceptionCode AND ExceptionDescription = e.ExceptionDescription)

		INSERT INTO #EXCEPTION_LEVEL([Level], ExceptionCode, ExceptionDescription)
		SELECT DISTINCT 1, e.ExceptionCode, e.ExceptionDescription
		FROM 
			RDC_EXCEPTION_LEVEL1 e WITH(NOLOCK)
		INNER JOIN
			RD_DOCCODE d WITH(NOLOCK)
		ON d.[ExceptionLevel1ID] = e.[ExceptionLevelID]
		INNER JOIN
			RD_DOCCODE_AGGREGATION_COMPATIBILITY da
		ON da.[AggDocCodeID] = @DocCodeID
			AND da.[RefDocCodeID] = d.DocCodeID
		WHERE NOT EXISTS (SELECT TOP 1 1 FROM #EXCEPTION_LEVEL WHERE [Level] = 1 AND ExceptionCode = e.ExceptionCode AND ExceptionDescription = e.ExceptionDescription)


		--EXCEPTION_LEVEL2
		INSERT INTO #EXCEPTION_LEVEL([Level], ExceptionCode, ExceptionDescription)
		SELECT DISTINCT 2, e.ExceptionCode, e.ExceptionDescription
		FROM 
			RDC_EXCEPTION_LEVEL2 e WITH(NOLOCK)
		INNER JOIN
			RD_DOCCODE d WITH(NOLOCK)
		ON d.[ExceptionLevel2ID] = e.[ExceptionLevelID]
		INNER JOIN
			RD_DOCCODE_AGGREGATION_COMPATIBILITY da
		ON da.[RefDocCodeID] = @DocCodeID
			AND da.[AggDocCodeID] = d.DocCodeID
		WHERE NOT EXISTS (SELECT TOP 1 1 FROM #EXCEPTION_LEVEL WHERE [Level] = 2 AND ExceptionCode = e.ExceptionCode AND ExceptionDescription = e.ExceptionDescription)

		INSERT INTO #EXCEPTION_LEVEL([Level], ExceptionCode, ExceptionDescription)
		SELECT DISTINCT 2, e.ExceptionCode, e.ExceptionDescription
		FROM 
			RDC_EXCEPTION_LEVEL2 e WITH(NOLOCK)
		INNER JOIN
			RD_DOCCODE d WITH(NOLOCK)
		ON d.[ExceptionLevel2ID] = e.[ExceptionLevelID]
		INNER JOIN
			RD_DOCCODE_AGGREGATION_COMPATIBILITY da
		ON da.[AggDocCodeID] = @DocCodeID
			AND da.[RefDocCodeID] = d.DocCodeID
		WHERE NOT EXISTS (SELECT TOP 1 1 FROM #EXCEPTION_LEVEL WHERE [Level] = 2 AND ExceptionCode = e.ExceptionCode AND ExceptionDescription = e.ExceptionDescription)

		--EXCEPTION_LEVEL3
		INSERT INTO #EXCEPTION_LEVEL([Level], ExceptionCode, ExceptionDescription)
		SELECT DISTINCT 3, e.ExceptionCode, e.ExceptionDescription
		FROM 
			RDC_EXCEPTION_LEVEL3 e WITH(NOLOCK)
		INNER JOIN
			RD_DOCCODE d WITH(NOLOCK)
		ON d.[ExceptionLevel3ID] = e.[ExceptionLevelID]
		INNER JOIN
			RD_DOCCODE_AGGREGATION_COMPATIBILITY da
		ON da.[RefDocCodeID] = @DocCodeID
			AND da.[AggDocCodeID] = d.DocCodeID
		WHERE NOT EXISTS (SELECT TOP 1 1 FROM #EXCEPTION_LEVEL WHERE [Level] = 3 AND ExceptionCode = e.ExceptionCode AND ExceptionDescription = e.ExceptionDescription)

		INSERT INTO #EXCEPTION_LEVEL([Level], ExceptionCode, ExceptionDescription)
		SELECT DISTINCT 3, e.ExceptionCode, e.ExceptionDescription
		FROM 
			RDC_EXCEPTION_LEVEL3 e WITH(NOLOCK)
		INNER JOIN
			RD_DOCCODE d WITH(NOLOCK)
		ON d.[ExceptionLevel3ID] = e.[ExceptionLevelID]
		INNER JOIN
			RD_DOCCODE_AGGREGATION_COMPATIBILITY da
		ON da.[AggDocCodeID] = @DocCodeID
			AND da.[RefDocCodeID] = d.DocCodeID
		WHERE NOT EXISTS (SELECT TOP 1 1 FROM #EXCEPTION_LEVEL WHERE [Level] = 3 AND ExceptionCode = e.ExceptionCode AND ExceptionDescription = e.ExceptionDescription)

		--DOCCODE_AGGREGATION_COMPATIBILITY
		INSERT INTO #DOCCODE_AGGREGATION_COMPATIBILITY
		SELECT DISTINCT d.DocLayout, d.DocType, e1.ExceptionCode ExceptionLevel1Code, e2.ExceptionCode ExceptionLevel2Code, e3.ExceptionCode ExceptionLevel3Code
		FROM RD_DOCCODE d WITH(NOLOCK)
		INNER JOIN
			RD_DOCCODE_AGGREGATION_COMPATIBILITY da
		ON da.[RefDocCodeID] = @DocCodeID
			AND da.[AggDocCodeID] = d.DocCodeID
		LEFT OUTER JOIN
			RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
		ON e1.[ExceptionLevelID] = d.[ExceptionLevel1ID]
		LEFT OUTER JOIN
			RDC_EXCEPTION_LEVEL1 e2 WITH(NOLOCK)
		ON e2.[ExceptionLevelID] = d.[ExceptionLevel2ID]
		LEFT OUTER JOIN
			RDC_EXCEPTION_LEVEL1 e3 WITH(NOLOCK)
		ON e3.[ExceptionLevelID] = d.[ExceptionLevel3ID]
	
		INSERT INTO #DOCCODE_AGGREGATION_COMPATIBILITY
		SELECT DISTINCT d.DocLayout, d.DocType, e1.ExceptionCode ExceptionLevel1Code, e2.ExceptionCode ExceptionLevel2Code, e3.ExceptionCode ExceptionLevel3Code
		FROM RD_DOCCODE d WITH(NOLOCK)
		INNER JOIN
			RD_DOCCODE_AGGREGATION_COMPATIBILITY da
		ON da.[AggDocCodeID] = @DocCodeID
			AND da.[RefDocCodeID] = d.DocCodeID
		LEFT OUTER JOIN
			RDC_EXCEPTION_LEVEL1 e1 WITH(NOLOCK)
		ON e1.[ExceptionLevelID] = d.[ExceptionLevel1ID]
		LEFT OUTER JOIN
			RDC_EXCEPTION_LEVEL1 e2 WITH(NOLOCK)
		ON e2.[ExceptionLevelID] = d.[ExceptionLevel2ID]
		LEFT OUTER JOIN
			RDC_EXCEPTION_LEVEL1 e3 WITH(NOLOCK)
		ON e3.[ExceptionLevelID] = d.[ExceptionLevel3ID]
		WHERE NOT EXISTS (SELECT TOP 1 1 
							FROM #DOCCODE_AGGREGATION_COMPATIBILITY 
							WHERE DocLayout = d.DocLayout AND DocType = d.DocType
								AND ISNULL(ExceptionLevel1Code,0) = ISNULL(e1.ExceptionCode,0)
								AND ISNULL(ExceptionLevel2Code,0) = ISNULL(e2.ExceptionCode,0)
								AND ISNULL(ExceptionLevel3Code,0) = ISNULL(e3.ExceptionCode,0))
	END

	SELECT *
	FROM #EXCEPTION_LEVEL

	SELECT *
	FROM #DOCCODE_CONFIG

	SELECT *
	FROM #DOCCODE_AGGREGATION_COMPATIBILITY

	DROP TABLE #EXCEPTION_LEVEL
	DROP TABLE #DOCCODE_CONFIG
	DROP TABLE #DOCCODE_AGGREGATION_COMPATIBILITY

RETURN
GO
