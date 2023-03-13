IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_PURGE_PARAMETERS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_PURGE_PARAMETERS] AS' 
END
GO

ALTER  PROCEDURE [dbo].[RD_UX_GET_PURGE_PARAMETERS]
AS
BEGIN
	SELECT ParameterID,	ParameterRef, ParameterValue, ParameterDescription
	FROM RD_CONSTANT_PARAMETERS
END
GO
