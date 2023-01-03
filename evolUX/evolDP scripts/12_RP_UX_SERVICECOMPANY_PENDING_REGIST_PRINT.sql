IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RP_UX_SERVICECOMPANY_PENDING_REGIST_PRINT]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RP_UX_SERVICECOMPANY_PENDING_REGIST_PRINT] AS'
END
GO
ALTER  PROCEDURE [dbo].[RP_UX_SERVICECOMPANY_PENDING_REGIST_PRINT]
	@RunID int,
	@ServiceCompanyList IDList READONLY
AS
BEGIN
	SET NOCOUNT ON
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

	SELECT fl1.OutputName as [FileName],
		f.RunID as [RunID],
		f.FileID as [FileID],
		c.CompanyCode as [ServiceCompanyCode],
		fl2.EndTimeStamp as [SentToPrinterTimeStamp],
		fl2.OutputName [Operator],
		fl2.OutputPath [Printer]
	FROM 	
		RT_RUN r WITH(NOLOCK)
	INNER JOIN 
		RT_FILE_REGIST f WITH(NOLOCK)
	ON ISNULL(r.Closed,0) = 0 AND r.RunID = @RunID 
		AND f.RunID = r.RunID
		AND f.ErrorID = 0
		AND NOT EXISTS (SELECT TOP 1 1 
				FROM RT_FILE_LOG WITH(NOLOCK) 
				WHERE RunID = f.RunID 
					AND FileID = f.FileID
					AND ErrorID = 0
					AND RunStateID = @PrintedStateID
					AND EndTimeStamp is NOT NULL)
	INNER JOIN
		RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	ON	f.ProdDetailID = pd.ProdDetailID
		AND pd.ServiceCompanyID in (SELECT ID FROM @ServiceCompanyList)
	INNER JOIN
		RD_COMPANY c WITH(NOLOCK)
	ON c.CompanyID = pd.ServiceCompanyID
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
						AND RunStateID = fl1.RunStateID)
	INNER JOIN 
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
						AND RunStateID = fl2.RunStateID)

END

GO


