CREATE TABLE [dbo].[API_EmployeeTopicGraduation]
(
	[employeeId] NVARCHAR(255) NOT NULL PRIMARY KEY, 
    [categoryExternalId] NVARCHAR(60) NULL, 
    [subjectExternalId] NVARCHAR(60) NULL, 
    [topicExternalId] NVARCHAR(60) NULL, 
    [graduationTimestamp] DATETIME NULL, 
    [timeSpent] INT NULL, 
    [baselineMetricId] INT NULL, 
    [currentMetricId] INT NULL, 
    [overalMetricId] INT NULL
)
