ALTER PROCEDURE [dbo].[RT_MAKE_EXPEDITION_REPORT] 
	@ClientNr int = NULL,
	@BusinessCode varchar(10) = NULL,
	@CompanyCode varchar(6) = NULL,
	@OutputPath varchar(256) = NULL,
	@FileList varchar(3500) = NULL,
	@ExpReportStartNr smallint = 5000,
	@ServiceCompanyID int = NULL,
	@RequestID int = NULL
--WITH ENCRYPTION
AS
	SET NOCOUNT ON

	DECLARE @ExpeditionTypeID int,
			@ExpeditionRunStateID int

	SELECT @ExpeditionTypeID = ServiceTypeID
	FROM RD_SERVICE_TYPE
	WHERE ServiceTypeCode = 'EXPEDITION'

	SELECT @ExpeditionRunStateID = RunStateID
	FROM RD_RUN_STATE
	WHERE RunStateName = 'EXPEDITION'

	IF (EXISTS(SELECT * 
				FROM RT_EXPEDITION_REPORT er 
				WHERE er.ExpTimeStamp is NULL
					AND NOT EXISTS(SELECT TOP 1 * 
							FROM RT_FILE_EXPEDITION_REPORT WHERE ExpReportID = er.ExpReportID)))
	BEGIN
		DELETE RT_EXPEDITION_REPORT
		FROM RT_EXPEDITION_REPORT er 
		WHERE er.ExpTimeStamp is NULL
			AND NOT EXISTS(SELECT TOP 1 * 
					FROM RT_FILE_EXPEDITION_REPORT WHERE ExpReportID = er.ExpReportID)
	END

	IF (LEN(RTRIM(@FileList)) = 0)
		SET @FileList = NULL

	--Dar ficheiros por expedidos caso não sejam para envelopar
	INSERT INTO RT_FILE_LOG(RunID, FileID, RunStateID, ProcCountNr, StartTimeStamp, EndTimeStamp, ErrorID)
	SELECT f.RunID, f.FileID, rs.RunStateID, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, f.ErrorID
	FROM 
		RT_FILE_REGIST f WITH(NOLOCK)
	INNER JOIN
		RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
	ON	f.ProdDetailID = pd.ProdDetailID
	INNER JOIN
		RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
	ON	pd.ExpCode = est.ExpCode
	WHERE NOT EXISTS (SELECT TOP 1 1 
				FROM RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
				INNER JOIN
					dbo.RD_SERVICE_TYPE st WITH(NOLOCK)
				ON	stst.ServiceTypeID = st.ServiceTypeID
				WHERE stst.ServiceTaskID = est.ServiceTaskID
					AND st.ServiceTypeCode like 'FULLFILL%')
		AND NOT EXISTS (SELECT RunID 
					FROM RT_FILE_LOG WITH(NOLOCK) 
					WHERE RunID = f.RunID 
						AND FileID = f.FileID
						AND RunStateID = @ExpeditionRunStateID
						AND EndTimeStamp is NOT NULL
						AND ErrorID = 0)
		AND EXISTS (SELECT RunID 
					FROM RT_FILE_LOG WITH(NOLOCK) 
					WHERE RunID = f.RunID 
						AND FileID = f.FileID
						AND RunStateID = (SELECT RunStateID FROM RD_RUN_STATE WITH(NOLOCK) WHERE RunStateName = 'PRINTED') 
						AND EndTimeStamp is NOT NULL
						AND ErrorID = 0)

	DECLARE @ExpTimeStamp datetime,
		@ExpReportDate smallint,
		@ExpReportNr smallint,
		@ExpRegistReportNr smallint,
		@CompanyID int,
		@BusinessID int,
		@ExpTime char(15)

	IF (@BusinessCode = 'BS')
		SET @BusinessCode = NULL

	SELECT @ExpTimeStamp = CURRENT_TIMESTAMP

	--Data Juliana
	SELECT @ExpReportDate = SUBSTRING(CAST((YEAR(@ExpTimeStamp)) as varchar(4)),4,1) + RIGHT(REPLICATE('0',3) + CAST(DATEDIFF(DAY,CONVERT(datetime,(CAST((YEAR(@ExpTimeStamp)-1) as varchar(4))+'1231'),112),@ExpTimeStamp) as varchar),3),
		@ExpTime = REPLACE(REPLACE(REPLACE(CONVERT(varchar,@ExpTimeStamp,120),' ','_'),'-',''),':','')

	DECLARE @RunID int, 
		@FileID int,
		@ExpCompanyID int,
		@ExpReportID int,
		@OldServiceCompanyID int,
		@OldExpCompanyID int,
		@ContractID int,
		@OldContractID int,
		@OldCompanyID int,
		@OldBusinessID int,
		@RegistMode bit,
		@ExpRegistReportID numeric(18,0),
		@CheckDigit int,
		@CompanyRegistCode int,
		@OldRequestID int

	IF (@RequestID is NULL)
	BEGIN
		IF (@FileList is NULL)
		BEGIN
			IF ((SELECT COUNT(*) FROM RTT_FILE_EXPEDITION_REPORT_LIST) = 0)
			BEGIN
				DECLARE rCursor CURSOR LOCAL FAST_FORWARD
				FOR SELECT f1.RunID, f1.FileID, f1.ExpCompanyID, f1.CompanyID, eec.ContractID, f1.BusinessID, f1.RegistMode, 0, f1.ServiceCompanyID
				FROM (SELECT ff.RunID, ff.FileID, est.ExpCompanyID, c.CompanyID, b.BusinessID, pd.EnvMediaID, et.RegistMode, pd.ServiceCompanyID
						FROM VW_FULLFILLED_FILE ff
						INNER JOIN
							RT_FILE_REGIST f WITH(NOLOCK)
						ON	ff.RunID = f.RunID AND ff.FileID = f.FileID
						INNER JOIN
							RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
						ON	f.ProdDetailID = pd.ProdDetailID
						INNER JOIN
							RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
						ON	est.ExpCode = pd.ExpCode
						INNER JOIN
							RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
						ON	stst.ServiceTaskID = est.ServiceTaskID
						INNER JOIN
							RD_EXPCOMPANY_TYPE et WITH(NOLOCK)
						ON	et.ExpCompanyID = est.ExpCompanyID
							AND et.ExpeditionType = pd.ExpeditionType
						INNER JOIN
							RD_ENVMEDIA_EXPCOMPANY_CONTRACT eec
						ON eec.EnvMediaID = pd.EnvMediaID
							AND eec.ExpCompanyID = est.ExpCompanyID
						INNER JOIN
							RD_EXPCOMPANY_CONTRACT ec
						ON ec.ExpCompanyID = eec.ExpCompanyID
							AND ec.ContractID = eec.ContractID
						INNER JOIN
							RT_RUN r WITH(NOLOCK)
						ON	r.RunID = f.RunID
						INNER JOIN
							RD_BUSINESS b WITH(NOLOCK)
						ON	r.BusinessID = b.BusinessID
						INNER JOIN
							RD_COMPANY c WITH(NOLOCK)
						ON	c.CompanyID = b.CompanyID
						WHERE stst.ServiceTypeID = @ExpeditionTypeID
							AND f.ErrorID = 0
							AND (@ServiceCompanyID is NULL OR pd.ServiceCompanyID = @ServiceCompanyID)
							AND (@ClientNr is NULL OR ec.ClientNr = @ClientNr)
							AND (@BusinessCode is NULL OR b.BusinessCode = @BusinessCode)
							AND (@CompanyCode is NULL OR c.CompanyCode = @CompanyCode)) f1
					LEFT OUTER JOIN  
						RT_FILE_LOG f2
					ON 	f2.RunID = f1.RunID
						AND f2.FileID = f1.FileID
						AND f2.RunStateID = @ExpeditionRunStateID
						AND f2.EndTimeStamp is NOT NULL --20230129
						AND f2.ErrorID = 0 --20230129
				WHERE f2.RunID is NULL
				ORDER BY f1.ServiceCompanyID, f1.ExpCompanyID, f1.CompanyID, f1.BusinessID, eec.ContractID, f1.RegistMode DESC
			END
			ELSE
			BEGIN
				DECLARE rCursor CURSOR LOCAL FAST_FORWARD
				FOR SELECT f1.RunID, f1.FileID, f1.ExpCompanyID, f1.CompanyID, eec.ContractID, f1.BusinessID, f1.RegistMode, f1.RequestID, f1.ServiceCompanyID
				FROM (SELECT ff.RunID, ff.FileID, est.ExpCompanyID, c.CompanyID, b.BusinessID, pd.EnvMediaID, et.RegistMode, pd.ServiceCompanyID, fn.RequestID
						FROM VW_FULLFILLED_FILE ff
						INNER JOIN
							RT_FILE_REGIST f WITH(NOLOCK)
						ON	ff.RunID = f.RunID AND ff.FileID = f.FileID
						INNER JOIN
							RTT_FILE_EXPEDITION_REPORT_LIST fn
						ON fn.RunID = f.RunID AND fn.FileID = f.FileID
						INNER JOIN
							RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
						ON	f.ProdDetailID = pd.ProdDetailID
						INNER JOIN
							RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
						ON	est.ExpCode = pd.ExpCode
						INNER JOIN
							RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
						ON	stst.ServiceTaskID = est.ServiceTaskID
						INNER JOIN
							RD_EXPCOMPANY_TYPE et WITH(NOLOCK)
						ON	et.ExpCompanyID = est.ExpCompanyID
							AND et.ExpeditionType = pd.ExpeditionType
						INNER JOIN
							RD_ENVMEDIA_EXPCOMPANY_CONTRACT eec
						ON eec.EnvMediaID = pd.EnvMediaID
							AND eec.ExpCompanyID = est.ExpCompanyID
						INNER JOIN
							RD_EXPCOMPANY_CONTRACT ec
						ON ec.ExpCompanyID = eec.ExpCompanyID
							AND ec.ContractID = eec.ContractID
						INNER JOIN
							RT_RUN r WITH(NOLOCK)
						ON	r.RunID = f.RunID
						INNER JOIN
							RD_BUSINESS b WITH(NOLOCK)
						ON	r.BusinessID = b.BusinessID
						INNER JOIN
							RD_COMPANY c WITH(NOLOCK)
						ON	c.CompanyID = b.CompanyID
						WHERE stst.ServiceTypeID = @ExpeditionTypeID
							AND f.ErrorID = 0
							AND (@ServiceCompanyID is NULL OR pd.ServiceCompanyID = @ServiceCompanyID)) f1
					LEFT OUTER JOIN  
						RT_FILE_LOG f2
					ON 	f2.RunID = f1.RunID
						AND f2.FileID = f1.FileID
						AND f2.RunStateID = @ExpeditionRunStateID
						AND f2.EndTimeStamp is NOT NULL --20230129
						AND f2.ErrorID = 0 --20230129
				WHERE f2.RunID is NULL
				ORDER BY f1.ServiceCompanyID, f1.ExpCompanyID, f1.CompanyID, f1.BusinessID, eec.ContractID, f1.RegistMode DESC
			END
		END
		ELSE
		BEGIN
			DECLARE @SQLstr nvarchar(4000)
			DECLARE @iFileList int
		
			EXEC sp_xml_preparedocument @iFileList OUTPUT, @FileList
			DECLARE rCursor CURSOR LOCAL FAST_FORWARD
			FOR SELECT f1.RunID, f1.FileID, f1.ExpCompanyID, f1.CompanyID, eec.ContractID, f1.BusinessID, f1.RegistMode, 0, f1.ServiceCompanyID
			FROM (SELECT ff.RunID, ff.FileID, est.ExpCompanyID, c.CompanyID, b.BusinessID, pd.EnvMediaID, et.RegistMode, pd.ServiceCompanyID
					FROM VW_FULLFILLED_FILE ff
					INNER JOIN
						RT_FILE_REGIST f WITH(NOLOCK)
					ON	ff.RunID = f.RunID AND ff.FileID = f.FileID
					INNER JOIN
						(SELECT * 
							FROM OPENXML (@iFileList, '/FILE_LIST/FILE',1) 
							WITH (RunID int '@R', 
								FileID int '@F')) fn
					ON fn.RunID = f.RunID AND fn.FileID = f.FileID
					INNER JOIN
						RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
					ON	f.ProdDetailID = pd.ProdDetailID
					INNER JOIN
						RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
					ON	est.ExpCode = pd.ExpCode
					INNER JOIN
						RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
					ON	stst.ServiceTaskID = est.ServiceTaskID
					INNER JOIN
						RD_EXPCOMPANY_TYPE et WITH(NOLOCK)
					ON	et.ExpCompanyID = est.ExpCompanyID
						AND et.ExpeditionType = pd.ExpeditionType
					INNER JOIN
						RD_ENVMEDIA_EXPCOMPANY_CONTRACT eec
					ON eec.EnvMediaID = pd.EnvMediaID
						AND eec.ExpCompanyID = est.ExpCompanyID
					INNER JOIN
						RD_EXPCOMPANY_CONTRACT ec
					ON ec.ExpCompanyID = eec.ExpCompanyID
						AND ec.ContractID = eec.ContractID
					INNER JOIN
						RT_RUN r WITH(NOLOCK)
					ON	r.RunID = f.RunID
					INNER JOIN
						RD_BUSINESS b WITH(NOLOCK)
					ON	r.BusinessID = b.BusinessID
					INNER JOIN
						RD_COMPANY c WITH(NOLOCK)
					ON	c.CompanyID = b.CompanyID
					WHERE stst.ServiceTypeID = @ExpeditionTypeID
						AND f.ErrorID = 0) f1
				LEFT OUTER JOIN  
					RT_FILE_LOG f2
				ON 	f2.RunID = f1.RunID
					AND f2.FileID = f1.FileID
					AND f2.RunStateID = @ExpeditionRunStateID
					AND f2.EndTimeStamp is NOT NULL --20230129
					AND f2.ErrorID = 0 --20230129
			WHERE f2.RunID is NULL
			ORDER BY f1.ServiceCompanyID, f1.ExpCompanyID, f1.CompanyID, f1.BusinessID, eec.ContractID, f1.RegistMode DESC
		END
	END
	ELSE
	BEGIN
		DECLARE rCursor CURSOR LOCAL FAST_FORWARD
		FOR SELECT f1.RunID, f1.FileID, f1.ExpCompanyID, f1.CompanyID, eec.ContractID, f1.BusinessID, f1.RegistMode, f1.RequestID, f1.ServiceCompanyID
		FROM (SELECT ff.RunID, ff.FileID, est.ExpCompanyID, c.CompanyID, b.BusinessID, pd.EnvMediaID, et.RegistMode, pd.ServiceCompanyID, fn.RequestID
				FROM VW_FULLFILLED_FILE ff
				INNER JOIN
					RT_FILE_REGIST f WITH(NOLOCK)
				ON	ff.RunID = f.RunID AND ff.FileID = f.FileID
				INNER JOIN
					RTT_FILE_EXPEDITION_REPORT_LIST fn
				ON fn.RunID = f.RunID AND fn.FileID = f.FileID
				INNER JOIN
					RT_PRODUCTION_DETAIL pd WITH(NOLOCK)
				ON	f.ProdDetailID = pd.ProdDetailID
				INNER JOIN
					RD_EXPCOMPANY_SERVICE_TASK est WITH(NOLOCK)
				ON	est.ExpCode = pd.ExpCode
				INNER JOIN
					RD_SERVICE_TASK_SERVICE_TYPE stst WITH(NOLOCK)
				ON	stst.ServiceTaskID = est.ServiceTaskID
				INNER JOIN
					RD_EXPCOMPANY_TYPE et WITH(NOLOCK)
				ON	et.ExpCompanyID = est.ExpCompanyID
					AND et.ExpeditionType = pd.ExpeditionType
				INNER JOIN
					RD_ENVMEDIA_EXPCOMPANY_CONTRACT eec
				ON eec.EnvMediaID = pd.EnvMediaID
					AND eec.ExpCompanyID = est.ExpCompanyID
				INNER JOIN
					RD_EXPCOMPANY_CONTRACT ec
				ON ec.ExpCompanyID = eec.ExpCompanyID
					AND ec.ContractID = eec.ContractID
				INNER JOIN
					RT_RUN r WITH(NOLOCK)
				ON	r.RunID = f.RunID
				INNER JOIN
					RD_BUSINESS b WITH(NOLOCK)
				ON	r.BusinessID = b.BusinessID
				INNER JOIN
					RD_COMPANY c WITH(NOLOCK)
				ON	c.CompanyID = b.CompanyID
				WHERE stst.ServiceTypeID = @ExpeditionTypeID
					AND f.ErrorID = 0 AND fn.RequestID = @RequestID) f1
			LEFT OUTER JOIN  
				RT_FILE_LOG f2
			ON 	f2.RunID = f1.RunID
				AND f2.FileID = f1.FileID
				AND f2.RunStateID = @ExpeditionRunStateID
				AND f2.EndTimeStamp is NOT NULL --20230129
				AND f2.ErrorID = 0 --20230129
		WHERE f2.RunID is NULL
		ORDER BY f1.ServiceCompanyID, f1.ExpCompanyID, f1.CompanyID, f1.BusinessID, eec.ContractID, f1.RegistMode DESC
	END

	OPEN rCursor
	FETCH NEXT FROM rCursor INTO @RunID, @FileID, @ExpCompanyID, @CompanyID,  @ContractID, @BusinessID, @RegistMode, @RequestID, @ServiceCompanyID

	SELECT @OldExpCompanyID = 0, @OldContractID = NULL, @OldBusinessID = 0, @OldCompanyID = 0, @OldRequestID = -1, @OldServiceCompanyID = 0

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (EXISTS (SELECT TOP 1 1 FROM RT_POSTAL_OBJECT 
			WHERE PostObjRunID = @RunID AND PostObjFileID = @FileID
				AND ErrorID = 0 --20230129
				AND ExpCompanyLevel is NULL))
		BEGIN
			EXEC RT_CALCULATE_COSTS @RunID, NULL, @FileID
		END

		BEGIN TRANSACTION
		IF (@OldRequestID <> @RequestID
			OR @OldServiceCompanyID<>@ServiceCompanyID
			OR @OldExpCompanyID<>@ExpCompanyID 
			OR @OldCompanyID<>@CompanyID
			OR @OldBusinessID<>@BusinessID
			OR @OldContractID<>@ContractID 
			OR (@ContractID is NULL AND @OldContractID is NOT NULL)
			OR (@ContractID is NOT NULL AND @OldContractID is NULL))
		BEGIN
			DELETE RTT_FILE_EXPEDITION_REPORT_LIST
			WHERE RequestID = @OldRequestID

			SELECT @ExpReportID = MAX(ExpReportID)
			FROM RTT_FILE_EXPEDITION_REPORT_LIST
			WHERE RequestID = @RequestID

			IF (@ExpReportID is NOT NULL)
			BEGIN
				SELECT @ExpReportNr = ExpReportNr,
					@ExpRegistReportNr = ExpRegistReportNr,
					@ExpRegistReportID = ExpRegistReportID
				FROM RT_EXPEDITION_REPORT
				WHERE ExpReportID = @ExpReportID

				UPDATE RT_EXPEDITION_REPORT
				SET ExpTime = @ExpTime
				WHERE ExpReportID = @ExpReportID
			END
			ELSE
			BEGIN
				SELECT @CompanyRegistCode = CompanyRegistCode%100000000
				FROM RD_EXPEDITION_ID
				WHERE ExpCompanyID = @ExpCompanyID
	
				SELECT @ExpReportID = ISNULL(MAX(ExpReportID),0) + 1
				FROM RT_EXPEDITION_REPORT
		
				INSERT INTO RT_EXPEDITION_REPORT(ExpReportID, ExpCompanyID, ContractID, ExpReportDate, ExpReportNr)
				SELECT @ExpReportID, @ExpCompanyID, @ContractID, @ExpReportDate,  0
	
				SELECT @ExpReportNr = ISNULL(MAX(ExpReportNr),@ExpReportStartNr) + 1
				FROM RT_EXPEDITION_REPORT
				WHERE ExpCompanyID = @ExpCompanyID 
					AND ContractID = @ContractID 
					-- Fix para BusinessID's diferentes com o mesmo Contrato
					AND (ExpTimeStamp is NULL OR DATEPART(YEAR,ExpTimeStamp) = DATEPART(YEAR,@ExpTimeStamp))

				-- LOP-20140729 - Porque Contratos Diferentes não podem ter o mesmo nr de Guia de Registos
				SELECT @ExpRegistReportNr = ISNULL(MAX(ISNULL(ExpRegistReportNr,0)),@ExpReportStartNr) + 1
				FROM RT_EXPEDITION_REPORT
				WHERE ExpCompanyID = @ExpCompanyID 
					AND (ExpTimeStamp is NULL OR DATEPART(YEAR,ExpTimeStamp) = DATEPART(YEAR,@ExpTimeStamp))

				IF (@RegistMode = 1)
				BEGIN
					SELECT @CheckDigit = 11 - (8*2 + 8*7 
						+ (@CompanyRegistCode%1000000 - @CompanyRegistCode%100000)/100000 * 6
						+ (@CompanyRegistCode%100000 - @CompanyRegistCode%10000)/10000 * 5
						+ (@CompanyRegistCode%10000 - @CompanyRegistCode%1000)/1000 * 4
						+ (@CompanyRegistCode%1000 - @CompanyRegistCode%100)/100 * 3
						+ (@CompanyRegistCode%100 - @CompanyRegistCode%10)/10 * 2
						+ @CompanyRegistCode%10 * 7
						+ DATEPART(YEAR,@ExpTimeStamp)%10 * 6
						+ (@ExpRegistReportNr%10000 - @ExpRegistReportNr%1000)/1000 * 5
						+ (@ExpRegistReportNr%1000 - @ExpRegistReportNr%100)/100 * 4
						+ (@ExpRegistReportNr%100 - @ExpRegistReportNr%10)/10 * 3
						+ @ExpRegistReportNr%10 * 2) % 11
	
					--Fix 1.0.3.12 - START
					SELECT @CheckDigit = CASE WHEN @CheckDigit = 10 THEN 0 WHEN @CheckDigit = 11 THEN 0 ELSE @CheckDigit END
					--Fix 1.0.3.12 - END
	
					SELECT @ExpRegistReportID = 88000000000000 + CAST((@CompanyRegistCode%1000000) as numeric(18,0)) * 1000000
							+ CAST((DATEPART(YEAR,@ExpTimeStamp)%10) as numeric(18,0))* 100000
							+ CAST(@ExpRegistReportNr as numeric(18,0)) * 10
							+ CAST(@CheckDigit as numeric(18,0))
	
					UPDATE RT_EXPEDITION_REPORT
					SET ExpReportNr = @ExpReportNr, ExpTime = @ExpTime,
						ExpRegistReportNr = @ExpRegistReportNr,
						ExpRegistReportID = @ExpRegistReportID
					WHERE ExpCompanyID = @ExpCompanyID AND ExpReportID = @ExpReportID
				END
				ELSE
				BEGIN
					UPDATE RT_EXPEDITION_REPORT
					SET ExpReportNr = @ExpReportNr, ExpTime = @ExpTime
					WHERE ExpCompanyID = @ExpCompanyID AND ExpReportID = @ExpReportID
				END
			END
			SELECT @OldServiceCompanyID = @ServiceCompanyID, @OldExpCompanyID = @ExpCompanyID, 
				@OldContractID = @ContractID, @OldBusinessID = @BusinessID, 
				@OldCompanyID = @CompanyID, @OldRequestID = @RequestID
		END

		INSERT INTO RT_FILE_EXPEDITION_REPORT(RunID, FileID, ExpCompanyLevel, ExpDate, ExpWeight, Quantity, Cost, ExpReportID)
		SELECT PostObjRunID, PostObjFileID, ExpCompanyLevel, MAX(ExpDate), SUM(ISNULL(PostObjWeight,0)), COUNT(PostObjID), SUM(ISNULL(ExpeditionCost,0)), @ExpReportID
		FROM RT_POSTAL_OBJECT
		WHERE PostObjRunID = @RunID AND PostObjFileID = @FileID
		-- Não contabilizar Objectos Postais em Erro (20050922)
			AND ErrorID = 0
		GROUP BY PostObjRunID, PostObjFileID, ExpCompanyLevel

		UPDATE RTT_FILE_EXPEDITION_REPORT_LIST
		SET ExpReportID = @ExpReportID
		WHERE RunID = @RunID AND FileID = @FileID AND RequestID = @RequestID

		INSERT INTO RT_FILE_LOG (RunID, FileID, RunStateID, ProcCountNr, StartTimeStamp, EndTimeStamp, OutputPath, OutputName)
		SELECT @RunID, @FileID, @ExpeditionRunStateID, ISNULL(MAX(ProcCountNr),0) + 1 , CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 
			ISNULL(@OutputPath,''), 'XEXPREP_'+ @ExpTime + '_' + CAST(@ExpReportID as varchar) +'.REP'
		FROM RT_FILE_LOG fl
		WHERE RunID = @RunID AND FileID = @FileID AND RunStateID = @ExpeditionRunStateID
	
		COMMIT TRANSACTION
		FETCH NEXT FROM rCursor INTO @RunID, @FileID, @ExpCompanyID, @CompanyID,  @ContractID, @BusinessID, @RegistMode, @RequestID, @ServiceCompanyID
	END

	CLOSE rCursor
	DEALLOCATE rCursor

	SET NOCOUNT OFF

	SELECT er.ExpReportID, er.ExpCompanyID, er.ExpTime, ISNULL(@OutputPath,'') + 'XEXPREP_'+ er.ExpTime + '_' + CAST(er.ExpReportID as varchar) +'.REP' as ExpFileName, 
		ISNULL(@OutputPath,'') + 'RMC' + REPLICATE('0',8-LEN(CAST(ei.CompanyRegistCode%100000000 as varchar))) + CAST(ei.CompanyRegistCode%100000000 as varchar) +  '.' + REPLICATE('0',3 - LEN(CAST((er.ExpRegistReportNr%1000) as varchar))) + CAST((er.ExpRegistReportNr%1000) as varchar) as ExpRegistReport, 
		REPLICATE('0',8-LEN(CAST(ei.CompanyRegistCode%100000000 as varchar))) + CAST(ei.CompanyRegistCode%100000000 as varchar) CompanyRegistCode,
		SUBSTRING(er.ExpTime,1,8) ExpDate,
		SUBSTRING(er.ExpTime,10,6) ExpHour,
		CAST((er.ExpRegistReportNr%100000000) as varchar(8)) ExpRegistReportNr,
		CAST(er.ExpRegistReportID as varchar(14)) ExpRegistReportID
	FROM RT_EXPEDITION_REPORT er
		LEFT OUTER JOIN
		(SELECT e.* FROM RD_EXPEDITION_ID e WHERE e.StartExpeditionID = (SELECT MIN(StartExpeditionID) FROM RD_EXPEDITION_ID WHERE ExpCompanyID = e.ExpCompanyID AND EndExpeditionID > LastExpeditionID))ei
	ON	ei.ExpCompanyID = er.ExpCompanyID
	WHERE	er.ExpTimeStamp is NULL --AND ExpTime = @ExpTime
	ORDER BY er.ExpTime ASC
RETURN
GO