IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_COMPANIES_INFO]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_COMPANIES_INFO] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_COMPANIES_INFO]
	@CompanyID int = NULL,
	@CompanyList IDList READONLY
AS
BEGIN
	IF (@CompanyID is NOT NULL)
	BEGIN
		SELECT c.CompanyID,
			c.CompanyCode,
			c.CompanyName,
			c.CompanyAddress,
			c.CompanyPostalCode,
			c.CompanyPostalCodeDescription,
			c.CompanyCountry
		FROM
			RD_COMPANY c WITH(NOLOCK) 
		WHERE c.CompanyID = @CompanyID
		ORDER BY c.CompanyID
	END
	ELSE
	BEGIN
		SELECT c.CompanyID,
			c.CompanyCode,
			c.CompanyName,
			c.CompanyAddress,
			c.CompanyPostalCode,
			c.CompanyPostalCodeDescription,
			c.CompanyCountry
		FROM
			@CompanyList cl
		INNER JOIN
			RD_COMPANY c WITH(NOLOCK) 
		ON	cl.ID = c.CompanyID
		ORDER BY c.CompanyID
	END
END
GO