IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'IDList' AND SCHEMA_ID('dbo') = schema_id)
DROP TYPE dbo.IDList
GO
CREATE TYPE dbo.IDList AS TABLE (ID int);
GO
IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'IDPairList' AND SCHEMA_ID('dbo') = schema_id)
DROP TYPE dbo.IDPairList
GO
CREATE TYPE dbo.IDPairList AS TABLE (ID1 int, ID2 int);
GO
IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'IDCodeList' AND SCHEMA_ID('dbo') = schema_id)
DROP TYPE dbo.IDCodeList
GO
CREATE TYPE dbo.IDCodeList AS TABLE (ID int, [Code] varchar(20), [GroupCode] varchar(20));
GO
--DROP FUNCTION IF EXISTS [RDC_GetFormat4SuportType];
--GO
--CREATE FUNCTION dbo.[RDC_GetFormat4SuportType] (@FormatSuportType int, @VariableSuportType int, @Separator varchar(10), @TableName IDDescList READONLY)
--RETURNS NVARCHAR(MAX)
--AS
--BEGIN
--	DECLARE @Result NVARCHAR(MAX) = ''

--	IF (EXISTS (SELECT TOP 1 1 FROM @TableName WHERE ID = (@FormatSuportType & @VariableSuportType)))
--	BEGIN
--		SELECT @Result = @Result + [Desc] + @Separator
--		FROM @TableName
--		WHERE ID = (@FormatSuportType & @VariableSuportType) AND (ID & @FormatSuportType) > 0
--	END
--	ELSE
--	BEGIN
--		SELECT @Result = @Result + [Desc] + @Separator
--		FROM @TableName
--		WHERE (ID & @VariableSuportType) = ID AND (ID & @FormatSuportType) > 0
--	END


--	IF (LEN(@Result) > 0)
--	BEGIN
--		SELECT @Result = SUBSTRING(@Result,1, LEN(@Result)-LEN(@Separator) + 1)
--	END
--	RETURN @Result;
--END
--GO