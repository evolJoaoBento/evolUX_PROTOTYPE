INSERT INTO [evolDP_DESCRIPTION](FieldName, FieldDescription)
SELECT 'EmailJoin', 'false'
WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolDP_DESCRIPTION] WHERE FieldName = 'EmailJoin')

INSERT INTO [evolDP_DESCRIPTION](FieldName, FieldDescription)
SELECT 'ElectronicJoin', 'false'
WHERE NOT EXISTS (SELECT TOP 1 1 FROM [evolDP_DESCRIPTION] WHERE FieldName = 'ElectronicJoin')