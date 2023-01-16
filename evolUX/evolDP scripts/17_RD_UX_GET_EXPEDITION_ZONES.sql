IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_EXPEDITION_ZONES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_ZONES] AS' 
END
GO

ALTER  PROCEDURE [dbo].[RD_UX_GET_EXPEDITION_ZONES]
AS
BEGIN
	SELECT ExpeditionZone ExpeditionZoneID,
	[Description] ExpeditionZoneDesc
	FROM RD_EXPEDITION_ZONE
END
GO
