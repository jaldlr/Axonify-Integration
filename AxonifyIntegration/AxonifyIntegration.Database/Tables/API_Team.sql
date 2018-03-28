CREATE TABLE [dbo].[API_Team]
(
	[teamId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [parentName] NVARCHAR(60) NULL, 
    [teamName] NVARCHAR(60) NULL
)
