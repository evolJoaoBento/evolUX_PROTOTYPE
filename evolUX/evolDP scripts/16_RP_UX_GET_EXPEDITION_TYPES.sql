IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_GET_EXPEDITION_TYPES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_GET_EXPEDITION_TYPES] AS' 
END
GO

ALTER  PROCEDURE [dbo].[RP_UX_GET_EXPEDITION_TYPES]
AS
BEGIN
	SELECT ExpeditionType ExpeditionTypeID,
		[Priority],
		[Description] ExpeditionTypeDesc
	FROM RD_EXPEDITION_TYPE
END
GO
