IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_BUSINESS_INFO]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_BUSINESS_INFO] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_BUSINESS_INFO]
	@BusinessID int = NULL,
	@CompanyID int = NULL,
	@CompanyBusinessList IDList READONLY
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
				@CompanyBusinessList bl
			INNER JOIN
				RD_BUSINESS b WITH(NOLOCK) 
			ON	bl.ID = b.BusinessID
			ORDER BY b.BusinessID
		END
	END
END
GO
