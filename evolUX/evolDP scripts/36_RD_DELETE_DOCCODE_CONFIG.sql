IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_DELETE_DOCCODE_CONFIG]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_DELETE_DOCCODE_CONFIG] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_DELETE_DOCCODE_CONFIG]
	@DocCodeID int,
	@StartDate int
--WITH ENCRYPTION
AS
	SET NOCOUNT ON

	DELETE RD_DOCCODE_CONFIG    
	WHERE DocCodeID = @DocCodeID AND StartDate = @StartDate 

	SELECT 0 ErrorID, 'Success' Error
					
RETURN
GO
