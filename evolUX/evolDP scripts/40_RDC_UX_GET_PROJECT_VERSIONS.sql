IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RDC_UX_GET_PROJECT_VERSIONS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RDC_UX_GET_PROJECT_VERSIONS] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RDC_UX_GET_PROJECT_VERSIONS]
	@CompanyBusinessList IDList READONLY
AS
BEGIN
	SELECT b.BusinessID, 
		b.BusinessCode,
		b.[Description] [BusinessDescription], 
		v.VersionID,
		v.[Type],
		v.ProjectCode, 
		v.StartDate, 
		v.StartTime, 
		v.Major,
		v.Minor,
		v.Revision,
		v.Patch,
		v.[Description]
	FROM
		@CompanyBusinessList bl
	INNER JOIN
		RD_BUSINESS b WITH(NOLOCK) 
	ON	bl.ID = b.BusinessID
	INNER JOIN
		RDC_VERSION v WITH(NOLOCK)
	ON	v.BusinessID = b.BusinessID
	ORDER BY b.BusinessID, v.[Type], v.ProjectCode, v.StartDate DESC, v.StartTime DESC
END
GO
