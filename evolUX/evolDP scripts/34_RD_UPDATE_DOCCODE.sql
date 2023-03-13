IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UPDATE_DOCCODE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UPDATE_DOCCODE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UPDATE_DOCCODE]
	@DocCodeID int,
	@Description varchar(256),
	@PrintMatchCode varchar(10) = ''
--WITH ENCRYPTION
AS
	SET NOCOUNT ON
	BEGIN TRANSACTION
	DECLARE
		@ErrorNumber int,
		@ErrorMsg varchar(255)


	UPDATE RD_DOCCODE
	SET [Description] = @Description, PrintMatchCode = @PrintMatchCode
	WHERE DocCodeID = @DocCodeID

	COMMIT TRANSACTION
	SET NOCOUNT OFF
RETURN
ErrorHandler:
	RAISERROR (@ErrorMsg, 16, 1)
	ROLLBACK TRANSACTION
GO
