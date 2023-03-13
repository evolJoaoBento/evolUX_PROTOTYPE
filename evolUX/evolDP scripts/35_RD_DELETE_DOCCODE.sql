IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_DELETE_DOCCODE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_DELETE_DOCCODE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_DELETE_DOCCODE]
	@DocCodeID int,
	@DeleteAll bit = 0
--WITH ENCRYPTION
AS
	SET NOCOUNT ON
	DECLARE
		@ErrorNumber int,
		@ErrorMsg varchar(255)


	IF (EXISTS(SELECT TOP 1 1 FROM RT_DOCUMENT WITH(NOLOCK) WHERE DocCodeID = @DocCodeID))   
	BEGIN
		SELECT -1 ErrorID, 'ExistsDocs4DocCode' Error
		RETURN  
	END   
	ELSE	
	BEGIN    
		IF (EXISTS(SELECT TOP 1 1 FROM RT_DOCUMENT_SET WITH(NOLOCK) WHERE DocCodeID = @DocCodeID))   
		BEGIN     
			SELECT -2 ErrorID, 'ExistsDocSets4DocCode' Error
			RETURN  
		END    
		ELSE    
		BEGIN
			BEGIN TRANSACTION

			IF (@DeleteAll = 1)
			BEGIN
				DELETE RD_DOCCODE_CONFIG    
				WHERE DocCodeID = @DocCodeID  
					
				DELETE RD_DOCCODE_AGGREGATION_COMPATIBILITY     
				WHERE RefDocCodeID = @DocCodeID       
						OR AggDocCodeID = @DocCodeID      
			END
			ELSE
			BEGIN
				IF (EXISTS (SELECT TOP 1 1 FROM RD_DOCCODE_CONFIG WITH(NOLOCK) WHERE DocCodeID = @DocCodeID)
					OR
					EXISTS (SELECT TOP 1 1 FROM RD_DOCCODE_AGGREGATION_COMPATIBILITY WITH(NOLOCK) WHERE RefDocCodeID = @DocCodeID OR AggDocCodeID = @DocCodeID))
				BEGIN
					SELECT -3 ErrorID, 'ExistsDocConfigs4DocCode' Error
				END
			END
  
			DELETE RD_DOCCODE     
			WHERE DocCodeID = @DocCodeID
			
			SELECT 0 ErrorID, 'Success' Error
			COMMIT TRANSACTION
		END   
	END   

	SET NOCOUNT OFF
RETURN
ErrorHandler:
	RAISERROR (@ErrorMsg, 16, 1)
	ROLLBACK TRANSACTION
GO
