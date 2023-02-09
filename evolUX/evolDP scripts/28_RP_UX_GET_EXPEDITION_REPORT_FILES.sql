IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_GET_EXPEDITION_REPORT_FILES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_GET_EXPEDITION_REPORT_FILES] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_GET_EXPEDITION_REPORT_FILES]
	@ExpReportID int
AS
BEGIN
	SELECT	f.RunID,
			f.FileID,
			f.[FileName],
			SUM(fr.Quantity) TotalPostalObjects
	FROM RT_FILE_EXPEDITION_REPORT fr
	INNER JOIN
		RT_FILE_REGIST f
	ON f.RunID = fr.RunID AND f.FileID = fr.FileID
	WHERE fr.ExpReportID = @ExpReportID
	GROUP BY f.RunID, f.FileID, f.[FileName]
	ORDER BY f.RunID, f.FileID
END
GO