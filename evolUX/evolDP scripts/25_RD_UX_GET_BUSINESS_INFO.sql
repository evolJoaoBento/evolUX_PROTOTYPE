IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_BUSINESS_INFO]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_BUSINESS_INFO] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_BUSINESS_INFO]
	@BusinessID int = NULL,
	@CompanyID int = NULL,
	@CompanyList IDList READONLY
AS
BEGIN
	IF (@BusinessID = 0)
		SET @BusinessID = NULL
	IF (@BusinessID is NOT NULL)
	BEGIN
		SELECT b.BusinessID, 
			b.BusinessCode,
			b.CompanyID, 
			b.[Description], 
			b.FileSheetsCutoffLevel, 
			ISNULL(b.InternalExpeditionMode, 2) InternalExpeditionMode, 
			b.InternalCodeStart, 
			b.InternalCodeLen, 
			b.ExternalExpeditionMode, 
			b.TotalBannerPages, 
			b.PostObjOrderMode
		FROM
			RD_BUSINESS b WITH(NOLOCK) 
		WHERE b.BusinessID = @BusinessID
		ORDER BY b.BusinessID
	END
	ELSE
	BEGIN
		IF (@CompanyID is NOT NULL)
		BEGIN
			SELECT b.BusinessID, 
				b.BusinessCode,
				b.CompanyID, 
				b.[Description], 
				b.FileSheetsCutoffLevel, 
				ISNULL(b.InternalExpeditionMode, 2) InternalExpeditionMode, 
				b.InternalCodeStart, 
				b.InternalCodeLen, 
				b.ExternalExpeditionMode, 
				b.TotalBannerPages, 
				b.PostObjOrderMode
			FROM
				RD_BUSINESS b WITH(NOLOCK) 
			WHERE b.CompanyID = @CompanyID
			ORDER BY b.BusinessID
		END
		ELSE
		BEGIN
			SELECT b.BusinessID, 
				b.BusinessCode,
				b.CompanyID, 
				b.[Description], 
				b.FileSheetsCutoffLevel, 
				ISNULL(b.InternalExpeditionMode, 2) InternalExpeditionMode, 
				b.InternalCodeStart, 
				b.InternalCodeLen, 
				b.ExternalExpeditionMode, 
				b.TotalBannerPages, 
				b.PostObjOrderMode
			FROM
				@CompanyList c
			INNER JOIN
				RD_BUSINESS b WITH(NOLOCK) 
			ON	c.ID = b.CompanyID
			ORDER BY b.BusinessID
		END
	END
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_BUSINESS_INFO]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_BUSINESS_INFO] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_SET_BUSINESS_INFO]
	@BusinessID int = NULL,
	@BusinessCode varchar(10),
	@CompanyID int,
	@Description varchar(256) = NULL,
	@FileSheetsCutoffLevel int = NULL,
	@InternalExpeditionMode tinyint = NULL,
	@InternalCodeStart tinyint = NULL,
	@InternalCodeLen tinyint = NULL,
	@ExternalExpeditionMode tinyint = NULL,
	@TotalBannerPages tinyint = NULL,
	@PostObjOrderMode tinyint = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF (@BusinessID = 0)
		SET @BusinessID = NULL
	IF (@BusinessID is NOT NULL)
	BEGIN
		UPDATE RD_BUSINESS
		SET [Description] = @Description, 
			FileSheetsCutoffLevel = @FileSheetsCutoffLevel, 
			InternalExpeditionMode = ISNULL(@InternalExpeditionMode, 2), 
			InternalCodeStart = @InternalCodeStart, 
			InternalCodeLen = @InternalCodeLen, 
			ExternalExpeditionMode = @ExternalExpeditionMode, 
			TotalBannerPages = @TotalBannerPages, 
			PostObjOrderMode = @PostObjOrderMode
		WHERE BusinessID = @BusinessID
	END
	ELSE
	BEGIN
		INSERT INTO RD_BUSINESS(BusinessID, BusinessCode, CompanyID, [Description], FileSheetsCutoffLevel, InternalExpeditionMode, InternalCodeStart, InternalCodeLen, ExternalExpeditionMode, TotalBannerPages, PostObjOrderMode)
		SELECT ISNULL(MAX(BusinessID),0) + 1, 
				@BusinessCode,
				@CompanyID, 
				@Description, 
				@FileSheetsCutoffLevel, 
				ISNULL(@InternalExpeditionMode, 2), 
				@InternalCodeStart, 
				@InternalCodeLen, 
				@ExternalExpeditionMode, 
				@TotalBannerPages, 
				@PostObjOrderMode
			FROM RD_BUSINESS b

		SELECT @BusinessID = BusinessID
		FROM RD_BUSINESS
		WHERE BusinessCode = @BusinessCode AND CompanyID = @CompanyID
	END
	RETURN @BusinessID
END
GO