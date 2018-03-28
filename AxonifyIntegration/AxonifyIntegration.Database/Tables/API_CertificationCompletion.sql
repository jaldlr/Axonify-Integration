CREATE TABLE [dbo].[API_CertificationCompletion]
(
	[employeeId] NVARCHAR(255) NOT NULL PRIMARY KEY, 
    [categoryExternalId] NVARCHAR(60) NULL, 
    [subjectExternalId] NVARCHAR(60) NULL, 
    [topicExternalId] NVARCHAR(60) NULL, 
    [completionTimestamp] DATETIME NULL, 
    [assessmentScore] DECIMAL(2, 1) NULL, 
    [timeSpent] INT NULL
)
