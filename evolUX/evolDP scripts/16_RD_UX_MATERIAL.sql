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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_MATERIAL_TYPE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_MATERIAL_TYPE] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_MATERIAL_TYPE]
	@GroupCodes bit = 0,
	@MaterialTypeCode varchar(10) = NULL
AS
	SET NOCOUNT ON
	IF (@GroupCodes = 1)
	BEGIN
		SELECT mt.MaterialTypeID, x.MaterialTypeCode, mt.MaterialTypeDescription, 
			ISNULL((SELECT MAX(MaterialPosition) FROM RD_SERVICE_COMPANY_RESTRICTION WITH(NOLOCK) WHERE MaterialTypeID = mt.MaterialTypeID),0) MaxMaterialPosition
		FROM
			(SELECT MIN(MaterialTypeID) MaterialTypeID, 
				CASE WHEN MaterialTypeCode like '%Paper' THEN 'Paper'
				ELSE
					CASE WHEN MaterialTypeCode like '%Station' THEN 'Station'
					ELSE
						CASE WHEN MaterialTypeCode like '%Envelope' THEN 'Envelope'
						ELSE
							MaterialTypeCode
						END
					END
				END MaterialTypeCode
			FROM RD_MATERIAL_TYPE WITH(NOLOCK)
			GROUP BY (CASE WHEN MaterialTypeCode like '%Paper' THEN 'Paper'
				ELSE
					CASE WHEN MaterialTypeCode like '%Station' THEN 'Station'
					ELSE
						CASE WHEN MaterialTypeCode like '%Envelope' THEN 'Envelope'
						ELSE
							MaterialTypeCode
						END
					END
				END)) x
		INNER JOIN
			RD_MATERIAL_TYPE mt
		ON mt.MaterialTypeID = x.MaterialTypeID
		ORDER BY mt.MaterialTypeID ASC
	END
	ELSE
	BEGIN
		SELECT mt.MaterialTypeID, mt.MaterialTypeCode, mt.MaterialTypeDescription, 
			ISNULL((SELECT MAX(MaterialPosition) FROM RD_SERVICE_COMPANY_RESTRICTION WITH(NOLOCK) WHERE MaterialTypeID = mt.MaterialTypeID),0) MaxMaterialPosition
		FROM RD_MATERIAL_TYPE mt WITH(NOLOCK)
		WHERE @MaterialTypeCode is NULL OR mt.MaterialTypeCode like ('%' + @MaterialTypeCode)
		ORDER BY mt.MaterialTypeID ASC
	END
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_MATERIAL]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_MATERIAL] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_MATERIAL]
	@MaterialID int = NULL,
	@MaterialRef varchar(20) = NULL,
	@MaterialTypeID int = NULL,
	@MaterialTypeCode varchar(10) = NULL,
	@MaterialCode varchar(20) = NULL,
	@GroupID int = NULL,
	@ServiceCompanyList IDList READONLY
AS
	SET NOCOUNT ON
	IF (@MaterialID is NOT NULL OR @MaterialRef is NOT NULL)
	BEGIN
		SELECT m.MaterialID, m.MaterialTypeID, m.MaterialCode, m.MaterialDescription, m.MaterialWeight, m.MaterialRef, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight, m.GroupID,
			mcc.ServiceCompanyID, mcc.ProviderCompanyID, mcc.CostDate, mcc.MaterialCost, mcc.MaterialBinPosition, ISNULL(mcc.MaterialPosition,0) ServiceCompanyMaterialPosition
		FROM RD_MATERIAL m WITH(NOLOCK)
		LEFT OUTER JOIN
			(SELECT mc.ServiceCompanyID, mc.MaterialID, mc.ProviderCompanyID, mc.CostDate, mc.MaterialCost, mc.MaterialBinPosition, scr.MaterialPosition, scr.MaterialTypeID
			FROM RD_MATERIAL mm WITH(NOLOCK)
			INNER JOIN
				RD_MATERIAL_COST mc WITH(NOLOCK)
			ON mm.MaterialID = mc.MaterialID
			INNER JOIN
				@ServiceCompanyList s
			ON s.ID = mc.ServiceCompanyID
			LEFT OUTER JOIN
				RD_SERVICE_COMPANY_RESTRICTION scr WITH(NOLOCK)
			ON scr.ServiceCompanyID = mc.ServiceCompanyID
				AND scr.MaterialTypeID = mm.MaterialTypeID
			WHERE mc.CostDate = (SELECT MAX(CostDate)
								FROM RD_MATERIAL_COST WITH(NOLOCK)
								WHERE ServiceCompanyID = mc.ServiceCompanyID
									AND MaterialID = mc.MaterialID)) mcc
		ON mcc.MaterialID = m.MaterialID
		WHERE m.MaterialID = ISNULL(@MaterialID,m.MaterialID)
			OR
			m.MaterialRef = ISNULL(@MaterialRef,m.MaterialRef)
		ORDER BY m.MaterialRef ASC, mcc.CostDate DESC, mcc.ServiceCompanyID ASC
	END
	ELSE
	BEGIN
		IF (@MaterialTypeCode is NOT NULL)
		BEGIN
			SELECT m.MaterialID, m.MaterialTypeID, m.MaterialCode, m.MaterialDescription, m.MaterialWeight, m.MaterialRef, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight, m.GroupID,
				mcc.ServiceCompanyID, mcc.ProviderCompanyID, mcc.CostDate, mcc.MaterialCost, mcc.MaterialBinPosition, ISNULL(mcc.MaterialPosition,0) ServiceCompanyMaterialPosition
			FROM RD_MATERIAL m WITH(NOLOCK)
			INNER JOIN
				RD_MATERIAL_TYPE mt WITH(NOLOCK)
			ON mt.MaterialTypeID = m.MaterialTypeID
			LEFT OUTER JOIN
				(SELECT mc.ServiceCompanyID, mc.MaterialID, mc.ProviderCompanyID, mc.CostDate, mc.MaterialCost, mc.MaterialBinPosition, scr.MaterialPosition, scr.MaterialTypeID
				FROM RD_MATERIAL mm WITH(NOLOCK)
				INNER JOIN
					RD_MATERIAL_COST mc WITH(NOLOCK)
				ON mm.MaterialID = mc.MaterialID
				INNER JOIN
					@ServiceCompanyList s
				ON s.ID = mc.ServiceCompanyID
				LEFT OUTER JOIN
					RD_SERVICE_COMPANY_RESTRICTION scr WITH(NOLOCK)
				ON scr.ServiceCompanyID = mc.ServiceCompanyID
					AND scr.MaterialTypeID = mm.MaterialTypeID
				WHERE mc.CostDate = (SELECT MAX(CostDate)
								FROM RD_MATERIAL_COST WITH(NOLOCK)
								WHERE ServiceCompanyID = mc.ServiceCompanyID
									AND MaterialID = mc.MaterialID)) mcc
			ON mcc.MaterialID = m.MaterialID
			WHERE (@MaterialCode is NULL OR m.MaterialCode = @MaterialCode)
				AND mt.MaterialTypeCode like ('%' + @MaterialTypeCode)
				AND (@GroupID is NULL OR GroupID = @GroupID)
			ORDER BY m.MaterialRef ASC, mcc.CostDate DESC, mcc.ServiceCompanyID ASC
		END
		ELSE
		BEGIN
			SELECT m.MaterialID, m.MaterialTypeID, m.MaterialCode, m.MaterialDescription, m.MaterialWeight, m.MaterialRef, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight, m.GroupID,
				mcc.ServiceCompanyID, mcc.ProviderCompanyID, mcc.CostDate, mcc.MaterialCost, mcc.MaterialBinPosition, ISNULL(mcc.MaterialPosition,0) ServiceCompanyMaterialPosition
			FROM RD_MATERIAL m WITH(NOLOCK)
			INNER JOIN
				RD_MATERIAL_TYPE mt WITH(NOLOCK)
			ON mt.MaterialTypeID = m.MaterialTypeID
			LEFT OUTER JOIN
				(SELECT mc.ServiceCompanyID, mc.MaterialID, mc.ProviderCompanyID, mc.CostDate, mc.MaterialCost, mc.MaterialBinPosition, scr.MaterialPosition, scr.MaterialTypeID
				FROM RD_MATERIAL mm WITH(NOLOCK)
				INNER JOIN
					RD_MATERIAL_COST mc WITH(NOLOCK)
				ON mm.MaterialID = mc.MaterialID
				INNER JOIN
					@ServiceCompanyList s
				ON s.ID = mc.ServiceCompanyID
				LEFT OUTER JOIN
					RD_SERVICE_COMPANY_RESTRICTION scr WITH(NOLOCK)
				ON scr.ServiceCompanyID = mc.ServiceCompanyID
					AND scr.MaterialTypeID = mm.MaterialTypeID
				WHERE mc.CostDate = (SELECT MAX(CostDate)
								FROM RD_MATERIAL_COST WITH(NOLOCK)
								WHERE ServiceCompanyID = mc.ServiceCompanyID
									AND MaterialID = mc.MaterialID)) mcc
			ON mcc.MaterialID = m.MaterialID
			WHERE (@MaterialCode is NULL OR m.MaterialCode = @MaterialCode)
				AND (@MaterialTypeID is NULL OR mt.MaterialTypeID = @MaterialTypeID)
				AND (@GroupID is NULL OR GroupID = @GroupID)
			ORDER BY m.MaterialRef ASC, mcc.CostDate DESC, mcc.ServiceCompanyID ASC
		END
	END
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_MATERIAL]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_MATERIAL] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_SET_MATERIAL]
	@MaterialID int = NULL,
	@MaterialRef varchar(20),
	@MaterialTypeID int,
	@MaterialCode varchar(20),
	@MaterialDescription varchar(50) = NULL, 
	@MaterialWeight float = NULL,
	@FullFillSheets int = NULL,
	@FullFillMaterialCode varchar(10) = NULL,
	@ExpeditionMinWeight float = NULL,
	@GroupID int = NULL,
	@ProviderCompanyID int = NULL,
	@ServiceCompanyID int = NULL,
	@CostDate int = 0,
	@MaterialCost float = 0,
	@MaterialBinPosition smallint = NULL,
	@ServiceCompanyList IDList READONLY
AS
	SET NOCOUNT ON
	IF (@MaterialID is NULL)
	BEGIN
		IF (NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[RD_MATERIAL] WHERE MaterialRef = @MaterialRef))
		BEGIN
			IF (NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[RD_MATERIAL] WHERE MaterialCode = @MaterialCode AND MaterialTypeID = @MaterialTypeID))
			BEGIN
				INSERT INTO [dbo].[RD_MATERIAL](MaterialID, MaterialRef, MaterialTypeID, MaterialCode, MaterialDescription, MaterialWeight, FullFillSheets, FullFillMaterialCode, ExpeditionMinWeight, GroupID)
				SELECT ISNULL(MAX(MaterialID),0) + 1, @MaterialRef, @MaterialTypeID, @MaterialCode, @MaterialDescription, @MaterialWeight, @FullFillSheets, @FullFillMaterialCode, @ExpeditionMinWeight, @GroupID
				FROM RD_MATERIAL

				SELECT @MaterialID = MaterialID 
				FROM [dbo].[RD_MATERIAL]
				WHERE MaterialRef = @MaterialRef

				INSERT INTO RD_MATERIAL_COST(MaterialID, ServiceCompanyID, ProviderCompanyID, CostDate, MaterialCost, MaterialBinPosition)
				SELECT @MaterialID, s.ID, @ProviderCompanyID, @CostDate, @MaterialCost, @MaterialBinPosition
				FROM @ServiceCompanyList s
			END
			ELSE
			BEGIN
				SELECT @MaterialID = MaterialID 
				FROM [dbo].[RD_MATERIAL]
				WHERE MaterialCode = @MaterialCode AND MaterialTypeID = @MaterialTypeID
			END
		END
		ELSE
		BEGIN
			SELECT @MaterialID = MaterialID 
			FROM [dbo].[RD_MATERIAL]
			WHERE MaterialRef = @MaterialRef
		END
	END
	ELSE
	BEGIN
		IF (NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[RD_MATERIAL] WHERE MaterialCode = @MaterialCode AND MaterialTypeID = @MaterialTypeID))
		BEGIN
			UPDATE [dbo].[RD_MATERIAL]
			SET MaterialCode = @MaterialCode,
				MaterialTypeID = @MaterialTypeID,
				MaterialDescription = @MaterialDescription, 
				MaterialWeight = @MaterialWeight,
				FullFillSheets = @FullFillSheets,
				FullFillMaterialCode = @FullFillMaterialCode, 
				ExpeditionMinWeight = @ExpeditionMinWeight
			WHERE MaterialID = @MaterialID
		END
		ELSE
		BEGIN
			UPDATE [dbo].[RD_MATERIAL]
			SET MaterialDescription = @MaterialDescription, 
				MaterialWeight = @MaterialWeight,
				FullFillSheets = @FullFillSheets,
				FullFillMaterialCode = @FullFillMaterialCode, 
				ExpeditionMinWeight = @ExpeditionMinWeight
			WHERE MaterialID = @MaterialID
		END
		
		IF (@ServiceCompanyID is NOT NULL)
		BEGIN
			EXEC RD_UX_SET_MATERIAL_COST @MaterialID, @ServiceCompanyID, @CostDate, @MaterialCost, @ProviderCompanyID, @MaterialBinPosition
		END
	END
RETURN @MaterialID
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_MATERIAL_COST]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_MATERIAL_COST] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_SET_MATERIAL_COST]
	@MaterialID int,
	@ServiceCompanyID int,
	@CostDate int,
	@MaterialCost float,
	@ProviderCompanyID int = NULL,
	@MaterialBinPosition smallint = NULL
AS
	SET NOCOUNT ON
	IF (NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[RD_MATERIAL_COST]
					WHERE MaterialID = @MaterialID 
							AND ServiceCompanyID = @ServiceCompanyID
							AND CostDate = @CostDate))
	BEGIN
		INSERT INTO [dbo].[RD_MATERIAL_COST](MaterialID, ServiceCompanyID, CostDate, ProviderCompanyID, MaterialCost, MaterialBinPosition)
		SELECT @MaterialID, @ServiceCompanyID, @CostDate,  @ProviderCompanyID, @MaterialCost, @MaterialBinPosition
	END
	ELSE
	BEGIN
		UPDATE RD_MATERIAL_COST
		SET ProviderCompanyID = @ProviderCompanyID,
			CostDate = @CostDate,
			MaterialCost = @MaterialCost,
			MaterialBinPosition = @MaterialBinPosition
		WHERE MaterialID = @MaterialID AND ServiceCompanyID = @ServiceCompanyID AND CostDate = @CostDate
	END
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_MATERIAL_GROUP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_MATERIAL_GROUP] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_MATERIAL_GROUP]
	@GroupID int = NULL,
	@MaterialTypeID int = NULL,
	@MaterialTypeCode varchar(10) = NULL,
	@GroupCode varchar(20) = NULL,
	@ServiceCompanyList IDList READONLY
AS
	SET NOCOUNT ON
	IF (@MaterialTypeCode is NOT NULL)
	BEGIN
		SELECT m.GroupID, m.MaterialTypeID, m.GroupCode, m.GroupDescription, m.MaterialWeight, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight,
				mcc.ServiceCompanyID, mcc.ProviderCompanyID, mcc.CostDate, mcc.MaterialCost, mcc.MaterialBinPosition, ISNULL(mcc.MaterialPosition,0) ServiceCompanyMaterialPosition
		FROM RD_MATERIAL_GROUP m WITH(NOLOCK)
		INNER JOIN
			RD_MATERIAL_TYPE mt WITH(NOLOCK)
		ON mt.MaterialTypeID = m.MaterialTypeID
		LEFT OUTER JOIN
			(SELECT mc.ServiceCompanyID, mc.GroupID, mc.ProviderCompanyID, mc.CostDate, mc.MaterialCost, mc.MaterialBinPosition, scr.MaterialPosition, scr.MaterialTypeID
			FROM RD_MATERIAL_GROUP mm WITH(NOLOCK)
			INNER JOIN
				RD_MATERIAL_GROUP_COST mc WITH(NOLOCK)
			ON mm.GroupID = mc.GroupID
			INNER JOIN
				@ServiceCompanyList s
			ON s.ID = mc.ServiceCompanyID
			INNER JOIN
				RD_SERVICE_COMPANY_RESTRICTION scr WITH(NOLOCK)
			ON scr.ServiceCompanyID = mc.ServiceCompanyID
				AND scr.MaterialTypeID = mm.MaterialTypeID
			WHERE mc.CostDate = (SELECT MAX(CostDate)
								FROM RD_MATERIAL_GROUP_COST WITH(NOLOCK)
								WHERE ServiceCompanyID = mc.ServiceCompanyID
									AND GroupID = mc.GroupID)) mcc
		ON mcc.GroupID = m.GroupID
		WHERE (@GroupCode is NULL OR m.GroupCode = @GroupCode)
			AND mt.MaterialTypeCode like ('%' + @MaterialTypeCode)
			AND m.GroupID = ISNULL(@GroupID,m.GroupID)
		ORDER BY m.GroupCode ASC, mcc.CostDate DESC, mcc.ServiceCompanyID ASC
	END
	ELSE
	BEGIN
		SELECT m.GroupID, m.MaterialTypeID, m.GroupCode, m.GroupDescription, m.MaterialWeight, m.FullFillSheets, m.FullFillMaterialCode, m.ExpeditionMinWeight,
				mcc.ServiceCompanyID, mcc.ProviderCompanyID, mcc.CostDate, mcc.MaterialCost, mcc.MaterialBinPosition, ISNULL(mcc.MaterialPosition,0) ServiceCompanyMaterialPosition
		FROM RD_MATERIAL_GROUP m WITH(NOLOCK)
		INNER JOIN
			RD_MATERIAL_TYPE mt WITH(NOLOCK)
		ON mt.MaterialTypeID = m.MaterialTypeID
		LEFT OUTER JOIN
			(SELECT mc.ServiceCompanyID, mc.GroupID, mc.ProviderCompanyID, mc.CostDate, mc.MaterialCost, mc.MaterialBinPosition, scr.MaterialPosition, scr.MaterialTypeID
			FROM RD_MATERIAL_GROUP mm WITH(NOLOCK)
			INNER JOIN
				RD_MATERIAL_GROUP_COST mc WITH(NOLOCK)
			ON mm.GroupID = mc.GroupID
			INNER JOIN
				@ServiceCompanyList s
			ON s.ID = mc.ServiceCompanyID
			LEFT OUTER JOIN
				RD_SERVICE_COMPANY_RESTRICTION scr WITH(NOLOCK)
			ON scr.ServiceCompanyID = mc.ServiceCompanyID
				AND scr.MaterialTypeID = mm.MaterialTypeID
			WHERE mc.CostDate = (SELECT MAX(CostDate)
								FROM RD_MATERIAL_GROUP_COST WITH(NOLOCK)
								WHERE ServiceCompanyID = mc.ServiceCompanyID
									AND GroupID = mc.GroupID)) mcc
		ON mcc.GroupID = m.GroupID
		WHERE (@GroupCode is NULL OR m.GroupCode = @GroupCode)
			AND (@MaterialTypeID is NULL OR mt.MaterialTypeID = @MaterialTypeID)
			AND m.GroupID = ISNULL(@GroupID,m.GroupID)
		ORDER BY m.GroupCode ASC, mcc.CostDate DESC, mcc.ServiceCompanyID ASC
	END
RETURN
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_SET_MATERIAL_GROUP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_SET_MATERIAL_GROUP] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_SET_MATERIAL_GROUP]
	@GroupID int = NULL,
	@MaterialTypeID int,
	@GroupCode varchar(20),
	@GroupDescription varchar(50) = NULL, 
	@MaterialWeight float = NULL,
	@FullFillSheets int = NULL,
	@FullFillMaterialCode varchar(10) = NULL,
	@ExpeditionMinWeight float = NULL,
	@ProviderCompanyID int = NULL,
	@ServiceCompanyID int = NULL,
	@CostDate int = 0,
	@MaterialCost float = 0,
	@MaterialBinPosition smallint = NULL,
	@ServiceCompanyList IDList READONLY
AS
	SET NOCOUNT ON
	IF (@GroupID is NULL)
	BEGIN
		IF (NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[RD_MATERIAL_GROUP] WHERE GroupCode = @GroupCode))
		BEGIN
			INSERT INTO [dbo].[RD_MATERIAL_GROUP](GroupID, MaterialTypeID, GroupCode, GroupDescription, MaterialWeight, FullFillSheets, FullFillMaterialCode, ExpeditionMinWeight)
			SELECT ISNULL(MAX(GroupID),0) + 1, @MaterialTypeID, @GroupCode, @GroupDescription, @MaterialWeight, @FullFillSheets, @FullFillMaterialCode, @ExpeditionMinWeight
			FROM RD_MATERIAL_GROUP

			SELECT @GroupID = GroupID 
			FROM [dbo].[RD_MATERIAL_GROUP]
			WHERE GroupCode = @GroupCode

			IF (@ServiceCompanyID is NULL)
			BEGIN
				INSERT INTO RD_MATERIAL_GROUP_COST(GroupID, ServiceCompanyID, ProviderCompanyID, CostDate, MaterialCost, MaterialBinPosition)
				SELECT @GroupID, s.ID, @ProviderCompanyID, @CostDate, @MaterialCost, @MaterialBinPosition
				FROM @ServiceCompanyList s
			END
			ELSE
			BEGIN
				INSERT INTO RD_MATERIAL_GROUP_COST(GroupID, ServiceCompanyID, ProviderCompanyID, CostDate, MaterialCost, MaterialBinPosition)
				SELECT @GroupID, @ServiceCompanyID, @ProviderCompanyID, @CostDate, @MaterialCost, @MaterialBinPosition
			END
		END
	END
	ELSE
	BEGIN
		UPDATE [dbo].[RD_MATERIAL_GROUP]
		SET GroupDescription = @GroupDescription, 
			MaterialWeight = @MaterialWeight,
			FullFillSheets = @FullFillSheets,
			FullFillMaterialCode = @FullFillMaterialCode, 
			ExpeditionMinWeight = @ExpeditionMinWeight
		WHERE GroupID = @GroupID

		IF (@ServiceCompanyID is NOT NULL)
		BEGIN
			UPDATE RD_MATERIAL_GROUP_COST
			SET ProviderCompanyID = @ProviderCompanyID,
				CostDate = @CostDate,
				MaterialCost = @MaterialCost,
				MaterialBinPosition = @MaterialBinPosition
			WHERE GroupID = @GroupID AND ServiceCompanyID = @ServiceCompanyID
		END
	END
RETURN @GroupID
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_ENVELOPE_MEDIA_GROUP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_ENVELOPE_MEDIA_GROUP] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_ENVELOPE_MEDIA_GROUP]
	@EnvMediaGroupID int = NULL
AS
BEGIN
	SET NOCOUNT ON
	SELECT  EnvMediaGroupID, [Description], [DefaultEnvMediaID]
	FROM RD_ENVELOPE_MEDIA_GROUP WITH(NOLOCK)
	WHERE @EnvMediaGroupID is NULL OR EnvMediaGroupID = @EnvMediaGroupID
	ORDER BY [Description]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RD_UX_GET_ENVELOPE_MEDIA]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RD_UX_GET_ENVELOPE_MEDIA] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RD_UX_GET_ENVELOPE_MEDIA]
	@EnvMediaID int = NULL
AS
BEGIN
	SET NOCOUNT ON
	SELECT  EnvMediaID, EnvMediaName, [Description]
	FROM RD_ENVELOPE_MEDIA WITH(NOLOCK)
	WHERE @EnvMediaID is NULL OR EnvMediaID = @EnvMediaID
	ORDER BY [Description]
END
GO