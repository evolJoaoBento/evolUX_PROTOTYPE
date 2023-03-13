IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RDC_UX_SUPORT_TYPE_OPTIONS]') AND type in (N'V'))
BEGIN
 EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[RDC_UX_SUPORT_TYPE_OPTIONS] AS SELECT 0 [ID], ''None'' [Code], ''None'' [GroupCode]' 
END
GO
ALTER  VIEW [dbo].[RDC_UX_SUPORT_TYPE_OPTIONS]
AS
	SELECT 1 [ID], 'Finishing' [Code], 'Finishing' [GroupCode]
	UNION
	SELECT 2 [ID], 'Archive' [Code], 'Archive' [GroupCode]
	UNION
	SELECT 12 [ID], 'GenerateCSV' [Code], 'Other' [GroupCode]
	UNION
	SELECT 18 [ID], 'ArchivePDF' [Code], 'Archive' [GroupCode]
	UNION
	SELECT 32 [ID], 'EmailBody' [Code], 'Email' [GroupCode]
	UNION
	SELECT 64 [ID], 'EmailAttachPDF' [Code], 'Email' [GroupCode]
	UNION
	SELECT 96 [ID], 'EmailBodyAttachPDF' [Code], 'Email' [GroupCode]
	UNION
	SELECT 224 [ID], 'ArchiveWithColor' [Code], 'Archive' [GroupCode]
GO
