IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_SERVICECOMPANY_PENDING_REGIST]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_SERVICECOMPANY_PENDING_REGIST] AS'
END
GO
ALTER   PROCEDURE [dbo].[RP_UX_SERVICECOMPANY_PENDING_REGIST]
	@ServiceCompanyList IDList READONLY
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @RetStr varchar(5000)

	DECLARE @ToPrintStateID int,
		@PrintedStateID int,
		@FullFillStateID int,
		@Send2PrinterID int
	
		SELECT @Send2PrinterID = RunStateID 
		FROM RD_RUN_STATE 
		WHERE RunStateName='SEND2PRINTER'

		SELECT @ToPrintStateID = RunStateID 
		FROM RD_RUN_STATE 
		WHERE RunStateName = 'TOPRINT'
	
		SELECT @PrintedStateID = RunStateID 
		FROM RD_RUN_STATE 
		WHERE RunStateName = 'PRINTED'
	
		SELECT @FullFillStateID = RunStateID 
		FROM RD_RUN_STATE 
		WHERE RunStateName = 'FULLFILLED'

	SELECT fx.RunID, fx.BusinessID, fx.[BusinessDesc], fx.RunDate, fx.RunSequence, 
		fx.[TotalProcessed], 
		fx.[FilesLeftToPrint],
		fx.[FilesLeftToRegistPrint],
		fx.[FilesLeftToRegistFullFill],
		SUM(fp.TotalPrint) TotalPrint,
		SUM(fp.TotalPostObjs) TotalPostObjs
	FROM 
		(SELECT fl1.RunID, b.BusinessID, b.[Description] [BusinessDesc], r.RunDate, r.RunSequence, 
			COUNT(DISTINCT fl1.FileID) as [TotalProcessed], 
			COUNT(DISTINCT fl1.FileID) - COUNT(DISTINCT fl2.FileID) as [FilesLeftToPrint],
			COUNT(DISTINCT fl2.FileID) - COUNT(DISTINCT fl3.FileID) as [FilesLeftToRegistPrint],
			COUNT(DISTINCT fl3.FileID) - COUNT(DISTINCT fl4.FileID) as [FilesLeftToRegistFullFill],
			COUNT(DISTINCT fl1.FileID) - COUNT(DISTINCT fl4.FileID) as [FilesLeftToFullFill]
		FROM
			RT_RUN r WITH(NOLOCK)
		INNER JOIN 
			RD_BUSINESS b WITH(NOLOCK)
		ON 	r.BusinessID = b.BusinessID
		INNER JOIN 
			RT_FILE_REGIST f WITH(NOLOCK)
		ON	f.RunID = r.RunID
		INNER JOIN
			RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
		ON	f.ProdDetailID = pd.ProdDetailID
		INNER JOIN
			@ServiceCompanyList s
		ON	pd.ServiceCompanyID = s.ID
		INNER JOIN
			RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
		ON	pd.ExpCode = est.ExpCode
			AND EXISTS (SELECT TOP 1 1
				FROM RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
				INNER JOIN
					RD_SERVICE_TYPE st WITH(NOLOCK)
				ON stst.ServiceTypeID = st.ServiceTypeID
				WHERE stst.ServiceTaskID = est.ServiceTaskID
					AND st.ServiceTypeCode like 'PRINT%')
		INNER JOIN
			RT_FILE_LOG fl1 WITH(NOLOCK)
		ON	f.RunID = fl1.RunID
			AND f.FileID = fl1.FileID
			AND fl1.RunStateID = @ToPrintStateID
			AND fl1.ErrorID = 0
			AND fl1.EndTimeStamp is NOT NULL
			AND fl1.ProcCountNr = (SELECT MAX(ProcCountNr)
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = fl1.RunID
						AND FileID = fl1.FileID
						AND RunStateID = fl1.RunStateID
						AND ErrorID = 0
						AND EndTimeStamp is NOT NULL)
		LEFT OUTER JOIN 
			RT_FILE_LOG fl2 WITH(NOLOCK)
		ON 	fl1.RunID = fl2.RunID
			AND fl1.FileID = fl2.FileID
			AND fl2.runstateid = @Send2PrinterID
			AND fl2.ErrorID = 0
			AND fl2.EndTimeStamp is NOT NULL
			AND fl2.ProcCountNr = (SELECT MAX(ProcCountNr)
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = fl2.RunID
						AND FileID = fl2.FileID
						AND RunStateID = fl2.RunStateID
						AND ErrorID = 0
						AND EndTimeStamp is NOT NULL)
		LEFT OUTER JOIN 
			RT_FILE_LOG fl3 WITH(NOLOCK)
		ON 	fl1.RunID = fl3.RunID
			AND fl1.FileID = fl3.FileID
			AND fl3.RunStateID = @PrintedStateID
			AND fl3.ErrorID = 0
			AND fl3.Endtimestamp is NOT NULL
			AND fl3.ProcCountNr = (SELECT MAX(ProcCountNr)
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = fl3.RunID
						AND FileID = fl3.FileID
						AND RunStateID = fl3.RunStateID
						AND ErrorID = 0
						AND EndTimeStamp is NOT NULL)
		LEFT OUTER JOIN 
			RT_FILE_LOG fl4 WITH(NOLOCK)
		ON 	fl1.RunID = fl4.RunID
			AND fl1.FileID = fl4.FileID
			AND fl4.ErrorID = 0
			AND fl4.EndTimeStamp is NOT NULL
			AND fl4.ProcCountNr = (SELECT MAX(ProcCountNr)
					FROM RT_FILE_LOG WITH(NOLOCK)
					WHERE RunID = fl4.RunID
						AND FileID = fl4.FileID
						AND RunStateID = fl4.RunStateID
						AND ErrorID = 0
						AND EndTimeStamp is NOT NULL)
			AND (
					(NOT EXISTS (SELECT TOP 1 1
							FROM RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
							INNER JOIN
								RD_SERVICE_TYPE st WITH(NOLOCK)
							ON	stst.ServiceTypeID = st.ServiceTypeID
							WHERE stst.ServiceTaskID = est.ServiceTaskID
								AND st.ServiceTypeCode like 'FULLFILL%') 
						AND fl3.RunStateID = fl4.RunStateID)
				OR fl4.RunStateID = @FullFillStateID
				)
	WHERE f.ErrorID = 0
		AND ISNULL(r.Closed,0) = 0 
	GROUP BY fl1.RunID, b.BusinessID, b.[Description], r.RunDate, r.RunSequence) fx
	INNER JOIN
		RT_FILE_PRODUCTION fp WITH(NOLOCK)
	ON fp.RunID = fx.RunID
	WHERE 	fx.[FilesLeftToFullFill] > 0
		AND (fx.[FilesLeftToPrint] > 0
			OR fx.[FilesLeftToRegistPrint] > 0 
			OR fx.[FilesLeftToRegistFullFill] > 0)
	GROUP BY fx.RunID, fx.BusinessID, fx.[BusinessDesc], fx.RunDate, fx.RunSequence, 
		fx.[TotalProcessed], 
		fx.[FilesLeftToPrint],
		fx.[FilesLeftToRegistPrint],
		fx.[FilesLeftToRegistFullFill]

END
