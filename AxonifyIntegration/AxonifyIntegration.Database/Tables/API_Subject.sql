CREATE TABLE [dbo].[API_Subject]
(
	[subjectExternalId] NVARCHAR(128) NOT NULL PRIMARY KEY, 
    [categoryExternalId] NVARCHAR(128) NOT NULL, 
    [subjectName] NVARCHAR(60) NOT NULL, 
    [revision] NVARCHAR(128) NULL
)
