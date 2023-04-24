IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_FULLFILL_MATERIALCODE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_FULLFILL_MATERIALCODE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_FULLFILL_MATERIALCODE]
	@FullFillMaterialCode varchar(10) = NULL
AS
	SET NOCOUNT ON

	SELECT FullFillMaterialCode, FullFillCapacity, [Format], [Description]
	FROM RD_FULLFILL_MATERIALCODE WITH(NOLOCK)
	WHERE @FullFillMaterialCode is NULL OR FullFillMaterialCode = @FullFillMaterialCode
	ORDER BY FullFillCapacity ASC
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_MATERIAL_TYPE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_MATERIAL_TYPE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_MATERIAL_TYPE]
	@GroupCodes bit = 0,
	@MaterialTypeCode varchar(10) = NULL
AS
	SET NOCOUNT ON
	IF (@GroupCodes = 1)
	BEGIN
		SELECT mt.MaterialTypeID, x.MaterialTypeCode, mt.MaterialTypeDescription
		FROM
			(SELECT MIN(MaterialTypeID) MaterialTypeID, 
				CASE WHEN MaterialTypeCode like '%Paper' THEN 'Paper'
				ELSE
					CASE WHEN MaterialTypeCode like '%Station' THEN 'Station'
					ELSE
						CASE WHEN MaterialTypeCode like '%Envelope' THEN 'Envelope'
						ELSE
							MaterialTypeCode
						END
					END
				END MaterialTypeCode
			FROM RD_MATERIAL_TYPE WITH(NOLOCK)
			GROUP BY (CASE WHEN MaterialTypeCode like '%Paper' THEN 'Paper'
				ELSE
					CASE WHEN MaterialTypeCode like '%Station' THEN 'Station'
					ELSE
						CASE WHEN MaterialTypeCode like '%Envelope' THEN 'Envelope'
						ELSE
							MaterialTypeCode
						END
					END
				END)) x
		INNER JOIN
			RD_MATERIAL_TYPE mt
		ON mt.MaterialTypeID = x.MaterialTypeID
		ORDER BY mt.MaterialTypeID ASC
	END
	ELSE
	BEGIN
		SELECT MaterialTypeID, MaterialTypeCode, MaterialTypeDescription
		FROM RD_MATERIAL_TYPE WITH(NOLOCK)
		WHERE @MaterialTypeCode is NULL OR MaterialTypeCode like ('%' + @MaterialTypeCode)
		ORDER BY MaterialTypeID ASC
	END
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_MATERIAL]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_MATERIAL] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_MATERIAL]
	@MaterialID int = NULL,
	@MaterialRef varchar(20) = NULL,
	@MaterialTypeID int = NULL,
	@MaterialTypeCode varchar(10) = NULL,
	@MaterialCode varchar(20) = NULL,
	@GroupID int = NULL
AS
	SET NOCOUNT ON
	IF (@MaterialID is NOT NULL OR @MaterialRef is NOT NULL)
	BEGIN
		SELECT m.MaterialID, m.MaterialTypeID, m.MaterialCode, m.MaterialDescription, m.MaterialWeight, m.MaterialRef, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight, m.GroupID
		FROM RD_MATERIAL m WITH(NOLOCK)
		WHERE m.MaterialID = ISNULL(@MaterialID,m.MaterialID)
			OR
			m.MaterialRef = ISNULL(@MaterialRef,m.MaterialRef)
	END
	ELSE
	BEGIN
		IF (@MaterialTypeCode is NOT NULL)
		BEGIN
			SELECT m.MaterialID, m.MaterialTypeID, m.MaterialCode, m.MaterialDescription, m.MaterialWeight, m.MaterialRef, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight, m.GroupID
			FROM RD_MATERIAL m WITH(NOLOCK)
			INNER JOIN
				RD_MATERIAL_TYPE mt WITH(NOLOCK)
			ON mt.MaterialTypeID = m.MaterialTypeID
			WHERE (@MaterialCode is NULL OR m.MaterialCode = @MaterialCode)
				AND mt.MaterialTypeCode like ('%' + @MaterialTypeCode)
				AND (@GroupID is NULL OR GroupID = @GroupID)
		END
		ELSE
		BEGIN
			SELECT m.MaterialID, m.MaterialTypeID, m.MaterialCode, m.MaterialDescription, m.MaterialWeight, m.MaterialRef, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight, m.GroupID
			FROM RD_MATERIAL m WITH(NOLOCK)
			INNER JOIN
				RD_MATERIAL_TYPE mt WITH(NOLOCK)
			ON mt.MaterialTypeID = m.MaterialTypeID
			WHERE (@MaterialCode is NULL OR m.MaterialCode = @MaterialCode)
				AND (@MaterialTypeID is NULL OR mt.MaterialTypeID = @MaterialTypeID)
				AND (@GroupID is NULL OR GroupID = @GroupID)
		END
	END
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_MATERIAL_GROUP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_MATERIAL_GROUP] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_MATERIAL_GROUP]
	@GroupID int = NULL,
	@MaterialTypeID int = NULL,
	@MaterialTypeCode varchar(10) = NULL,
	@GroupCode varchar(20) = NULL
AS
	SET NOCOUNT ON
	IF (@MaterialTypeCode is NOT NULL)
	BEGIN
		SELECT m.GroupID, m.MaterialTypeID, m.GroupCode, m.GroupDescription, m.MaterialWeight, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight
		FROM RD_MATERIAL_GROUP m WITH(NOLOCK)
		INNER JOIN
			RD_MATERIAL_TYPE mt WITH(NOLOCK)
		ON mt.MaterialTypeID = m.MaterialTypeID
		WHERE (@GroupCode is NULL OR m.GroupCode = @GroupCode)
			AND mt.MaterialTypeCode like ('%' + @MaterialTypeCode)
			AND m.GroupID = ISNULL(@GroupID,m.GroupID)
	END
	ELSE
	BEGIN
		SELECT m.GroupID, m.MaterialTypeID, m.GroupCode, m.GroupDescription, m.MaterialWeight, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight
		FROM RD_MATERIAL_GROUP m WITH(NOLOCK)
		INNER JOIN
			RD_MATERIAL_TYPE mt WITH(NOLOCK)
		ON mt.MaterialTypeID = m.MaterialTypeID
		WHERE (@GroupCode is NULL OR m.GroupCode = @GroupCode)
			AND (@MaterialTypeID is NULL OR mt.MaterialTypeID = @MaterialTypeID)
			AND m.GroupID = ISNULL(@GroupID,m.GroupID)
	END
RETURN
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_ENVELOPE_MEDIA]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_ENVELOPE_MEDIA] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_ENVELOPE_MEDIA]
	@EnvMediaID int = NULL
AS
BEGIN
	SET NOCOUNT ON
	SELECT  EnvMediaID, EnvMediaName, [Description]
	FROM RD_ENVELOPE_MEDIA WITH(NOLOCK)
	WHERE @EnvMediaID is NULL OR EnvMediaID = @EnvMediaID
	ORDER BY [Description]
END
GO