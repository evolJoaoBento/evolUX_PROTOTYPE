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