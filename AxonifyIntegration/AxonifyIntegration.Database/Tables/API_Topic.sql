CREATE TABLE [dbo].[API_Topic]
(
	[topicExternalId] NVARCHAR(128) NOT NULL PRIMARY KEY, 
    [subjectExternalId] NVARCHAR(128) NOT NULL, 
    [topicName] NVARCHAR(60) NULL, 
    [revision] NVARCHAR(128) NOT NULL
)
