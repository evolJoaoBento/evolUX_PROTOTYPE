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