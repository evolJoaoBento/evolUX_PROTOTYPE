IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_PRODUCTION_RUN_REPORT]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_PRODUCTION_RUN_REPORT] AS' 
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_PRODUCTION_RUN_REPORT] 
	@ServiceCompanyID int,
	@Filter bit = 1,
	@StartDateStr varchar(8) = '00000000',
	@EndDateStr varchar(8) = '99999999',
	@BusinessID int = NULL
--WITH ENCRYPTION
AS
	SET NOCOUNT ON
	DECLARE 
		@StartDate int,
		@EndDate int,
		@RecoverStateID int

	SELECT @StartDate = 00000000, @StartDateStr = RTRIM(@StartDateStr)
	IF (LEN(@StartDateStr) > 0)
		SELECT @StartDate = CAST(@StartDateStr as int)

	SELECT @EndDate = 99999999, @EndDateStr = RTRIM(@EndDateStr)
	IF (LEN(@EndDateStr) > 0)
		SELECT @EndDate = CAST(@EndDateStr as int)

	SELECT @RecoverStateID = RunStateID
	FROM dbo.RD_RUN_STATE WITH(NOLOCK)
	WHERE RunStateName = 'RECOVER'

	CREATE TABLE #RP_UX_RUN_REPORT(
		[RunID] int NULL,
		[ServiceCompanyID] int NULL,
		[BusinessID] int NULL,
		[BusinessDesc] varchar(256),
		[RunDate] int,
		[RunSequence] smallint,
		[ServiceCompany] varchar(256),
		[FilesLeftToPrint] int NULL,
		[RecFilesLeftToPrint] int NULL,
		[TotalProcessed] int NULL, 
		[StartProcessed] datetime NULL,
		[EndProcessed] datetime NULL,
		[TotalToPrint] int NULL, 
		[StartToPrint] datetime NULL,
		[EndToPrint] datetime NULL,
		[TotalS2Printer] int NULL, 
		[StartS2Printer] datetime NULL,
		[EndS2Printer] datetime NULL,
		[TotalPrinted] int NULL, 
		[StartPrinted] datetime NULL,
		[EndPrinted] datetime NULL,
		[TotalFullFill] int NULL, 
		[StartFullFill] datetime NULL,
		[EndFullFill] datetime NULL,
		[TotalExpedition] int NULL,
		[StartExpedition] datetime NULL,
		[EndExpedition] datetime NULL)

	INSERT INTO #RP_UX_RUN_REPORT([RunID], [ServiceCompanyID],
		[BusinessID], [BusinessDesc],
		[RunDate], [RunSequence],
		[ServiceCompany],
		[FilesLeftToPrint], [RecFilesLeftToPrint],
		[TotalProcessed], 
		[StartProcessed],
		[EndProcessed],
		[TotalToPrint], 
		[StartToPrint],
		[EndToPrint],
		[TotalS2Printer], 
		[StartS2Printer],
		[EndS2Printer],
		[TotalPrinted], 
		[StartPrinted],
		[EndPrinted],
		[TotalFullFill], 
		[StartFullFill],
		[EndFullFill],
		[TotalExpedition],
		[StartExpedition],
		[EndExpedition])
	SELECT	r.RunID, p.ServiceCompanyID,
		r.BusinessID, b.[Description],
		r.Rundate, r.RunSequence,
		c.CompanyCode,
		0, 0,
		COUNT(f.FileID),
		MIN(fFork.StartTimeStamp),
		MAX(fFork.EndTimeStamp),
		COUNT(fToPrint.FileID),
		MIN(fToPrint.StartTimeStamp),
		MAX(fToPrint.EndTimeStamp),
		COUNT(fS2Printer.FileID),
		MIN(fS2Printer.StartTimeStamp),
		MAX(fS2Printer.EndTimeStamp),
		COUNT(fPrinted.FileID),
		MIN(fPrinted.StartTimeStamp),
		MAX(fPrinted.EndTimeStamp),
		COUNT(fFullFill.FileID),
		MIN(fFullFill.StartTimeStamp),
		MAX(fFullFill.EndTimeStamp),
		COUNT(fExpedition.FileID),
		MIN(fExpedition.StartTimeStamp),
		MAX(fExpedition.EndTimeStamp)
	FROM
		RT_RUN r WITH(NOLOCK)
	INNER JOIN
		RD_BUSINESS b WITH(NOLOCK)
	ON b.BusinessID = r.BusinessID
	INNER JOIN
		RT_FILE_REGIST f WITH(NOLOCK)
	ON	r.RunID = f.RunID
	INNER JOIN
		RT_PRODUCTION_DETAIL p WITH(NOLOCK)
	ON	f.ProdDetailID = p.ProdDetailID
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK)
	ON	c.CompanyID = p.ServiceCompanyID
	INNER JOIN
		RT_FILE_LOG fFork WITH(NOLOCK)
	ON	f.RunID = fFork.RunID
		AND f.FileID = fFork.FileID
		AND fFork.RunStateID = 50--(SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'FORK')
		AND fFork.ProcCountNr = (SELECT MAX(ProcCountNr) FROM RT_FILE_LOG WITH(NOLOCK)
						WHERE RunID = fFork.RunID 
							AND FileID = fFork.FileID 
							AND RunStateID = fFork.RunStateID
							AND EndTimeStamp is NOT NULL
							AND ErrorID = 0)
	LEFT OUTER JOIN
		RT_FILE_LOG fToPrint WITH(NOLOCK)
	ON	f.RunID = fToPrint.RunID
		AND f.FileID = fToPrint.FileID
		AND fToPrint.RunStateID = 70--(SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'TOPRINT')
		AND fToPrint.ProcCountNr = (SELECT MAX(ProcCountNr) FROM RT_FILE_LOG WITH(NOLOCK)
						WHERE RunID = fToPrint.RunID 
							AND FileID = fToPrint.FileID 
							AND RunStateID = fToPrint.RunStateID
							AND EndTimeStamp is NOT NULL
							AND ErrorID = 0)
	LEFT OUTER JOIN
		RT_FILE_LOG fS2Printer WITH(NOLOCK)
	ON 	fS2Printer.RunID = f.RunID 
		AND fS2Printer.FileID = f.FileID 
		AND fS2Printer.RunStateID = 80--(SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'SEND2PRINTER')
		AND fS2Printer.ProcCountNr = (SELECT MAX(ProcCountNr) FROM RT_FILE_LOG WITH(NOLOCK) 
						WHERE RunID = fS2Printer.RunID 
							AND FileID = fS2Printer.FileID 
							AND RunStateID = fS2Printer.RunStateID
							AND EndTimeStamp is NOT NULL
							AND ErrorID = 0)
	LEFT OUTER JOIN
		RT_FILE_LOG fPrinted WITH(NOLOCK)
	ON 	fPrinted.RunID = f.RunID 
		AND fPrinted.FileID = f.FileID 
		AND fPrinted.RunStateID = 90--(SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'PRINTED')
		AND fPrinted.ProcCountNr = (SELECT MAX(ProcCountNr) FROM RT_FILE_LOG WITH(NOLOCK) 
						WHERE RunID = fPrinted.RunID 
							AND FileID = fPrinted.FileID 
							AND RunStateID = fPrinted.RunStateID
							AND EndTimeStamp is NOT NULL
							AND ErrorID = 0)
	LEFT OUTER JOIN
		RT_FILE_LOG fFullFill WITH(NOLOCK)
	ON 	fFullFill.RunID = f.RunID 
		AND fFullFill.FileID = f.FileID 
		AND fFullFill.RunStateID = 100--(SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'FULLFILLED')
		AND fFullFill.ProcCountNr = (SELECT MAX(ProcCountNr) FROM RT_FILE_LOG WITH(NOLOCK)
						WHERE RunID = fFullFill.RunID 
							AND FileID = fFullFill.FileID 
							AND RunStateID = fFullFill.RunStateID
							AND EndTimeStamp is NOT NULL
							AND ErrorID = 0)
	LEFT OUTER JOIN
		RT_FILE_LOG fExpedition WITH(NOLOCK)
	ON 	fExpedition.RunID = f.RunID 
		AND fExpedition.FileID = f.FileID 
		AND fExpedition.RunStateID = 110--(SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'EXPEDITION')
		AND fExpedition.ProcCountNr = (SELECT MAX(ProcCountNr) FROM RT_FILE_LOG WITH(NOLOCK)
						WHERE RunID = fExpedition.RunID 
							AND FileID = fExpedition.FileID 
							AND RunStateID = fExpedition.RunStateID
							AND ErrorID = 0 
							AND EndTimeStamp is NOT NULL)
	WHERE f.RunStateID <> @RecoverStateID
		AND f.ErrorID = 0
		AND (@Filter = 0 OR ISNULL(r.Closed,0) = 0)
		AND r.RunDate between @StartDate AND @EndDate
		AND p.ServiceCompanyID = @ServiceCompanyID
	GROUP BY r.RunID, p.ServiceCompanyID, r.BusinessID, b.[Description], r.Rundate, r.RunSequence, c.CompanyCode

	UPDATE #RP_UX_RUN_REPORT
	SET FilesLeftToPrint = x.FilesLeftToPrint,
		RecFilesLeftToPrint = x.RecFilesLeftToPrint
	FROM
		#RP_UX_RUN_REPORT u
	INNER JOIN
		(SELECT r.RunID, pd.ServiceCompanyID,
			COUNT(CASE WHEN f.RunStateID = @RecoverStateID THEN NULL ELSE f.FileID END) FilesLeftToPrint,
			COUNT(CASE WHEN f.RunStateID = @RecoverStateID THEN f.FileID ELSE NULL END) RecFilesLeftToPrint
		FROM
			RT_RUN r WITH(NOLOCK)
		INNER JOIN
			RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
		ON	pd.RunID = r.RunID
		INNER JOIN
			RT_FILE_REGIST f WITH(NOLOCK)
		ON	f.RunID = r.RunID
			AND f.ProdDetailID = pd.ProdDetailID
		WHERE
			EXISTS (SELECT TOP 1 1 
				FROM RT_FILE_LOG fl WITH(NOLOCK) 
				WHERE fl.RunID = f.RunID
					AND fl.FileID = f.FileID
					AND fl.ErrorID = 0
					AND fl.RunStateID = 70 --(SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'TOPRINT')
					AND fl.EndTimeStamp is NOT NULL
					)
			AND NOT EXISTS (SELECT TOP 1 1
				FROM RT_FILE_LOG fl WITH(NOLOCK) 
				WHERE fl.RunID = f.RunID
					AND fl.FileID = f.FileID
					AND fl.ErrorID = 0
					AND fl.RunStateID = 80 --(SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'SEND2PRINTER')
					)
			AND pd.ServiceCompanyID = @ServiceCompanyID
		GROUP BY r.RunID, pd.ServiceCompanyID) x
	ON x.RunID = u.RunID
		AND x.ServiceCompanyID = u.ServiceCompanyID

	SELECT *
	FROM #RP_UX_RUN_REPORT
	ORDER BY RunDate ASC, BusinessID ASC, RunID ASC, ServiceCompanyID ASC

	DROP TABLE #RP_UX_RUN_REPORT

	SET NOCOUNT OFF
GO
