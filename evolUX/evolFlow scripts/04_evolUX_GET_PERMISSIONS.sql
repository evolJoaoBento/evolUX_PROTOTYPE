IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_GET_PERMISSIONS]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[evolUX_GET_PERMISSIONS] AS'
END
GO
ALTER PROCEDURE [dbo].[evolUX_GET_PERMISSIONS]
	@ProfileList IDList READONLY,
	@LocalizationKey varchar(50) = NULL
AS
BEGIN
	SELECT DISTINCT u.LocalizationKey
	FROM @ProfileList p
	INNER JOIN
		[evolUX_PERMISSIONS] pu
	ON	p.ID = pu.ProfileID
	INNER JOIN
		[evolUX_ACTIONS] u
	ON u.ActionID = pu.ActionID
	WHERE ActionTypeID = 3 AND ISNULL(@LocalizationKey,LocalizationKey) = LocalizationKey
END


