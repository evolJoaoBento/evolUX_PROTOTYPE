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
		SELECT c.CompanyID [ID],
			c.CompanyCode,
			c.CompanyName,
			c.CompanyAddress,
			c.CompanyPostalCode,
			c.CompanyPostalCodeDescription,
			c.CompanyCountry,
			c.CompanyServer
		FROM
			RD_COMPANY c WITH(NOLOCK) 
		WHERE c.CompanyID = @CompanyID
		ORDER BY c.CompanyID
	END
	ELSE
	BEGIN
		SELECT c.CompanyID [ID],
			c.CompanyCode,
			c.CompanyName,
			c.CompanyAddress,
			c.CompanyPostalCode,
			c.CompanyPostalCodeDescription,
			c.CompanyCountry,
			c.CompanyServer
		FROM
			@CompanyList cl
		INNER JOIN
			RD_COMPANY c WITH(NOLOCK) 
		ON	cl.ID = c.CompanyID
		ORDER BY c.CompanyID
	END
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_COMPANY_CONFIG]') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].[RD_COMPANY_CONFIG]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_COMPANY_CONFIG]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RD_COMPANY_CONFIG](
	[CompanyID] [int] NOT NULL,
	[RelationCompanyID] [int] NOT NULL,
	[RelationType] [varchar](10) NOT NULL,
 CONSTRAINT [PK_RD_COMPANY_CONFIG] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[RelationCompanyID] ASC,
	[RelationType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET NOCOUNT ON
INSERT INTO [dbo].[RD_COMPANY_CONFIG]([CompanyID], [RelationCompanyID], [RelationType])
SELECT DISTINCT 1, c.[ServiceCompanyID], 'SERVICE'
FROM [dbo].[RD_SERVICE_COMPANY_RESTRICTION] c
WHERE NOT EXISTS (SELECT TOP 1 1
				FROM [dbo].[RD_COMPANY_CONFIG]
				WHERE [CompanyID] = 1
					AND [RelationCompanyID] = c.[ServiceCompanyID]
					AND [RelationType] = 'SERVICE')

INSERT INTO [dbo].[RD_COMPANY_CONFIG]([CompanyID], [RelationCompanyID], [RelationType])
SELECT DISTINCT 1, c.[ExpCompanyID], 'EXPEDITION'
FROM [dbo].[RD_EXPCOMPANY_TYPE] c
WHERE NOT EXISTS (SELECT TOP 1 1
				FROM [dbo].[RD_COMPANY_CONFIG]
				WHERE [CompanyID] = 1
					AND [RelationCompanyID] = c.[ExpCompanyID]
					AND [RelationType] = 'EXPEDITION')

INSERT INTO [dbo].[RD_COMPANY_CONFIG]([CompanyID], [RelationCompanyID], [RelationType])
SELECT DISTINCT c1.[RelationCompanyID], c2.[RelationCompanyID], c2.[RelationType]
FROM [dbo].[RD_COMPANY_CONFIG] c1
INNER JOIN
	[dbo].[RD_COMPANY_CONFIG] c2
ON c1.[CompanyID] = c2.[CompanyID]
	AND c1.[RelationType] = 'SERVICE'
	AND c2.[RelationType] = 'EXPEDITION'
WHERE c1.[CompanyID] = 1
	AND NOT EXISTS (SELECT TOP 1 1
				FROM [dbo].[RD_COMPANY_CONFIG]
				WHERE [CompanyID] = c1.[RelationCompanyID]
					AND [RelationCompanyID] = c2.[RelationCompanyID]
					AND [RelationType] = c2.[RelationType])
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_COMPANY_INFO]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_COMPANY_INFO] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_SET_COMPANY_INFO]
	@CompanyID int = NULL,
	@CompanyCode varchar(6),
	@CompanyName varchar(256) = NULL,
	@CompanyAddress varchar(256) = NULL,
	@CompanyPostalCode varchar(256) = NULL,
	@CompanyPostalCodeDescription varchar(256) = NULL,
	@CompanyCountry varchar(256) = NULL,
	@CompanyServer varchar(255) = NULL
AS
BEGIN
	IF (@CompanyID is NOT NULL)
	BEGIN
		UPDATE RD_COMPANY
		SET CompanyName = @CompanyName,
			CompanyAddress = @CompanyAddress,
			CompanyPostalCode = @CompanyPostalCode,
			CompanyPostalCodeDescription = @CompanyPostalCodeDescription,
			CompanyCountry = @CompanyCountry
		WHERE CompanyID = @CompanyID
	END
	ELSE
	BEGIN
		SELECT @CompanyID = CompanyID
		FROM RD_COMPANY
		WHERE CompanyCode = @CompanyCode
		IF (@CompanyID IS NULL)
		BEGIN
			INSERT INTO RD_COMPANY(CompanyID, CompanyName, CompanyAddress, CompanyPostalCode, CompanyPostalCodeDescription, CompanyCountry, CompanyCode, CompanyServer)
			SELECT ISNULL(MAX(CompanyID),0) + 1,
				@CompanyName,
				@CompanyAddress,
				@CompanyPostalCode,
				@CompanyPostalCodeDescription,
				@CompanyCountry,
				@CompanyCode,
				@CompanyServer
			FROM RD_COMPANY

			SELECT @CompanyID = CompanyID
			FROM RD_COMPANY
			WHERE CompanyCode = @CompanyCode
		END
	END
	RETURN @CompanyID
END
GO