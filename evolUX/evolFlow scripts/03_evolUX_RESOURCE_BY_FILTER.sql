IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_RESOURCE_BY_FILTER]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[evolUX_RESOURCE_BY_FILTER] AS'
END
GO
ALTER PROCEDURE [dbo].[evolUX_RESOURCE_BY_FILTER]
	@ProfileList IDList READONLY,
	@ResName varchar(50),
	@ResValueFilter varchar(5000) = NULL,
	@IgnoreProfiles bit = 0
AS
BEGIN
	SET NOCOUNT ON

	IF (ISNULL(@IgnoreProfiles,0) = 0)
	BEGIN
		SELECT ResValue, [Description], 
			CASE WHEN @ResValueFilter is NULL THEN 1 
			ELSE
				CASE WHEN ResValue like ('%' + @ResValueFilter + '%') THEN 1 ELSE 0 END 
			END MatchFilter
		FROM RESOURCES 
		WHERE MachineName in (SELECT DISTINCT p.CompanyServer 
					FROM PROFILES p,
						@ProfileList pp
					WHERE p.ProfileID = pp.ID) 
			AND ResName = @ResName and FreeAmount > 0 and IsResource = 1
		ORDER BY CASE WHEN @ResValueFilter is NULL THEN 1 
			ELSE
				CASE WHEN ResValue like ('%' + @ResValueFilter + '%') THEN 1 ELSE 0 END 
			END DESC


	END
	ELSE
	BEGIN
		SELECT ResValue, [Description], 
			CASE WHEN @ResValueFilter is NULL THEN 1 
			ELSE
				CASE WHEN ResValue like ('%' + @ResValueFilter + '%') THEN 1 ELSE 0 END 
			END MatchFilter
		FROM RESOURCES 
		WHERE ResName = @ResName and FreeAmount > 0 and IsResource = 1
		ORDER BY CASE WHEN @ResValueFilter is NULL THEN 1 
			ELSE
				CASE WHEN ResValue like ('%' + @ResValueFilter + '%') THEN 1 ELSE 0 END 
			END DESC
	END

	SET NOCOUNT OFF
END


