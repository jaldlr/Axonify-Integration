CREATE TABLE [dbo].[API_Category]
(
	[categoryExternalId] NVARCHAR(128) NOT NULL PRIMARY KEY, 
    [categoryName] NVARCHAR(60) NOT NULL, 
    [revision] NVARCHAR(128) NULL
)
