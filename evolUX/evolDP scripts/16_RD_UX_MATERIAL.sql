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