IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[evolUX_GET_JOB_BY_FlowID]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[evolUX_GET_JOB_BY_FlowID] AS'
END
GO
ALTER  PROCEDURE [dbo].[evolUX_GET_JOB_BY_FlowID]
	@FlowID int
AS
BEGIN
	SELECT j.JobID, j.RunID, j.[Priority], j.FlowID, j.RegistrationTimeStamp, j.StartTimeStamp, j.EndTimeStamp, j.JobDescription, j.JobFactor, j.Obs, j.JobStatus, s.[Description] [StateDescription]
	FROM
		JOBS j WITH(NOLOCK)
	INNER JOIN
		CURRENT_STATE cs WITH(NOLOCK)
	ON	cs.JobID = j.JobID
	INNER JOIN
		STATES s WITH(NOLOCK)
	ON s.FlowID = j.FlowID
		AND s.StateID = cs.StateID
	WHERE j.FlowID = @FlowID 
		AND ((j.EndTimeStamp is NULL
		AND j.JobStatus is NULL)
		OR EXISTS (SELECT TOP 1 1 FROM ACTIVE_TASKS a WITH(NOLOCK) WHERE a.JobID = j.JobID))
END
GO
