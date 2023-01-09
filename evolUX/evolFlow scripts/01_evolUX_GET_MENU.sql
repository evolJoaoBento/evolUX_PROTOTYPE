IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'LocalizationKey' AND OBJECT_ID = OBJECT_ID(N'ACTIONS'))
BEGIN
	ALTER TABLE dbo.ACTIONS
	ADD [LocalizationKey] varchar(50) NULL DEFAULT (0)
END
GO
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_GET_MENU]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[evolUX_GET_MENU] AS'
END
GO
ALTER  PROCEDURE [dbo].[evolUX_GET_MENU]
	@ProfileList IDList READONLY
AS
BEGIN
	SET NOCOUNT ON
	SELECT DISTINCT x.[ActionIDLevel1], x.[DescriptionLevel1], x.[LocalizationKeyLevel1], x.ActionIDLevel2, x.[DescriptionLevel2], x.[LocalizationKeyLevel2], x.[URLLevel2], z.[ActionID] as [ActionIDLevel3], z.[Description] as [DescriptionLevel3], z.[LocalizationKey] as [LocalizationKeyLevel3], z.[URL] as [URLLevel3], x.ActionOrder [ActionOrderLevel2], ISNULL(z.ActionOrder, x.ActionOrder) [ActionOrderLevel1] 
	FROM 
		(SELECT pa.ActionID as [ActionIDLevel1], pa.[Description] as [DescriptionLevel1], pa.[LocalizationKey] as [LocalizationKeyLevel1], a.ActionID as [ActionIDLevel2], a.[Description] as [DescriptionLevel2], a.[LocalizationKey] as [LocalizationKeyLevel2], CASE WHEN ISNULL(a.evolGUI_TypeID, 0) = 1 THEN a.[URL] END as [URLLevel2], MAX(p.ActionOrder) ActionOrder 
		FROM
			ACTIONS a WITH(NOLOCK)
		INNER JOIN
			ACTIONS pa WITH(NOLOCK)
		ON a.ParActionID = pa.ActionID
		INNER JOIN
			[PERMISSIONS] p WITH(NOLOCK)
		ON p.ActionID = a.ActionID
		WHERE 	a.ActionTypeID = 1   
			AND ISNULL(a.evolGUI_TypeID,0) >= 0
			AND ISNULL(pa.evolGUI_TypeID,0) >= 0
			AND p.ProfileID in (SELECT ID FROM @ProfileList)
			AND p.FlowType in (SELECT DISTINCT FlowType
								FROM BACKUP_JOBS WITH(NOLOCK)
								UNION
								SELECT FlowType
								FROM FLOW_TYPE WITH(NOLOCK)
								WHERE NOT EXISTS(SELECT TOP 1 1 FROM BACKUP_JOBS WITH(NOLOCK)))   
			AND pa.ActionTypeID = 0  
		GROUP BY pa.ActionID, pa.[Description], pa.[LocalizationKey], a.ActionID, a.[Description], a.[LocalizationKey], a.[URL], a.evolGUI_TypeID) x  
	LEFT OUTER JOIN 
		(SELECT a.ActionID, a.[Description], a.[LocalizationKey], CASE WHEN ISNULL(a.evolGUI_TypeID, 0) = 1 THEN a.[URL] END [URL], a.ParActionID, a.ActionTypeID, MAX(p.ActionOrder) ActionOrder
		FROM 
			ACTIONS a WITH(NOLOCK)
		INNER JOIN
			[PERMISSIONS] p WITH(NOLOCK)
		ON a.ActionID = p.ActionID
		WHERE  p.ProfileID in (SELECT ID FROM @ProfileList)
			AND ISNULL(a.evolGUI_TypeID,0) >= 0
		GROUP BY a.ActionID, a.[URL], a.[Description], a.[LocalizationKey], a.ParActionID, a.ActionTypeID, a.evolGUI_TypeID) z  
	ON x.ActionIDLevel2 = z.ParActionID  
		AND z.ActionTypeID = 1
	ORDER BY x.ActionIDLevel1, x.ActionOrder, x.ActionIDLevel2, ISNULL(z.ActionOrder,x.ActionOrder) ASC

END