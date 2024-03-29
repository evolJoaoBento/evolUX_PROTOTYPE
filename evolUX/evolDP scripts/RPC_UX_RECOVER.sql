USE [DMS_evolDP]
GO
/****** Object:  StoredProcedure [dbo].[RPC_UX_RECOVER]    Script Date: 9/19/2023 11:24:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[RPC_UX_RECOVER]
	@StartDateStr varchar(8),
	@EndDateStr varchar(8)
AS
	SET NOCOUNT ON

	DECLARE @StartDate int,
		@EndDate int
		@SQLString nvarchar(4000)

	SELECT @StartDate = 00000000, @StartDateStr = RTRIM(@StartDateStr)
	IF (LEN(@StartDateStr) > 0)
		SELECT @StartDate = CAST(@StartDateStr as int)

	SELECT @EndDate = 99999999, @EndDateStr = RTRIM(@EndDateStr)
	IF (LEN(@EndDateStr) > 0)
		SELECT @EndDate = CAST(@EndDateStr as int)

	CREATE TABLE #RP_UX_RECOVER(
		[Link] varchar(1000)  NULL,
		[Title] varchar(200) NULL,
		[Order] int NOT NULL,
		[BusinessID] int NULL,
		[RunState] int NULL,
		[BusinessDesc] varchar(256) NULL,
		[Ficheiro] varchar(256) NULL,
		[StartSeqNum] varchar(20) NULL,
		[EndSeqNum] varchar(20) NULL,
		[Quantity] int NULL,
		[Reason] varchar(256) NULL,
		[UserName] varchar(50) NULL,
		[Data] varchar(10) NULL,
		[Hora] varchar(20) NULL)

	INSERT INTO #RP_UX_RECOVER(Link,Title,[Order],[RunState],[BusinessID], [BusinessDesc],[Ficheiro],[StartSeqNum],[EndSeqNum],[Quantity],[Reason],[UserName], [Data], [Hora])
	SELECT NULL,NULL,1,CASE WHEN fl.EndTimeStamp is NULL THEN 1 ELSE 2 END ,
		b.BusinessID, b.[Description], fl2.[OutputName], 
		CASE WHEN rr.StartPostObjID = 0 THEN 'Primeiro' ELSE CAST(spo.PostObjFileSeq as varchar) END, 
		CASE WHEN rr.EndPostObjID = 0 THEN 'Último' ELSE CAST(epo.PostObjFileSeq as varchar) END, 
		SUM(CASE 
			WHEN (po.PostObjFileSeq between spo.PostObjFileSeq AND epo.PostObjFileSeq) 
				THEN 1 
			WHEN (rr.StartPostObjID = 0 AND rr.EndPostObjID = 0)
				THEN 1
			WHEN (rr.StartPostObjID = 0 AND po.PostObjFileSeq<=epo.PostObjFileSeq)
				THEN 1
			WHEN (rr.EndPostObjID = 0 AND po.PostObjFileSeq>=spo.PostObjFileSeq)
				THEN 1
			ELSE 0 END), 
		rr.Reason, rr.UserName, CONVERT(varchar,rr.RegistrationTimeStamp,111),
		CONVERT(varchar,rr.RegistrationTimeStamp,108)
	FROM RT_RECOVER_REGIST rr WITH(NOLOCK)
		INNER JOIN
		RT_RUN r WITH(NOLOCK)
	ON r.RunID = rr.RunID AND CONVERT(varchar,rr.RegistrationTimeStamp,112) between @StartDate and @EndDate
		INNER JOIN
		RD_BUSINESS b WITH(NOLOCK)
	ON	r.BusinessID = b.BusinessID
		LEFT OUTER JOIN
		RT_POSTAL_OBJECT spo WITH(NOLOCK)
	ON rr.RunID = spo.PostObjRunID AND rr.FileID = spo.PostObjFileID AND spo.PostObjID = rr.StartPostObjID
		LEFT OUTER JOIN
		RT_POSTAL_OBJECT epo WITH(NOLOCK)
	ON rr.RunID = epo.PostObjRunID AND rr.FileID = epo.PostObjFileID AND epo.PostObjID = rr.EndPostObjID
		LEFT OUTER JOIN
		RT_POSTAL_OBJECT po WITH(NOLOCK)
	ON rr.RunID = po.PostObjRunID AND rr.FileID = po.PostObjFileID
		LEFT OUTER JOIN
		RT_FILE_LOG fl WITH(NOLOCK)
	ON fl.RunID = rr.RunID and fl.FileID = rr.FileID AND rr.RegistrationTimeStamp>fl.EndTimeStamp 
			AND fl.EndTimeStamp is NOT NULL
			AND fl.RunStateID = (SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'PRINTED')
			AND fl.ProcCountNr = (SELECT MAX(ProcCountNr) FROM RT_FILE_LOG WITH(NOLOCK)
						WHERE RunID = fl.RunID AND FileID = fl.FileID AND RunStateID = fl.RunStateID)
		LEFT OUTER JOIN
		RT_FILE_LOG fl2 WITH(NOLOCK)
	ON fl2.RunID = rr.RunID and fl2.FileID = rr.FileID
			AND fl2.EndTimeStamp is NOT NULL
			AND fl2.RunStateID = (SELECT RunStateID FROM RD_RUN_STATE WHERE RunStateName = 'DOC1PCE')
			AND fl2.ProcCountNr = (SELECT MAX(ProcCountNr) FROM RT_FILE_LOG WITH(NOLOCK)
						WHERE RunID = fl2.RunID AND FileID = fl2.FileID AND RunStateID = fl2.RunStateID)
	GROUP BY fl.EndTimeStamp, b.BusinessID, b.[Description], fl2.[OutputName], spo.PostObjFileSeq, epo.PostObjFileSeq, rr.Reason, rr.UserName,rr.RegistrationTimeStamp, rr.StartPostObjID,rr.EndPostObjID

	IF (EXISTS(SELECT TOP 1 * FROM #RP_UX_RECOVER))
	BEGIN
		INSERT INTO #RP_UX_RECOVER(Link, [Order],[RunState])
		SELECT '<TR class="lttableheader" align="middle">
				<TD colspan="9">Relatório de Recuperações ' + 
				CASE WHEN @StartDateStr <>'' THEN 'de ' + @StartDateStr + ' ' ELSE '' END +  'até ' +
				CASE WHEN @EndDateStr<>'' THEN @EndDateStr ELSE REPLACE(CONVERT(varchar,CURRENT_TIMESTAMP,102),'.','') END +  '</TD>
			</TR>
			<TR class="lttableheader" align="middle">
				<TD rowspan="2">Area de Negócio</TD>
				<TD rowspan="2">Ficheiro</TD>
				<TD colspan="3">Objectos Postais</TD>
				<TD rowspan="2">Motivo</TD>
				<TD rowspan="2">Operador</TD>
				<TD colspan="2">Registo</TD>
			</TR>
			<TR class="lttableheader" align="middle">
				<TD>Nº Seq. Inicial</TD>
				<TD>Nº Seq. Final</TD>
				<TD>Quantidade</TD>
				<TD>Data</TD>
				<TD>Hora</TD>
			</TR>', 0, 0

		IF (EXISTS (SELECT TOP 1 * FROM #RP_UX_RECOVER WHERE RunState = 1))
		BEGIN
			INSERT INTO #RP_UX_RECOVER(Link,Title,[Order],[RunState],[BusinessID], [BusinessDesc])
			SELECT NULL,
				NULL,
				2147483647,
				0, 0, 
				'IMPRESSÃO'
		END

		IF (EXISTS (SELECT TOP 1 * FROM #RP_UX_RECOVER WHERE RunState = 2))
		BEGIN
			INSERT INTO #RP_UX_RECOVER(Link,Title,[Order],[RunState],[BusinessID], [BusinessDesc])
			SELECT NULL,
				NULL,
				2147483647,
				1, 0, 
				'ENVELOPAGEM'
		END
	END
	ELSE
	BEGIN
		INSERT INTO #RP_UX_RECOVER(Link, [Order])
		SELECT '<TR class="lttableheader" align="middle">
				<TD>Não existem Recuperações registadas ' + 
						CASE WHEN @StartDateStr <>'' THEN 'de ' + @StartDateStr + ' ' ELSE '' END +  'até ' +
						CASE WHEN @EndDateStr<>'' THEN CONVERT(varchar,CURRENT_TIMESTAMP,112) END +  '</TD>
			</TR>', 0
	END

	SELECT *
	FROM #RP_UX_RECOVER
	ORDER BY RunState ASC, [Order] ASC, BusinessID ASC

	DROP TABLE #RP_UX_RECOVER

	SET NOCOUNT OFF

