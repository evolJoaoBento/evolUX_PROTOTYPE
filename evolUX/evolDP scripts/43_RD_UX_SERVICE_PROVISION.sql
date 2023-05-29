IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_SERVICE_TASK]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASK] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_SERVICE_TASK]
	@ServiceTaskID int = NULL
AS
BEGIN
	SELECT ServiceTaskID, ServiceTaskCode, [Description] ServiceTaskDesc, StationExceededDesc, ComplementServiceTaskID, ExternalExpeditionMode
	FROM RD_SERVICE_TASK
	WHERE @ServiceTaskID is NULL OR ServiceTaskID = @ServiceTaskID
	ORDER BY ServiceTaskID
END
GO