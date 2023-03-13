IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPC_UX_GET_PROJECTS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RPC_UX_GET_PROJECTS] AS' 
END
GO

ALTER  PROCEDURE [dbo].[RPC_UX_GET_PROJECTS]
	@Type varchar(50) = 'DOC1'
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @CURRENT_TIMESTAMP datetime
	SELECT @CURRENT_TIMESTAMP = CURRENT_TIMESTAMP

	CREATE TABLE #VERSION(
		[BusinessID] int NOT NULL,
		[ProjectCode] varchar(14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[VersionID] int NULL)
	
	DECLARE @BusinessID int,
		@ProjectCode varchar(14)
	
	DECLARE tCursor CURSOR LOCAL
	FOR SELECT DISTINCT BusinessID, ProjectCode
	FROM RDC_VERSION

	OPEN tCursor
	FETCH NEXT FROM tCursor INTO @BusinessID, @ProjectCode

	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO #VERSION
		SELECT TOP 1 @BusinessID, @ProjectCode, VersionID
		FROM RDC_VERSION v WITH(NOLOCK)
		WHERE v.BusinessID = @BusinessID
			AND v.ProjectCode = @ProjectCode
			AND v.[Type] = @Type
			AND (v.StartDate < CAST(CONVERT(varchar, @CURRENT_TIMESTAMP, 112) as int)
				OR (v.StartDate = CAST(CONVERT(varchar, @CURRENT_TIMESTAMP, 112) as int) 
					AND v.StartTime < CAST(REPLACE(CONVERT(varchar,@CURRENT_TIMESTAMP,8),':','') as int)))
		ORDER BY StartDate DESC, StartTime DESC
		FETCH NEXT FROM tCursor INTO @BusinessID, @ProjectCode
	END
	CLOSE tCursor
	DEALLOCATE tCursor

	SELECT b.[Description] BusinessDesc,
		b.BusinessCode,
		b.BusinessID,
		v2.[Type],
		v1.ProjectCode,
		CASE WHEN LEN(CAST(v2.Major as varchar)) < 2 THEN REPLICATE('0', 2 - LEN(CAST(v2.Major as varchar))) ELSE '' END + CAST(v2.Major as varchar)
		+ '_' +
		CASE WHEN LEN(CAST(v2.Minor as varchar)) < 2 THEN REPLICATE('0', 2 - LEN(CAST(v2.Minor as varchar))) ELSE '' END + CAST(v2.Minor as varchar)
		+ '_' +
		CASE WHEN LEN(CAST(v2.Revision as varchar)) < 2 THEN REPLICATE('0', 2 - LEN(CAST(v2.Revision as varchar))) ELSE '' END + CAST(v2.Revision as varchar)
		+ '_' +
		CASE WHEN LEN(CAST(v2.Patch as varchar)) < 2 THEN REPLICATE('0', 2 - LEN(CAST(v2.Patch as varchar))) ELSE '' END + CAST(v2.Patch as varchar) [Version],
		v2.[Description] [VersionDesc],
		v2.StartDate,
		v2.StartTime
		FROM (SELECT DISTINCT BusinessID, ProjectCode
				FROM  RDC_VERSION WITH(NOLOCK)) v1
		INNER JOIN
			RD_BUSINESS b
		ON v1.BusinessID = b.BusinessID
		LEFT OUTER JOIN
			#VERSION v
		ON v.BusinessID = v1.BusinessID	AND v.ProjectCode = v1.ProjectCode
		LEFT OUTER JOIN
			RDC_VERSION v2 WITH(NOLOCK)
		ON v2.VersionID = v.VersionID
		ORDER BY v1.BusinessID, v1.ProjectCode    

	DROP TABLE #VERSION
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPC_UX_GET_PROJECT_VERSIONS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RPC_UX_GET_PROJECT_VERSIONS] AS' 
END
GO

ALTER  PROCEDURE [dbo].[RPC_UX_GET_PROJECT_VERSIONS]
	@BusinessID int,
	@ProjectCode varchar(14),
	@Type varchar(50) = 'DOC1'
AS
BEGIN
	SET NOCOUNT ON

	SELECT 	VersionID,
		ProjectCode,
		StartDate,
		StartTime,
		CASE WHEN LEN(CAST(Major as varchar)) < 2 THEN REPLICATE('0', 2 - LEN(CAST(Major as varchar))) ELSE '' END + CAST(Major as varchar)
		+ '_' +
		CASE WHEN LEN(CAST(Minor as varchar)) < 2 THEN REPLICATE('0', 2 - LEN(CAST(Minor as varchar))) ELSE '' END + CAST(Minor as varchar)
		+ '_' +
		CASE WHEN LEN(CAST(Revision as varchar)) < 2 THEN REPLICATE('0', 2 - LEN(CAST(Revision as varchar))) ELSE '' END + CAST(Revision as varchar)
		+ '_' +
		CASE WHEN LEN(CAST(Patch as varchar)) < 2 THEN REPLICATE('0', 2 - LEN(CAST(Patch as varchar))) ELSE '' END + CAST(Patch as varchar) [Version],
		[Description] [VersionDesc]
	FROM RDC_VERSION v2 WITH(NOLOCK)
	WHERE BusinessID = @BusinessID
		AND ProjectCode = @ProjectCode
		AND [Type] = @Type
	ORDER BY StartDate DESC, StartTime Desc
END
GO

